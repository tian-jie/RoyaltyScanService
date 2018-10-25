using System;

namespace Innocellence.WeChat.Domain.CommonEntity
{
    /// <summary>
    /// 给HCP用的放在claims里的信息，当session用,iDoctor
    /// </summary>
    public class WeChatLccpHcpClaim : IClaimEntity
    {
        /// <summary>
        /// iDoctor服务号里的OpenID
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// iDoctor里的ProjectHcpId
        /// </summary>
        public string HcpId { get; set; }

        /// <summary>
        /// iDoctor里的DctrId
        /// </summary>
        public string DctrId { get; set; }

        /// <summary>
        /// 用户登录的ID
        /// </summary>
        public string UserId
        {
            get
            {
                return HcpId;
            }
        }

        /// <summary>
        /// ConfigType
        /// </summary>
        public string ConfigType
        {
            get
            {
                return "WeChatLccpHcpClaim";
            }
        }
    }

    /// <summary>
    /// 给Nurse预留的放在claims里的信息，当session用
    /// </summary>
    public class WeChatLccpNurseClaim : IClaimEntity
    {
        /// <summary>
        /// iNurse服务号里的OpenID
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// iNurse服务号里的ProjectNurseId
        /// </summary>
        public string ProjectNurseId { get; set; }

        /// <summary>
        /// iNurse里的NurseId
        /// </summary>
        public string NurseId { get; set; }

        /// <summary>
        /// 用户登录的ID
        /// </summary>
        public string UserId
        {
            get
            {
                return NurseId;
            }
        }

        /// <summary>
        /// ConfigType
        /// </summary>
        public string ConfigType
        {
            get
            {
                return "WeChatLccpNurseClaim";
            }
        }
    }

    /// <summary>
    /// 给企业号销售预留的放在claims里的信息，当session用
    /// </summary>
    public class WeChatLccpRepClaim : IClaimEntity
    {
        /// <summary>
        /// 企业号里销售的礼来系统ID
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// 员工里的globalId
        /// </summary>
        public string GlobalId { get; set; }

        /// <summary>
        /// 服务号里的role
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 用户登录的ID
        /// </summary>
        public string UserId
        {
            get
            {
                return EmployeeId;
            }
        }

        /// <summary>
        /// ConfigType
        /// </summary>
        public string ConfigType
        {
            get
            {
                return "WeChatLccpRepClaim";
            }
        }
    }

    /// <summary>
    /// 这个预留得有点远:)，给药店用，当session用
    /// </summary>
    public class WeChatLccpDrugStoreClaim : IClaimEntity
    {

        /// <summary>
        /// 用户登录的ID
        /// </summary>
        public string UserId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// ConfigType
        /// </summary>
        public string ConfigType
        {
            get
            {
                throw new NotImplementedException();
                //return "WeChatLccpDrugStoreClaim";
            }
        }
    }


    /// <summary>
    /// 给微信小程序用，当session用
    /// </summary>
    public class WeChatCorpAssistMiniProgramClaim : IClaimEntity
    {
        /// <summary>
        /// 小程序里的OpenID
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 用户登录的ID
        /// </summary>
        public string UserId
        {
            get
            {
                return OpenId;
            }
        }

        /// <summary>
        /// ConfigType
        /// </summary>
        public string ConfigType
        {
            get
            {
                return "WeChatCorpAssistMiniProgramClaim";
            }
        }
    }


    /// <summary>
    /// Base Entity for XXXClaim
    /// </summary>
    public interface IClaimEntity
    {
        /// <summary>
        /// 必须继承这个，把用于登录的ID都放到这个里
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// 记录自己是什么ConfigType
        /// </summary>
        string ConfigType { get; }

    }
}
