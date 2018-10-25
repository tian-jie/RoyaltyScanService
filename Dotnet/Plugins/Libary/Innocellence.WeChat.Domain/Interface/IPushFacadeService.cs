using Infrastructure.Core;
using Innocellence.WeChat.Domain.CommonEntity;

namespace Innocellence.WeChat.Domain.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPushFacadeService : IDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appInfo"></param>
        /// <param name="entity"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        Result<CheckResult> Verify(ConfigedInfo appInfo, object entity, ContentType contentType);
    }
}
