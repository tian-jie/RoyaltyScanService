using Infrastructure.Core.Data;
using Innocellence.RoyaltyScan.Domain.Entity;

namespace Innocellence.RoyaltyScan.Configuration
{
    public class SoftwareModelConfiguration : EntityConfigurationBase<SoftwareModel, int>
    {
        public SoftwareModelConfiguration()
        {
            ToTable("Softwares");
        }
    }
   
}

