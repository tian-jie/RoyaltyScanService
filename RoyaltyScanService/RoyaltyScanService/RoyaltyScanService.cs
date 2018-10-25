using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RoyaltyScanService
{
    public class RoyaltyScanService
    {
        /// <summary>
        ///  扫描本地所有软件，并且上传到服务器，得到由服务器返回的非法软件列表
        /// </summary>
        /// <returns></returns>
        public async Task<IList<SoftwareModel>> ScanAndUpload()
        {
            //获取临时目录路径
            string temp = System.Environment.GetEnvironmentVariable("TEMP");
            DirectoryInfo info = new DirectoryInfo(temp);
            //info.FullName 即是临时目录的路径字符串。
            var tempFolder = info.FullName;

            // 获取本机软件列表信息
            var softwares = await Task.Run(() =>
            {
                return GetSoftwares();
            });

            // 发送信息并且得到结果
            var machineName = System.Environment.MachineName;
            var userName = System.Environment.UserName;
            
            var submitSoftwaresView = new SubmitSoftwaresView()
            {
                UserName = userName,
                MachineName = machineName,
                Softwares = softwares
            };

            // 获取url，提交到服务器
            var url = ConfigurationManager.AppSettings["submitUrl"];
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("url can't be empty.");
            }

            var httpClient = new HttpClient();
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(submitSoftwaresView));
            var response = await httpClient.PostAsync(url, content);

            var illegalSoftwares = Newtonsoft.Json.JsonConvert.DeserializeObject<SubmitSoftwaresView>(await response.Content.ReadAsStringAsync());
            return illegalSoftwares.Softwares;
        }

        /// <summary>          
        /// 本机所有安装软件         
        /// </summary>          
        /// <returns>软件列表：软件名称，安装版本等</returns>         
        public static IList<SoftwareModel> GetSoftwares()
        {
            List<RegistryKey> RegistryKeys = new List<RegistryKey>();
            //对应注册表
            RegistryKeys.Add(Registry.ClassesRoot);
            RegistryKeys.Add(Registry.CurrentConfig);
            RegistryKeys.Add(Registry.CurrentUser);
            RegistryKeys.Add(Registry.LocalMachine);
            RegistryKeys.Add(Registry.PerformanceData);
            RegistryKeys.Add(Registry.Users);

            var softwares = new List<SoftwareModel>();
            string SubKeyName = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";
            foreach (RegistryKey Registrykey in RegistryKeys)
            {
                using (RegistryKey RegistryKey1 = Registrykey.OpenSubKey(SubKeyName, false))
                {
                    if (RegistryKey1 == null)
                        continue;
                    if (RegistryKey1.GetSubKeyNames() == null)
                        continue;
                    string[] KeyNames = RegistryKey1.GetSubKeyNames();
                    foreach (string KeyName in KeyNames)
                    {
                        using (RegistryKey RegistryKey2 = RegistryKey1.OpenSubKey(KeyName, false))
                        {
                            if (RegistryKey2 == null)
                                continue;
                            //获取软件名
                            string SoftwareName = RegistryKey2.GetValue("DisplayName", "").ToString();
                            //获取软件版本
                            string InstallLocation = RegistryKey2.GetValue("DisplayVersion", "").ToString();
                            if (!string.IsNullOrEmpty(InstallLocation) && !string.IsNullOrEmpty(SoftwareName))
                            {
                                softwares.Add(new SoftwareModel()
                                {
                                    DisplayIcon = RegistryKey2.GetValue("DisplayIcon", "").ToString(),
                                    DisplayName = RegistryKey2.GetValue("DisplayName", "").ToString(),
                                    DisplayVersion = RegistryKey2.GetValue("DisplayVersion", "").ToString(),
                                    InstallDate = RegistryKey2.GetValue("InstallDate", "").ToString(),
                                    InstallLocation = RegistryKey2.GetValue("InstallLocation", "").ToString(),
                                    ProductChanel = RegistryKey2.GetValue("ProductChanel", "").ToString(),
                                    Publisher = RegistryKey2.GetValue("Publisher", "").ToString(),
                                    UnstallString = RegistryKey2.GetValue("UnstallString", "").ToString(),
                                    URLInfoAbout = RegistryKey2.GetValue("URLInfoAbout", "").ToString(),
                                });

                            }
                        }
                    }
                }
            }
            return softwares;
        }
    }
}
