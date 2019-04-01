using Permission.Common.ExtendAttributes.ModelExtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Common.ExtendAttributes
{
    public static class ExtendAttribute
    {
        #region kongge 2019-04-01 获取表名称
        /// <summary>
        /// 获取表名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTableName(this Type type)
        {
            if (type.IsDefined(typeof(TableAttribute), true))
            {
                TableAttribute attribute = (TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), true)[0];
                return attribute.TableName;
            }
            else
            {
                return type.Name;
            }
        }
        #endregion

        #region kongge 2019-04-01 获取列名
        /// <summary>
        /// 获取列名
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetColumnName(this PropertyInfo property)
        {
            if (property.IsDefined(typeof(ColumnAttribute), true))
            {
                ColumnAttribute attribute = (ColumnAttribute)property.GetCustomAttributes(typeof(ColumnAttribute), true)[0];
                return attribute.ColumnName;
            }
            else
            {
                return property.Name;
            }
        }
        #endregion

        #region kongge 2019-04-01 判断数据字段自增长
        /// <summary>
        /// 判断数据字段自增长
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool ValidateIdentity(this PropertyInfo property)
        {
            if (property.IsDefined(typeof(IdentityAttribute), true))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region kongge 2019-04-01 判断数据字段主键
        /// <summary>
        /// 判断数据字段主键
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool ValidatePrimary(this PropertyInfo property)
        {
            if (property.IsDefined(typeof(PrimaryKeyAttribute), true))
            {
                return true;
            }
            return false;
        }
        #endregion




    }
}
