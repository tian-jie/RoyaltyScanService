using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Core;
using Innocellence.WeChat.Domain.Common;
using Innocellence.WeChat.Domain.CommonEntity;
using Innocellence.Weixin.QY.AdvancedAPIs.App;
using Innocellence.Weixin.QY.AdvancedAPIs.MailList;
using Innocellence.Weixin.QY.AdvancedAPIs.Mass;

namespace Innocellence.WeChat.Domain.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPushService : IDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="appId"></param>
        /// <param name="plantformAccountId"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        Result<object> PushCheck(TargetInfoObject entity, int appId,int plantformAccountId, ContentType contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageResult"></param>
        /// <param name="historyId"></param>
        /// <returns></returns>
        Task Update(MassResult messageResult, int historyId);
    }

    /// <summary>
    /// 
    /// </summary>
    public class ErrorResult
    {
        /// <summary>
        /// 
        /// </summary>
        public IList<string> ErrorUsers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<Desc> ErrorDepartments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<Desc> ErrorTags { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SuccessResult
    {
        /// <summary>
        /// 
        /// </summary>
        public IList<string> ToUsers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<Desc> ToDepartments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<Desc> ToTags { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CheckResult
    {
        /// <summary>
        /// 
        /// </summary>
        public ErrorResult Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SuccessResult Success { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("news")]
        News,

        /// <summary>
        /// 
        /// </summary>
        [Description("Message")]
        Message,

        /// <summary>
        /// 
        /// </summary>
        [Description("text")]
        Text,

        /// <summary>
        /// 
        /// </summary>
        [Description("voice")]
        Video
    }

    /// <summary>
    /// 
    /// </summary>
    public class TargetInfoObject
    {
        /// <summary>
        /// 
        /// </summary>
        public IList<int> Departments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<string> Users { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<int> Tags { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsToAllUsers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ContentId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 
        /// </summary>
        Cancel,
        /// <summary>
        /// 
        /// </summary>
        Continue
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// 
        /// </summary>
        Checked,

        /// <summary>
        /// 
        /// </summary>
        Returned
    }

    /// <summary>
    /// 
    /// </summary>
    public enum TargetType
    {
        /// <summary>
        /// 
        /// </summary>
        AllUser,
        /// <summary>
        /// 
        /// </summary>
        Config
    }

    /// <summary>
    /// 
    /// </summary>
    public class ConfigedInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public List<int> AssignedDepartmentIds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> AssignedUserIds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GetAppInfoResult AppInfoResult { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public List<int> DepartmentsUnderSelectedTag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<int, GetTagMemberResult> Dictionary { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentPlantformAccountId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appInfo"></param>
        /// <param name="plantformAccountId"></param>
        public ConfigedInfo(GetAppInfoResult appInfo, int plantformAccountId)
        {
            Dictionary = new Dictionary<int, GetTagMemberResult>();
            AppInfoResult = appInfo;

            AssignedDepartmentIds = (appInfo.allow_partys == null || appInfo.allow_partys.partyid == null || !appInfo.allow_partys.partyid.Any()) ? new List<int>() : appInfo.allow_partys.partyid.ToList();

            AssignedUserIds = (appInfo.allow_userinfos == null || appInfo.allow_userinfos.user == null || !appInfo.allow_userinfos.user.Any()) ? new List<string>() : appInfo.allow_userinfos.user.Select(x => x.userid).ToList();

            //append depart and user of tag
            var tagIds = (appInfo.allow_tags == null || appInfo.allow_tags.tagid == null || !appInfo.allow_tags.tagid.Any()) ? new List<int>() : appInfo.allow_tags.tagid.ToList();

            foreach (var tagId in tagIds)
            {
                var memberResult = WeChatCommonService.GetTagMembers(tagId, int.Parse(appInfo.agentid));

                AssignedDepartmentIds.AddRange(memberResult.partylist);

                AssignedUserIds.AddRange(memberResult.userlist.Select(x => x.userid).ToList());

                Dictionary.Add(tagId, memberResult);
            }

            var subDepartments = WeChatCommonService.GetSubDepartments(AssignedDepartmentIds.Distinct().ToList(), plantformAccountId).ToList();

            AssignedUserIds.AddRange(WeChatCommonService.lstUser(plantformAccountId).Where(x => x.department.Any(y => subDepartments.Any(d => d.id == y))).Select(x => x.userid).ToList());

            AssignedUserIds = AssignedUserIds.Distinct().ToList();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum StatusCode
    {
        /// <summary>
        /// 
        /// </summary>
        CheckError = 1,
        /// <summary>
        /// 
        /// </summary>
        Others = 99,
        /// <summary>
        /// 
        /// </summary>
        Success = 200,
    }

    /// <summary>
    /// 
    /// </summary>
    //public class CheckResult
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public IList<Desc> ErrorDepartments { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public IList<Desc> ErrorTags { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public IList<string> ErrorUsers { get; set; }
    //}

    /// <summary>
    /// 
    /// </summary>
    public class Desc
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 
        /// </summary>
        Messsage,

        /// <summary>
        /// 
        /// </summary>
        EventMessage
    }
}
