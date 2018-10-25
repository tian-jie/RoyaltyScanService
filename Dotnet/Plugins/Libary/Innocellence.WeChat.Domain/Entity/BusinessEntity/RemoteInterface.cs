using System;
using Infrastructure.Core;

namespace Innocellence.WeChat.Domain.Entity
{
    /// <summary>
    /// 接口的配置，数据类
    /// </summary>
    public class RemoteInterface : EntityBase<int>
    {
        /// <summary>
        /// ID
        /// </summary>
        public override Int32 Id { get; set; }

        /// <summary>
        /// ApiName, 用于取API的key
        /// </summary>
        public string ApiName { get; set; }

        /// <summary>
        /// 第三方给的APPID
        /// </summary>
        public string AppID { get; set; }

        /// <summary>
        /// 第三方给的AppSignKey
        /// </summary>
        public string AppSignkey { get; set; }

        /// <summary>
        /// 这个API的URL
        /// </summary>
        public string URL { get; set; }
        
    }
}
