using Infrastructure.Core;
using System;

namespace Innocellence.RoyaltyScan.Domain.Entity
{
    public partial class SoftwareModel : EntityBase<int>
	{
        public override Int32 Id { get; set; }

        public string UserName { get; set; }
        public string MachineName { get; set; }
        public string DisplayIcon { get; set; }
        public string DisplayName { get; set; }
        public string DisplayVersion { get; set; }
        public string InstallDate { get; set; }
        public string InstallLocation { get; set; }
        public string ProductChanel { get; set; }
        public string Publisher { get; set; }
        public string UnstallString { get; set; }
        public string URLInfoAbout { get; set; }

        public DateTime? CreatedDate { get; set; }

        //[Column("CreatedUserID",DbType=DBType.VarChar,Length=50,Precision=50,IsNullable=true)]
        public String CreatedUserID { get; set; }

        //[Column("UpdatedDate",DbType=DBType.DateTime,Length=8,Precision=23,IsNullable=true)]
        public DateTime? UpdatedDate { get; set; }

        //
        //[Column("UpdatedUserID",DbType=DBType.VarChar,Length=50,Precision=50,IsNullable=true)]
        public String UpdatedUserID { get; set; }

        public Boolean? IsDeleted { get; set; }

 
	}
}
