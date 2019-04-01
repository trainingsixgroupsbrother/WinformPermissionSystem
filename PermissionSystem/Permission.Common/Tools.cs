using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Permission.Common
{
    public class Tools
    {
        #region kongge 2019-04-01 获取web.config中appSettings配置节点的值
        /// <summary>
        /// 获取web.config中appSettings配置节点的值
        /// </summary>
        /// <param name="configKey">appSettings配置节点的Key</param>
        /// <returns></returns>
        public static string GetConfigValue(string configKey)
        {
            string value = ConfigurationManager.AppSettings[configKey];
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            return value;
        }
        #endregion

        #region kongge 2019-04-01 分页
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageIndex">页码从1开始</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public static Tuple<int, int> CalPageIndex(int pageIndex, int pageSize)
        {
            int start = (pageIndex - 1) * pageSize + 1;
            int end = pageIndex * pageSize;
            return Tuple.Create(start, end);
        }
        #endregion
    }
}
