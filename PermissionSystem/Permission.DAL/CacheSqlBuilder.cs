using Permission.Common.ExtendAttributes;
using Permission.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Permission.DAL
{
    public static class CacheSqlBuilder<T> where T : BaseModel
    {
        public static string AddSql = "";
        public static string UpdSql = "";
        public static string DelSql = "";
        public static string SelSql = "";
        public static string CountSql = "";
        static CacheSqlBuilder()
        {
            Type type = typeof(T);
            //查询所有字段
            IEnumerable<PropertyInfo> iEnumerable = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            string columnStr = string.Join(",", iEnumerable.Where(p => !p.ValidateIdentity()).Select(p => "[" + p.GetColumnName() + "]"));
            string valueStr = string.Join(",", iEnumerable.Where(p => !p.ValidateIdentity()).Select(p => "@" + p.Name));
            //新增SQL
            AddSql = string.Format("insert into {0} ({1}) values ({2})", type.GetTableName(), columnStr, valueStr);

            string updColumnStr = string.Join(",", iEnumerable.Where(p => !p.ValidateIdentity() && !p.ValidatePrimary()).Select(p => "[" + p.GetColumnName() + "]=@" + p.Name));
            //修改SQL
            UpdSql = string.Format("update {0} set {1}", type.GetTableName(), updColumnStr);

            //删除SQL
            DelSql = string.Format("delete {0} ", type.GetTableName());

            string selectStr = string.Join(",", iEnumerable.Select(p => "[" + p.GetColumnName() + "] as [" + p.Name + "]"));
            //查询SQL
            SelSql = string.Format("select {0} from {1} ", selectStr, type.GetTableName());
            //获取总数据SQL
            CountSql = string.Format("select count(*) from {0} ", type.GetTableName());
        }
    }
}
