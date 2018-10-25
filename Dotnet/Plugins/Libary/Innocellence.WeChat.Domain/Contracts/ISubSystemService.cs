using Infrastructure.Core;
using Innocellence.WeChat.Domain.Entity;

namespace Innocellence.WeChat.Domain.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISubSystemService : IBaseService<DownstreamSys>, IDependency
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISubSystemMappingService: IBaseService<SubSystemMapping>, IDependency
    {
        
    }
}
