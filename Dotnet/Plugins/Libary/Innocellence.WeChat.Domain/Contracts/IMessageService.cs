using System;
using System.Collections.Generic;
using Infrastructure.Core;
using Innocellence.WeChat.Domain.CommonEntity;
using Innocellence.WeChat.Domain.Entity;
using System.Linq.Expressions;
namespace Innocellence.WeChat.Domain.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageService : IDependency, IBaseService<Message>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetList<T>(Expression<Func<Message, bool>> predicate) where T : IViewModel, new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="accountMangageId"></param>
        /// <param name="selectedDepartmentIds"></param>
        /// <param name="selectedTagIds"></param>
        /// <param name="selectedWeChatUserIDs"></param>
        /// <param name="contentId">消息id</param>
        /// <param name="isToAllUsers">如果是全员发, 把selectedWeChatUserIDs参数传入全部人员id</param>
        /// <returns></returns>
        Result<object> CheckMessagePushRule(int appId, int accountMangageId, IList<int> selectedDepartmentIds, IList<int> selectedTagIds, IList<string> selectedWeChatUserIDs, int contentId, bool isToAllUsers = false);
    }
}
