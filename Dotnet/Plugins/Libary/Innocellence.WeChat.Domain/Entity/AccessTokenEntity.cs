using System;
using System.Collections.Generic;
using Infrastructure.Core;
using System.ComponentModel.DataAnnotations.Schema;
namespace Innocellence.WeChat.Domain.Entity
{
    /// <summary>
    /// AccessToken
    /// </summary>
    [Table("AccessToken")]
    public class AccessTokenEntity : EntityBase<int>
	{
		//[Id("Id",IsDbGenerated=true)]
        /// <summary>
        /// Id
        /// </summary>
        public override Int32 Id { get; set; }

        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// AccessToken
        /// </summary>
        public String Token { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        public String OpenId { get; set; }

        /// <summary>
        /// UnionId
        /// </summary>
        public String UnionId { get; set; }

        /// <summary>
        /// UserType
        /// </summary>
        public String UserType { get; set; }

        /// <summary>
        /// CreatedDate
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// CreatedDate
        /// </summary>
        public DateTime? ExpiredDate { get; set; }

        /// <summary>
        /// CreatedUserId
        /// </summary>
        public string CreatedUserId { get; set; }

        /// <summary>
        /// UpdatedDate
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// UpdatedUserId
        /// </summary>
        public string UpdatedUserId { get; set; }

        /// <summary>
        /// IsValid
        /// </summary>
        public int IsValid { get; set; }

        /// <summary>
        /// IsDeleted
        /// </summary>
        public Boolean? IsDeleted { get; set; }

    }
}
