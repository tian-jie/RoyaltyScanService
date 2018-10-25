using System;
using System.Collections.Generic;
using Infrastructure.Core;
using System.ComponentModel.DataAnnotations.Schema;
namespace Innocellence.WeChat.Domain.Entity
{
    [Table("AEPC")]
    public class AEPCEntity : EntityBase<int>
	{
		//[Id("Id",IsDbGenerated=true)]
        public override Int32 Id { get; set; }
        public string AppId { get; set; }

        public String OpenId { get; set; }

        public String TextContent { get; set; }
      
		//[Column("CreatedDate",DbType=DBType.DateTime,Length=8,Precision=23,IsNullable=true)]
        public DateTime? CreatedDate { get; set; }

        public Boolean? IsDeleted { get; set; }

    }
}
