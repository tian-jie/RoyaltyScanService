using Infrastructure.Core.Data;
using Innocellence.RoyaltyScan.Domain.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using EntityFramework.BulkInsert.Extensions;
using Infrastructure.Core;

namespace Innocellence.RoyaltyScan.Services
{
    public class SoftwareService : BaseService<SoftwareModel>, ISoftwareService
    {
        public SoftwareService(IUnitOfWork unitOfWork)
            : base("CAAdmin")
        {

        }

        public int Save(string username, string machineName, IList<SoftwareModel> softwares)
        {
            foreach(var software in softwares)
            {
                software.UserName = username;
                software.MachineName = machineName;
            }

            var ctx = (DbContext)(Repository.UnitOfWork);

            ctx.BulkInsert(softwares);
            ctx.SaveChanges();

            return softwares.Count;
        }

    }

}
