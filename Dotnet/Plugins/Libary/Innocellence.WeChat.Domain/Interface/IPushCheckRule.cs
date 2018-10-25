using System;
using Infrastructure.Core;
using Innocellence.WeChat.Domain.CommonEntity;

namespace Innocellence.WeChat.Domain.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPushCheckRule : IDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appInfo"></param>
        /// <param name="targetInfo"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        Result<CheckResult> Verify(ConfigedInfo appInfo, TargetInfoObject targetInfo, Result<CheckResult> result);
    }

    /// <summary>
    /// 
    /// </summary>
    public class BasePushCheckRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected virtual void Init(Result<CheckResult> result)
        {
            if (result==null)
            {
                throw new ArgumentNullException("result");
            }

            if (result.Data == null)
            {
                result.Data = new CheckResult();
            }

            if (result.Data.Success == null)
            {
                result.Data.Success = new SuccessResult();
            }

            if (result.Data.Error == null)
            {
                result.Data.Error = new ErrorResult();
            }
        }
    }
}
