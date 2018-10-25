using System;
using System.Collections.Generic;
using Infrastructure.Core;
using System.ComponentModel.DataAnnotations.Schema;
using Innocellence.WeChat.Domain.Entity;
namespace Innocellence.WeChat.Domain.Contracts.ViewModel
{

    /// <summary>
    /// AccessTokenView
    /// </summary>
    public class AccessTokenView : IViewModel
    {

        /// <summary>
        /// Id
        /// </summary>
        public Int32 Id { get; set; }

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

        /// <summary>
        /// ConvertAPIModel
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IViewModel ConvertAPIModel(object obj)
        {
            if (obj == null) { return this; }
            var entity = (AccessTokenEntity)obj;
            Id = entity.Id;
            AppId = entity.AppId;
            Token = entity.Token;
            UserId = entity.UserId;
            UnionId = entity.UnionId;
            OpenId = entity.OpenId;
            UserType = entity.UserType;
            CreatedDate = entity.CreatedDate;
            ExpiredDate = entity.ExpiredDate;
            CreatedUserId = entity.CreatedUserId;
            UpdatedDate = entity.UpdatedDate;
            UpdatedUserId = entity.UpdatedUserId;
            IsDeleted = entity.IsDeleted;
            IsValid = entity.IsValid;

            return this;
        }
    }
}
