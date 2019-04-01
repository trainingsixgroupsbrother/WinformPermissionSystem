using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Permission.Model;
using Permission.Common;
using Permission.Common.ExtendAttributes;
using Permission.Common.Expressions;
using Permission.IDAL;

namespace Permission.DAL
{
    public class BaseDAL: IBaseDAL
    {
        private DBHelpers helpers = null;
        public BaseDAL(Enums.DBEnum _db)
        {
            helpers = new DBHelpers(_db);
        }

        #region kongge 2019-04-01 获取查询条件
        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        private Tuple<string, List<SqlParameter>> GetWhere<T>(Expression<Func<T, bool>> exp) where T : BaseModel
        {
            if (exp != null)
            {
                string where = ExpressionAnalysis.GetSql(exp);
                if (!String.IsNullOrWhiteSpace(where))
                {
                    where = string.Format(" where 1=1 and {0}", where);
                }
                return new Tuple<string, List<SqlParameter>>(where, ExpressionAnalysis.GetListParameter());
            }
            return new Tuple<string, List<SqlParameter>>("", null);
        }
        #endregion

        #region kongge 2019-04-01 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int Add<T>(T t) where T : BaseModel
        {
            string sql = CacheSqlBuilder<T>.AddSql;
            return helpers.ExexuteSql(sql, GetSqlParameters(t, p => !p.ValidateIdentity()));
        }
        /// <summary>
        /// 返回新增ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public object AddScalar<T>(T t) where T : BaseModel
        {
            string sql = CacheSqlBuilder<T>.AddSql;
            sql = sql.TrimEnd(';') + ";select @@IDENTITY";
            return helpers.ExexuteScalar(sql, GetSqlParameters(t, p => !p.ValidateIdentity()));
        }
        #endregion

        #region kongge 2019-04-01 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="dic">修改条件（key='字段名',value='字段值'）</param>
        /// <returns></returns>
        public int Upd<T>(Expression<Func<T, T>> expCloumn, Expression<Func<T, bool>> expWhere) where T : BaseModel
        {
            if (expCloumn == null || expCloumn.Body == null)
            {
                throw new Exception("未传入修改字段");
            }
            string cloumn = ExpressionAnalysis.GetSql(expCloumn);
            SqlParameter[] list = ExpressionAnalysis.GetListParameter().ToArray();
            if (String.IsNullOrWhiteSpace(cloumn))
            {
                throw new Exception("未传入修改字段");
            }
            string sql = string.Format("update {0} set {1} ", typeof(T).GetTableName(), cloumn);
            Tuple<string, List<SqlParameter>> tu = GetWhere(expWhere);
            sql += tu.Item1;
            List<SqlParameter> list2 = list.Union(tu.Item2.ToArray()).ToList();
            return helpers.ExexuteSql(sql, list2);
        }
        public int Upd<T>(T t, Expression<Func<T, bool>> expWhere) where T : BaseModel
        {
            List<SqlParameter> list = GetSqlParameters(t, p => !p.ValidateIdentity() && !p.ValidatePrimary());
            string sql = CacheSqlBuilder<T>.UpdSql;
            Tuple<string, List<SqlParameter>> tu = GetWhere(expWhere);
            sql += tu.Item1;
            list.AddRange(tu.Item2);
            return helpers.ExexuteSql(sql, list);
        }
        #endregion

        #region kongge 2019-04-01 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="dic">删除条件（key='字段名',value='字段值'）</param>
        /// <returns></returns>
        public int Del<T>(Expression<Func<T, bool>> exp) where T : BaseModel
        {
            string sql = CacheSqlBuilder<T>.DelSql;

            Tuple<string, List<SqlParameter>> tu = GetWhere(exp);
            sql += tu.Item1;
            return helpers.ExexuteSql(sql, tu.Item2);
        }
        #endregion

        #region kongge 2019-04-01 获取实体对象
        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic">删除条件（key='字段名',value='字段值'）</param>
        /// <returns></returns>
        public T GetModel<T>(Expression<Func<T, bool>> exp) where T : BaseModel
        {
            Type type = typeof(T);
            object obj = Activator.CreateInstance(type);
            string sql = CacheSqlBuilder<T>.SelSql;
            Tuple<string, List<SqlParameter>> tu = GetWhere(exp);
            sql += tu.Item1;
            return helpers.ExexuteDataReader<T>(sql, read =>
            {
                if (read.Read())
                {
                    foreach (var item in type.GetProperties())
                    {
                        item.SetValue(obj, read[item.Name] is DBNull ? null : read[item.Name], null);
                    }
                }
                return (T)obj;
                //return DataReaderExpression<T>.SqlDataReaderToModel(read);
            }, tu.Item2);
        }
        #endregion

