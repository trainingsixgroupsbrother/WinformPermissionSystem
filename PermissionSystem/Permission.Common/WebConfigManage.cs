using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Common
{
    public class WebConfigManage
    {
        /// <summary>
        /// 数据库写入链接配置
        /// </summary>
        public static string PermissionWrite = Tools.GetConfigValue("PermissionWrite");

        /// <summary>
        /// 数据库读取链接配置
        /// </summary>
        public static string PermissionRead = Tools.GetConfigValue("PermissionRead");
    }
}
