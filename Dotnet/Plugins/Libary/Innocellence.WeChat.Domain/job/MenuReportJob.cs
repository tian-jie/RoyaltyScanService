using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Binbin.Linq;
using Infrastructure.Core;
using Infrastructure.Core.Data;
using Infrastructure.Core.Logging;
using Infrastructure.Web.Domain.Service;
using Innocellence.WeChat.Domain.Common;
using Innocellence.WeChat.Domain.Contracts;
using Innocellence.WeChat.Domain.Entity;
using Innocellence.WeChat.Domain.Service;
using Innocellence.WeChat.Domain.Service.job;

namespace Innocellence.WeChat.Domain.Job
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    public class MenuReportJob : ICustomerJob
    {
        private readonly TimeSpan _interval;
        private readonly IReportJobLogService _jobLogService = new ReportJobLogService();
        private readonly IUserBehaviorService _userBehaviorService = new UserBehaviorService(new CodeFirstDbContext());
        private readonly IMenuReportService _menuReportService = new MenuReportService();
        private static readonly ILogger Log = LogManager.GetLogger(typeof(MenuReportJob));
        private readonly string _jobName = JobName.MenuReportJob.ToString();//"MenuReportJob";
        private bool _success = true;

        /// <summary>
        /// 
        /// </summary>
        public MenuReportJob()
        {
            _interval = TimeSpan.FromHours(24);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task Execute()
        {
            return new Task(RunJob);
        }

        private void RunJob()
        {
            ReportJobLog jobLog = null;

            try
            {
                jobLog = _jobLogService.CreateJobLog(_jobName);
                ImportDataByTime(jobLog.DateFrom, jobLog.DateTo);
                jobLog.JobStatus = JobStatus.Success.ToString();
            }
            catch (InnocellenceException e)
            {
                _success = false;
                if (jobLog != null)
                {
                    jobLog.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
                    jobLog.JobStatus = JobStatus.Error.ToString();
                }

                Log.Error<string>(string.Format("menu report job error:{0}" + Environment.NewLine + "{1}", e.Message, e.StackTrace));
            }
            catch (Exception e)
            {
                _success = false;
                if (jobLog != null)
                {
                    jobLog.ErrorMessage = e.InnerException == null ? e.Message : e.InnerException.Message;
                    jobLog.JobStatus = JobStatus.Error.ToString();
                }

                Log.Error<string>(string.Format("menu report job error:{0}" + Environment.NewLine + "{1}", e.Message, e.StackTrace));
            }
            finally
            {
                if (jobLog != null)
                {
                    jobLog.UpdatedDate = DateTime.Now;
                    _jobLogService.Repository.Update(jobLog);
                }
            }
        }

        /// <summary>
        /// contentType 是  4,5,6,7
        /// </summary>
        /// <param name="fromDateTime"></param>
        /// <param name="toDateTime"></param>
        private void ImportDataByTime(DateTime fromDateTime, DateTime toDateTime)
        {
            var menuReportLogs = new List<MenuReport>();
            var predicate = PredicateBuilder.True<UserBehavior>();

            predicate = predicate.And(x => x.CreatedTime <= toDateTime && x.CreatedTime > fromDateTime);

            predicate = predicate.And(x => x.ContentType == 4 || x.ContentType == 5 || x.ContentType == 6 || x.ContentType == 7);

            var logs = _userBehaviorService.Repository.Entities.Where(predicate).ToList();//.AsParallel();

            var logsFromFunctionIdColumn = logs.Where(x => x.ContentType == 4).ToList();
            var logsFromContentColumn = logs.Where(x => logsFromFunctionIdColumn.All(y => y.Id != x.Id)).ToList();

            menuReportLogs.AddRange(GetReportEntitiesFromContentColumn(logsFromContentColumn).ToList());
            menuReportLogs.AddRange(GetReportEntitiesFromFunctionColumn(logsFromFunctionIdColumn).ToList());

            _menuReportService.Repository.Insert(menuReportLogs.AsEnumerable());
        }

        private IEnumerable<MenuReport> GetReportEntitiesFromContentColumn(IEnumerable<UserBehavior> source)
        {
            //return source.GroupBy(x => x.AppId).SelectMany(appGroup => appGroup.GroupBy(x => x.Content).ToList().Select(GenerateReportEntity)).Where(entity => entity != null);

            return source.GroupBy(x => x.AppId).ToList().SelectMany(appGroup => appGroup.GroupBy(x => x.Content).ToList().SelectMany(menu => menu.GroupBy(x => x.CreatedTime.Date).Select(GenerateReportEntity).Where(entity => entity != null)));

            #region
            //foreach (var appGroup in source.GroupBy(x => x.AppId).ToList())
            //{
            //    foreach (var menu in appGroup.GroupBy(x => x.Content).ToList())
            //    {
            //        foreach (var dateGroup in menu.GroupBy(x => x.CreatedTime))
            //        {
            //            var entity = GenerateReportEntity(dateGroup);
            //            if (entity != null)
            //            {
            //                yield return entity;
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        private IEnumerable<MenuReport> GetReportEntitiesFromFunctionColumn(IEnumerable<UserBehavior> source)
        {
            return source.GroupBy(x => x.AppId).ToList().SelectMany(appGroup => appGroup.GroupBy(x => x.FunctionId).ToList().SelectMany(menu => menu.GroupBy(x => x.CreatedTime.Date).Select(GenerateReportEntity).Where(entity => entity != null)));
        }

        private static MenuReport GenerateReportEntity(IGrouping<DateTime, UserBehavior> menu)
        {
            var menuAction = menu.FirstOrDefault();
            if (menuAction == null) return null;

            var app = WeChatCommonService.lstSysWeChatConfig.FirstOrDefault(x => x.Id == menuAction.AppId);
            var appName = string.Empty;
            if (app == null)
            {
                Log.Info("没有找到app id 为{0}的app!", menuAction.AppId);
            }
            else
            {
                appName = app.AppName;
            }

            var entity = new MenuReport
            {
                AccessDate = menuAction.CreatedTime.Date,
                AppId = menuAction.AppId,
                AppName = appName,
                CreatedDate = DateTime.Now,
                MenuKey = menuAction.Content,
                VisitTimes = menu.Count(),
                VisitorCount = menu.GroupBy(x => x.UserId).Count(),
                UserId = menuAction.UserId,
            };

            if (menuAction.ContentType == 4)
            {
                entity.MenuKey = menuAction.FunctionId;
            }

            var category = CommonService.lstCategory.FirstOrDefault(x => x.CategoryCode == entity.MenuKey);
            var menuName = string.Empty;
            if (category == null)
            {
                Log.Info("没有找到key 为{0}的menu!", entity.MenuKey);
            }
            else
            {
                menuName = category.CategoryName;
            }
            entity.MenuName = menuName;
            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan Interval
        {
            get { return _interval; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return "Menu Report Job"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan Timeout
        {
            get { return TimeSpan.MaxValue; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ManuallyRunJob()
        {
            RunJob();
        }

        /// <summary>
        /// 
        /// </summary>
        public JobName JobName
        {
            get { return JobName.MenuReportJob; }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool Success
        {
            get { return _success; }
        }
    }
}
