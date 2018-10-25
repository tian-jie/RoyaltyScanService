using System;
using System.Collections.Generic;
using Infrastructure.Core;
using System.ComponentModel.DataAnnotations.Schema;
using Innocellence.WeChat.Domain.Entity;
namespace Innocellence.WeChat.Domain.Contracts.ViewModel
{
   
    public class AEPCView : IViewModel
	{
		
        public Int32 Id { get; set; }
        public string AppId { get; set; }

        public String OpenId { get; set; }

        public String TextContent { get; set; }
      
		//[Column("CreatedDate",DbType=DBType.DateTime,Length=8,Precision=23,IsNullable=true)]
        public DateTime? CreatedDate { get; set; }

        public Boolean? IsDeleted { get; set; }

        public IViewModel ConvertAPIModel(object obj)
        {
            if (obj == null) { return this; }
            var entity = (AEPCEntity)obj;
            Id = entity.Id;
            AppId = entity.AppId;
            CreatedDate = entity.CreatedDate;
            TextContent = entity.TextContent;
            OpenId = entity.OpenId;
            TextContent = entity.TextContent;
            return this;
        }
    }
}
