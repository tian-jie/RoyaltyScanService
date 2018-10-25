using Infrastructure.Core;
using Infrastructure.Core.Data;
using Innocellence.WeChat.Domain.Contracts;
using Innocellence.WeChat.Domain.Contracts.ViewModel;
using Innocellence.WeChat.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Infrastructure.Web.UI;
using Infrastructure.Utility.Data;
using Innocellence.Weixin.QY.AdvancedAPIs;
using Innocellence.WeChat.Domain.Common;
using Infrastructure.Web.Net.Mail;
using System.Threading.Tasks;
using System.IO;
namespace Innocellence.WeChat.Domain.Service
{
    public class AEPCService : BaseService<AEPCEntity>, IAEPCService
    {
        public AEPCService()
            : base("CAAdmin")
        {

        }
        public List<AEPCView> GetListByDate(DateTime datefrom, DateTime dateto, string AppId)
        {
            Expression<Func<AEPCEntity, bool>> con = _ => true;
            if (datefrom != null)
            {
                con = x => x.CreatedDate >= datefrom;
            }
            if (dateto != null)
            {
                con = con.AndAlso(x => x.CreatedDate <= dateto);
            }
            if (!string.IsNullOrEmpty(AppId))
            {
                con = con.AndAlso(x => x.AppId == AppId);
            }
            con = con.AndAlso(x => x.IsDeleted == false);
            var lst = Repository.Entities.Where(con).OrderByDescending(p => new { p.CreatedDate })
                .ToList().Select(n => (AEPCView)(new AEPCView().ConvertAPIModel(n))).ToList();
            return lst;
        }
        public bool SendMail(string jsonString, string name = "")
        {
            
            var EmailReceiver = "";
            var EmailEnableSsl = false;
            var EmailHost = "";
            var EmailPassword = "";
            var EmailUserName = "";
            var EmailPort = "";
            var EmailSender = "";
            var EmailEnable = true;
            var EmailTitle = "";
            var EmailTemplate = "";

            object jsonResult = null;
            jsonResult = JsonHelper.FromJson<Dictionary<string, object>>(jsonString);
            var fullResult = jsonResult as Dictionary<string, object>;
            if (fullResult != null)
            {
                EmailReceiver = fullResult["EmailReceiver"] as string;
                EmailEnableSsl = bool.Parse(fullResult["EmailEnableSsl"].ToString());
                EmailHost = fullResult["EmailHost"] as string;
                EmailPassword = fullResult["EmailPassword"] as string;
                EmailUserName = fullResult["EmailUserName"] as string;
                EmailPort = fullResult["EmailPort"] as string;
                EmailSender = fullResult["EmailSender"] as string;
                EmailEnable = bool.Parse(fullResult["EmailEnable"].ToString());
                EmailTitle = fullResult["EmailTitle"] as string;
                EmailTemplate = fullResult["EmailTemplate"] as string;
            }

            if (!string.IsNullOrEmpty(EmailTitle))
            {
                EmailTitle = EmailTitle.Replace("\r", "").Replace("\n", "");
            }

            var emailContent = EmailTemplate;

            var set = new EmailMessageSettingsRecord()
            {
                Host = EmailHost,
                UserName = EmailUserName,
                Password = EmailPassword,
                Port = int.Parse(EmailPort),
                EnableSsl = EmailEnableSsl,
                Enable = EmailEnable,
                DeliveryMethod = "Network",
                Address = EmailSender,
                RequireCredentials = true
            };

            Task.Run(() =>
            {
                var ser = new Innocellence.WeChat.Domain.EmailMessageService(set);
                ser.SendMessage(EmailSender, EmailReceiver, EmailTitle,
                    emailContent.Replace("\r\n", "<br/>").Replace("\n", "<br/>").Replace("\r", "<br/>")
                    , null, name);
            });

            return true;
        }
    }
}
