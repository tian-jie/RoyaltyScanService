using Infrastructure.Core;
using Innocellence.RoyaltyScan.Domain.Entity;
using System.Collections.Generic;

namespace Innocellence.RoyaltyScan.Services
{
    public interface ISoftwareService : IDependency, IBaseService<SoftwareModel>
    {
        int Save(string username, string machineName, IList<SoftwareModel> softwares);
    }
}