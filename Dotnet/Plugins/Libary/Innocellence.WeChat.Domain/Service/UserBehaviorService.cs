using System.Threading.Tasks;
using Infrastructure.Core;
using Infrastructure.Core.Data;
using Infrastructure.Core.Infrastructure;
using Infrastructure.Core.Logging;
using Innocellence.WeChat.Domain.Common;
using Innocellence.WeChat.Domain.Contracts;
using Innocellence.WeChat.Domain.Contracts.ViewModel;
using Innocellence.WeChat.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Innocellence.WeChat.Domain.Service
{
    /// <summary>
    /// 用户行为日志
    /// </summary>
    public class UserBehaviorService : BaseService<UserBehavior>, IUserBehaviorService
    {
        private readonly ILogger _log = LogManager.GetLogger(typeof(UserBehavior));

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWork"></param>
        public UserBehaviorService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserBehaviorService()
            : base()
        {

        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<UserBehaviorView> GetByList(DateTime begindate, DateTime endTime)
        {

            Expression<Func<UserBehavior, bool>> predicate = n => n.CreatedTime >= begindate && n.CreatedTime <= endTime;
            var t = Repository.Entities.Where(predicate).GroupBy(a => a.AppId).Select(a => new UserBehaviorView() { AppId = a.Key, Count = a.Count() }).ToList();

            return t;
        }

        /// <summary>
        /// 获取客户端列表
        /// </summary>
        /// <param name="begindate"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<int> GetAgentList(DateTime begindate, DateTime endTime)
        {
            Expression<Func<UserBehavior, bool>> predicate = n => n.CreatedTime >= begindate && n.CreatedTime <= endTime;
            var t = Repository.Entities.Where(predicate).Select(a => a.AppId).Distinct().ToList();
            return t;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userBehavior"></param>
        /// <param name="accountId"></param>
        public void InsertUserBehavior(UserBehavior userBehavior, string accountId)
        {
            _log.Debug("entring user behavior....   accountid is {0}", accountId);
            Task.Run(() =>
                {
                    var userBehaviorService = EngineContext.Current.Resolve<IUserBehaviorService>();
                    try
                    {
                        userBehaviorService.Repository.Insert(userBehavior);
                    }
                    catch (Exception e)
                    {
                        _log.Error(e, "user behavior error:{0}", accountId);
                    }
                });
        }
    }
}