        #region kongge 2019-04-01 获取实体对象集合
        /// <summary>
        /// 获取实体对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetModelToList<T>(Expression<Func<T, bool>> exp) where T : BaseModel
        {
            Type type = typeof(T);
            string sql = CacheSqlBuilder<T>.SelSql;
            Tuple<string, List<SqlParameter>> tu = GetWhere(exp);
            sql += tu.Item1;
            return helpers.ExexuteDataReader(sql, read =>
            {
                List<T> list = new List<T>();
                while (read.Read())
                {
                    object obj = Activator.CreateInstance(type);
                    foreach (var item in type.GetProperties())
                    {
                        item.SetValue(obj, read[item.Name] is DBNull ? null : read[item.Name], null);
                    }
                    list.Add((T)obj);
                }
                return list;
                //return DataReaderExpression<T>.SqlDataReaderToList(read);
            }, tu.Item2);
        }
        #endregion

        #region kongge 2019-04-01 获取总数据量
        /// <summary>
        /// 获取总数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public int GetCount<T>(Expression<Func<T, bool>> exp) where T : BaseModel
        {
            string sql = CacheSqlBuilder<T>.CountSql;
            Tuple<string, List<SqlParameter>> tu = GetWhere(exp);
            sql += tu.Item1;
            object obj = helpers.ExexuteScalar(sql, tu.Item2);
            if (obj != null)
            {
                return Convert.ToInt32(obj);
            }
            return 0;
        }
        #endregion

        #region kongge 2019-04-01 获取总数据-单表
        /// <summary>
        /// 获取总数据-单表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic"></param>
        /// <param name="order">排序（id desc/id asc）</param>
        /// <param name="list">查询字段</param>
        /// <returns></returns>
        public DataTable GetList<T>(Expression<Func<T, bool>> exp, string order = "", params string[] list) where T : BaseModel
        {
            string sql = "";
            if (list != null && list.Length > 0)
            {
                sql = string.Format("select {0} from {1}", string.Join(",", list), typeof(T).GetTableName());
            }
            else
            {
                sql = CacheSqlBuilder<T>.SelSql;
            }
            Tuple<string, List<SqlParameter>> tu = GetWhere(exp);
            sql += tu.Item1;
            if (String.IsNullOrWhiteSpace(order))
            {
                //未定义排序字段，默认主键倒序
                order = string.Format(" {0} desc", typeof(T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance).First(p => p.ValidatePrimary()).GetColumnName());
            }
            sql += string.Format(" order by {0} ", order);
            return helpers.ExexuteDataTable(sql, tu.Item2);
        }
        #endregion

        #region kongge 2019-04-01 分页获取数据-单表
        /// <summary>
        /// 分页获取数据-单表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic"></param>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <param name="order">排序（id desc/id asc）</param>
        /// <param name="list">查询字段</param>
        /// <returns></returns>
        public DataTable GetListByPage<T>(Expression<Func<T, bool>> exp, int s, int e, string order = "", params string[] list) where T : BaseModel
        {
            Type type = typeof(T);
            IEnumerable<PropertyInfo> iEnumerable = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            Tuple<string, List<SqlParameter>> tu = GetWhere(exp);
            StringBuilder sql = new StringBuilder();
            if (String.IsNullOrWhiteSpace(order))
            {
                //未定义排序字段，默认主键倒序
                order = string.Format(" {0} desc", iEnumerable.First(p => p.ValidatePrimary()).GetColumnName());
            }
            sql.Append(" select * from (");
            sql.AppendFormat(" select ROW_NUMBER() OVER (order by {0} )AS Row,", order);
            if (list != null && list.Length > 0)
            {
                sql.Append(string.Join(",", list));
            }
            else
            {
                //未定义查询字段，默认查询所有字段
                sql.Append(string.Join(",", iEnumerable.Select(p => "[" + p.GetColumnName() + "] as [" + p.Name + "]")));
            }
            sql.AppendFormat(" from {0}  {1}", type.GetTableName(), tu.Item1);
            sql.AppendFormat(") t where Row between {0} and {1}", s, e);
            return helpers.ExexuteDataTable(sql.ToString(), tu.Item2);
        }
        #endregion
        
        #region kongge 2019-04-01 获取sql参数集合
        /// <summary>
        /// 获取sql参数集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private static List<SqlParameter> GetSqlParameters<T>(T t, Func<PropertyInfo, bool> func = null)
        {
            Type type = t.GetType();
            var list = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            if (func != null)
            {
                return list.Where(func).Select(p => new SqlParameter(string.Format("@{0}", p.Name), p.GetValue(t, null) ?? DBNull.Value)).ToList();
            }
            else
            {
                return list.Select(p => new SqlParameter(string.Format("@{0}", p.Name), p.GetValue(t, null) ?? DBNull.Value)).ToList();
            }
        }
        #endregion
    }
}
