using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Utility.Data;
using Innocellence.WeChat.Domain.Common;
using Innocellence.WeChat.Domain.CommonEntity;
using Innocellence.WeChat.Domain.Contracts;
using Innocellence.WeChat.Domain.Entity;
using Innocellence.WeChat.Domain.Interface;
using Innocellence.Weixin.QY.AdvancedAPIs.Mass;

namespace Innocellence.WeChat.Domain.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class PushService : BasePushService, IPushService
    {
        private readonly IPushHistoryService _pushHistoryService;
        private readonly IPushHistoryDetailService _historyDetailService;
        private readonly IPushFacadeService _pushFacadeService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pushHistoryService"></param>
        /// <param name="historyDetailService"></param>
        /// <param name="pushFacadeService"></param>
        public PushService(IPushHistoryService pushHistoryService,
            IPushHistoryDetailService historyDetailService,
            IPushFacadeService pushFacadeService)
            : base(pushHistoryService, historyDetailService)
        {
            _pushHistoryService = pushHistoryService;
            _historyDetailService = historyDetailService;
            _pushFacadeService = pushFacadeService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="appId"></param>
        /// <param name="platformAccountId"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Result<object> PushCheck(TargetInfoObject entity, int appId, int platformAccountId, ContentType contentType)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (appId < 0)
            {
                throw new ArgumentException("appId");
            }

            var result = new Result<object>
               {
                   Status = 200,
               };

            var history = new PushHistoryEntity
            {
                ContentId = entity.ContentId,
                AppId = appId,
                ContentType = contentType.ToString(),
                TargetType = entity.IsToAllUsers ? TargetType.AllUser.ToString() : TargetType.Config.ToString(),
                PlatformAccountId=platformAccountId
            };

            _pushHistoryService.Repository.Insert(history);

            result.Data = history.Id;

            if (entity.IsToAllUsers)
            {
                return result;
            }

            var appInfo = WeChatCommonService.GetAppInfo(appId);

            var verifyResult = _pushFacadeService.Verify(new ConfigedInfo(appInfo, platformAccountId), entity, contentType);

            var passPart = verifyResult.Data.Success;
            //string.Join(",", passPart.ToDepartments.Select(JsonHelper.ToJson)) string.Join(",", passPart.ToTags.Select(JsonHelper.ToJson))
            history.ToDepartments = (passPart.ToDepartments != null && passPart.ToDepartments.Any()) ? JsonHelper.ToJson(passPart.ToDepartments) : null;
            history.ToTags = passPart.ToTags != null ? JsonHelper.ToJson(passPart.ToTags) : null;
            history.ToUsers = passPart.ToUsers != null ? string.Join("|", passPart.ToUsers) : null;

            //updated
            _pushHistoryService.Repository.Update(history, new List<string> { "ToDepartments", "ToTags", "ToUsers" });

            if (verifyResult.Status == 200) return result;

            result.Status = verifyResult.Status;

            var errorResult = verifyResult.Data.Error;

            _historyDetailService.Repository.Insert(new PushHistoryDetailEntity
            {
                HistoryId = history.Id,
                ErrorDepartments = errorResult.ErrorDepartments != null ? JsonHelper.ToJson(errorResult.ErrorDepartments) : null,
                ErrorTags = errorResult.ErrorTags != null ? JsonHelper.ToJson(errorResult.ErrorTags) : null,
                ErrorUsers = errorResult.ErrorUsers != null ? string.Join("|", errorResult.ErrorUsers) : null,
                ErrorType = ErrorType.Checked.ToString(),
            });

            return result;
        }

        //update history after send out
        public async Task Update(MassResult messageResult, int historyId)
        {
            if (messageResult.errcode != 0)
            {
                await _historyDetailService.Repository.InsertAsync(new PushHistoryDetailEntity
                  {
                      HistoryId = historyId,
                      ErrorUsers = messageResult.invaliduser,
                      ErrorTags = messageResult.invalidtag,
                      ErrorDepartments = messageResult.invalidparty,
                      ErrorType = ErrorType.Returned.ToString()
                  });
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BasePushService
    {
        private readonly IPushHistoryService _pushHistoryService;
        private readonly IPushHistoryDetailService _historyDetailService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pushHistoryService"></param>
        /// <param name="historyDetailService"></param>
        public BasePushService(IPushHistoryService pushHistoryService, IPushHistoryDetailService historyDetailService)
        {
            _pushHistoryService = pushHistoryService;
            _historyDetailService = historyDetailService;
        }

        /// <summary>
        /// template method
        /// </summary>
        protected virtual void CreatePushHistory(PushHistoryEntity pushHistoryEntity)
        {
            _pushHistoryService.Repository.GetByKey(1);
            _historyDetailService.Repository.GetByKey(1);
        }
    }
}
