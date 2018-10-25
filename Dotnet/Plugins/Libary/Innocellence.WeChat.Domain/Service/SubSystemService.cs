using Infrastructure.Core.Data;
using Innocellence.WeChat.Domain.Contracts;
using Innocellence.WeChat.Domain.Entity;

namespace Innocellence.WeChat.Domain.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class SubSystemService :BaseService<DownstreamSys>, ISubSystemService
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class SubSystemMappingService :BaseService<SubSystemMapping>, ISubSystemMappingService
    {
    }
}
