using Infrastructure.Core;
using Infrastructure.Core.Data;
using Infrastructure.Core.Logging;
using Innocellence.WeChat.Domain.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;




namespace Innocellence.WeChat.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class WeChatDomainCommonService
    {
        ILogger _logger = LogManager.GetLogger("Lccp");

        #region Cache
        //private static object objLock = new object();
        //private static object objLockSys = new object();
        //private static object objLockWeChat = new object();

        //public static List<Category> lstCategory
        //{
        //    get
        //    {
        //        var lst = CacheManager.GetCacher("Default").Get<List<Category>>("Category");
        //        if (lst == null)
        //        {
        //            lock (objLock)
        //            {
        //                if (lst == null)
        //                {
        //                    BaseService<Category> ser = new BaseService<Category>();
        //                    lst = ser.Entities.ToList();

        //                    //暂时不缓存
        //                  //  CacheManager.GetCacher("Default").Set("Category", lst,new TimeSpan(1,0,0));
        //                }
        //            }
        //        }

        //        return lst;
        //    }
        //}

        #endregion

        /// <summary>
        /// GetRemoteInterface，获取接口设置
        /// </summary>
        /// <param name="servicename"></param>
        /// <returns></returns>
        public static RemoteInterface GetRemoteInterface(string servicename)
        {
            string strConfig = Infrastructure.Web.Domain.Service.CommonService.GetSysConfig("WechatMP", "");
            return JsonConvert.DeserializeObject<List<RemoteInterface>>(strConfig).FirstOrDefault(a => a.ApiName == servicename);
        }




        #region 获取XML中节点信息
        public XmlNodeList GetXmlNodes(string tagName)
        {
            XmlNodeList nodes = null;
            try
            {
                // 系统目录
                string atr = System.AppDomain.CurrentDomain.BaseDirectory;
                // WebConfig中配置的XML文件路径
                string strPath = ConfigurationManager.AppSettings["XmlPath"];

                XmlDocument doc = new XmlDocument();

                doc.Load(atr + strPath);

                nodes = doc.GetElementsByTagName(tagName);
            }
            catch (Exception ex)
            {
                // Logger.Error(ex, "获取XML中节点信息错误-" + tagName, ex.ToString());
            }
            return nodes;
        }
        #endregion

        #region 发送邮件
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="strto">收件人</param>
        /// <param name="subj">邮件主题</param>
        /// <param name="bodys">邮件内容</param>
        /// <returns>是否发送成功</returns>
        public bool SendMail(string strto, string subj, string bodys)
        {
            XmlNode mailserver = GetXmlNodes("MailServer")[0];

            // SMTP服务器
            string smtpserver = mailserver.Attributes["smtpserver"].Value;
            // 发件人
            string strfrom = mailserver.Attributes["strfrom"].Value;
            // 发件人用户名
            string userName = mailserver.Attributes["userName"].Value;
            // 发件人密码
            string pwd = mailserver.Attributes["pwd"].Value;
            // 端口
            int sendPort = int.Parse(mailserver.Attributes["sendPort"].Value);
            // 是否SSL加密
            bool ssl = bool.Parse(mailserver.Attributes["ssl"].Value);

            try
            {
                System.Net.Mail.SmtpClient _smtpClient = new System.Net.Mail.SmtpClient();
                _smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                _smtpClient.Host = smtpserver;  //SMTP服务器
                _smtpClient.Port = sendPort;    //端口
                _smtpClient.EnableSsl = ssl;    //是否SSL加密
                _smtpClient.Credentials = new System.Net.NetworkCredential(userName, pwd);
                System.Net.Mail.MailMessage _mailMessage = new System.Net.Mail.MailMessage(strfrom, strto);
                _mailMessage.Subject = subj;
                _mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;//正文编码
                _mailMessage.Body = bodys;
                _mailMessage.BodyEncoding = System.Text.Encoding.UTF8;//正文编码
                _mailMessage.IsBodyHtml = true;//设置为HTML格式
                _mailMessage.Priority = System.Net.Mail.MailPriority.High;//优先级
                _smtpClient.Send(_mailMessage);
                return true;
            }
            catch
            {
                throw;
            }
        }


        #endregion

    }


}
