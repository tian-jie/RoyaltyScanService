using Infrastructure.Core;

namespace Innocellence.WeChat.Domain.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class DownstreamSys : EntityBase<int>
    {
        /// <summary>
        /// 
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DownstreamSysName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DownstreamSysDes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DownstreamSysKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Status { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SubSystemMapping : EntityBase<int>
    {
        /// <summary>
        /// 
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DownstreamSysKey { get; set; }

        /// <summary>
        /// 公众平台账号 id
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CallBackUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 二级标识
        /// </summary>
        public string UniqueId { get; set; }
    }
}
