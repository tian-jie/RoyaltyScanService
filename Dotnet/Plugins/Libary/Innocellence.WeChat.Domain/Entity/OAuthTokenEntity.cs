using System;
using Infrastructure.Core;

namespace Innocellence.WeChat.Domain.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class OAuthClientsEntity : EntityBase<int>
    {
        /// <summary>
        /// 
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientCallBackUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class OAuthTokenEntity : EntityBase<int>
    {
        /// <summary>
        /// 
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ExpiredDateUtc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDateUtc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsUsed { get; set; }
    }
}
