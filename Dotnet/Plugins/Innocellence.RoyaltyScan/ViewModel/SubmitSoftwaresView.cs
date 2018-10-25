using Infrastructure.Core;
using Innocellence.RoyaltyScan.Domain.Entity;
using System.Collections.Generic;

/// <summary>
/// 提交到服务器的软件列表 和 服务器返回的非法软件列表
/// </summary>
public partial class SubmitSoftwaresView : IViewModel
{
    public int Id { get; set; }
    /// <summary>
    /// 当前登录的用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 当前登录的机器名
    /// </summary>
    public string MachineName { get; set; }

    /// <summary>
    /// 列举的软件列表
    /// </summary>
    public IList<SoftwareModel> Softwares { get; set; }

    public IViewModel ConvertAPIModel(object model)
    {
        throw new System.NotImplementedException();
    }
}
