using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoyaltyScanService
{
    /// <summary>
    /// 提交到服务器的软件列表 和 服务器返回的非法软件列表
    /// </summary>
    public class SubmitSoftwaresView
    {
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
    }
}
