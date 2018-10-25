using Infrastructure.Core.Logging;
using Innocellence.Weixin.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Innocellence.WeChat.Domain.Common
{
    /// <summary>
    /// 口令类型
    /// </summary>
    public class SignClient
    {
        public static ILogger Logger = LogManager.GetLogger("WeChat");

        public static string Sign(Dictionary<string,string> dic )
        {
            if (dic==null && dic.Count==0)
            {
                Logger.Error("Sign error: dic is null");
                return null;
            }
                var strArray = dic.OrderBy(x => x.Key, new OrdinalComparer()).Select(x => string.Format("{0}={1}", x.Key, x.Value)).ToArray();
                
                var parameters = string.Join("&", strArray);

               var strRet = SHA1UtilHelper.GetSha1(parameters);
                Logger.Error("parameters:{0}  Sign:{1}", parameters, strRet);


                return strRet;

            
        }

        public static string SignFromParam(Dictionary<string, string> dic,string strAppSignKey)
        {
            dic.Add("nonce", JSSDKHelper.GetNoncestr());
            dic.Add("timestamp", JSSDKHelper.GetTimestamp().ToString());
            dic.Add("appSignKey", strAppSignKey);

            return Sign(dic);

        }

        public static string GetSignUrl(Dictionary<string, string> dic, string strAppSignKey)
        {
            var sign = SignFromParam(dic, strAppSignKey);
            dic.Remove("appSignKey");
            dic.Add("sign", sign);
           var strArray = dic.Select(x => string.Format("{0}={1}", x.Key, x.Value)).ToArray();

            return string.Join("&", strArray);
        }

        public static string GetSignUrl(Dictionary<string, string> signDic, Dictionary<string, string> paramDic, string strAppSignKey)
        {
            var sign = SignFromParam(signDic, strAppSignKey);

            signDic.Remove("appSignKey");
            signDic.Add("sign", sign);
            foreach (var d in paramDic)
            {
                signDic.Add(d.Key,d.Value);
            }
            var strArray = signDic.Select(x => string.Format("{0}={1}", x.Key, x.Value)).ToArray();

            return string.Join("&", strArray);
        }

    }


}
