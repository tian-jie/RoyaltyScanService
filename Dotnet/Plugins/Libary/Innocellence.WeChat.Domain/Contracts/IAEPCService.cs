using Infrastructure.Core;
using Innocellence.WeChat.Domain.Contracts.ViewModel;
using Innocellence.WeChat.Domain.Entity;
using System;
using System.Collections.Generic;
using System.IO;

namespace Innocellence.WeChat.Domain.Contracts
{
    public interface IAEPCService : IDependency, IBaseService<AEPCEntity>
    {
        List<AEPCView> GetListByDate(DateTime datefrom, DateTime dateto, string AppId);
        bool SendMail(string jsonString, string name = "");
    }
}
