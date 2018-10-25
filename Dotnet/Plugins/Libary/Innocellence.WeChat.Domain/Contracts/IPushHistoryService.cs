using Infrastructure.Core;
using Innocellence.WeChat.Domain.Entity;

namespace Innocellence.WeChat.Domain.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPushHistoryService : IDependency, IBaseService<PushHistoryEntity>
    {
    }
}
