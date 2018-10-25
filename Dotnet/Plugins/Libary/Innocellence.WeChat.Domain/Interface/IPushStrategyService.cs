using System.Collections.Generic;
using System.Linq;
using Infrastructure.Core;
using Innocellence.WeChat.Domain.Common;
using Innocellence.WeChat.Domain.CommonEntity;

namespace Innocellence.WeChat.Domain.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPushStrategyService : IDependency
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="appInfo"></param>
        /// <returns></returns>
        Result<CheckResult> CheckPushRule(object entity, ConfigedInfo appInfo);

        /// <summary>
        /// 
        /// </summary>
        ContentType ContentType { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BaseStrategyService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="appInfo"></param>
        /// <param name="entity"></param>
        protected virtual void HandlerResult(Result<CheckResult> result, ConfigedInfo appInfo, object entity)
        {
            if (result.Status == 200)
                return;

            var errorDepartments = new List<Desc>();
            var errorTags = new List<Desc>();

            var targetInfo = (TargetInfoObject)entity;

            var selectedDepartmentIds = targetInfo.Departments;

            //1. find out department from error departments ,that does not select
            //2.
            var errorInfo = result.Data.Error;

            #region
            if (errorInfo.ErrorDepartments != null && errorInfo.ErrorDepartments.Any())
            {
                var errorDepartmentIdsUnderSelected = selectedDepartmentIds.Where(x =>
                {
                    var subDepartments = WeChatCommonService.GetSubDepartments(x, appInfo.CurrentPlantformAccountId);
                    return subDepartments.Any(y => errorInfo.ErrorDepartments.Any(z => y.id == z.Id));
                });

                errorDepartments.AddRange(errorDepartmentIdsUnderSelected.ConvertDepartmentIdToObject(appInfo.CurrentPlantformAccountId));

                var errorTagsFromDepartment = targetInfo.Tags.Where(tag =>
                 {
                     var departmentsUnderSelectedTag = appInfo.Dictionary[tag].partylist;


                     if (errorInfo.ErrorDepartments.Any(x => WeChatCommonService.GetSubDepartments(departmentsUnderSelectedTag, appInfo.CurrentPlantformAccountId).Any(y => x.Id == y.id)))
                     {
                         return true;
                     }
                     return false;
                 }).ToList();

                errorTags.AddRange(errorTagsFromDepartment.ConvertTagIdToObject(appInfo.CurrentPlantformAccountId));
            }
            #endregion

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentIds"></param>
        /// <param name="plantformAccountId"></param>
        /// <returns></returns>
        /// <exception cref="InnocellenceException"></exception>
        public static IList<Desc> ConvertDepartmentIdToObject(this IEnumerable<int> departmentIds, int plantformAccountId)
        {
            return departmentIds.Select(x =>
            {
                var department = WeChatCommonService.lstDepartment(plantformAccountId).FirstOrDefault(y => x == y.id);
                if (department == null)
                {
                    throw new InnocellenceException(string.Format("the department id {0} have not been find!", x));
                }
                return new Desc { Id = department.id, Name = department.name };
            }).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagIds"></param>
        /// <param name="plantformAccountId"></param>
        /// <returns></returns>
        /// <exception cref="InnocellenceException"></exception>
        public static IList<Desc> ConvertTagIdToObject(this IEnumerable<int> tagIds, int plantformAccountId)
        {
            return tagIds.Select(x =>
            {
                var tag = WeChatCommonService.lstTag(plantformAccountId).FirstOrDefault(y => x == int.Parse(y.tagid));
                if (tag == null)
                {
                    throw new InnocellenceException(string.Format("the tag id {0} have not been find in cache!", x));
                }
                return new Desc { Id = int.Parse(tag.tagid), Name = tag.tagname };
            }).ToList();
        }
    }
}
