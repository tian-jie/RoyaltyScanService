﻿using Infrastructure.Core;
using Infrastructure.Core.Data;
using Infrastructure.Utility.Extensions;
using Innocellence.WeChat.Domain.Contracts;
using Innocellence.WeChat.Domain.Entity;
using Innocellence.WeChat.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Innocellence.WeChat.Domain.Services
{
    /// <summary>
    /// 业务实现——App访问量统计
    /// </summary>
    public partial class AppAccessReportService : BaseService<AppAccessReport>, IAppAccessReportService
    {
       
        public AppAccessReportService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            
        }

        public AppAccessReportService()
            
        {

        }

        public List<T> GetListByFromDate<T>(Expression<Func<AppAccessReport, bool>> predicate) where T : IViewModel, new()
        {
            //var lst = Repository.Entities.Where(predicate).DistinctBy(p => new { p.AccessDate }).ToList().Select(n => (T)(new T().ConvertAPIModel(n))).ToList();
            var lst = Repository.Entities.Where(predicate).OrderByDescending(p => new { p.AccessDate }).ToList().Select(n => (T)(new T().ConvertAPIModel(n))).ToList();
            return lst;
        }
          
    }
}