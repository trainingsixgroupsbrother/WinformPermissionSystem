using Permission.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.DAL
{
    public class DBHelpers
    {
        #region kongge 2019-04-01 缓存数据库所有链接
        private static Dictionary<Enums.DBEnum, string> dicDb = new Dictionary<Enums.DBEnum, string>();
        static DBHelpers()
        {
            dicDb[Enums.DBEnum.write] = WebConfigManage.PermissionWrite;
            dicDb[Enums.DBEnum.read] = WebConfigManage.PermissionRead;
        }
        #endregion

        #region kongge 2019-04-01 初始化数据库链接字段
        /// <summary>
        /// 数据库链接字段
        /// </summary>
        public string connectionString = string.Empty;

        /// <summary>
        /// 默认获取 读 链接
        /// </summary>
        public DBHelpers()
        {
            this.connectionString = dicDb[Enums.DBEnum.read];
        }

        /// <summary>
        /// 自定义链接数据库
        /// </summary>
        /// <param name="dBEnum"></param>
        public DBHelpers(Enums.DBEnum dBEnum)
        {
            this.connectionString = dicDb[dBEnum];
        }
        #endregion

        #region kongge 2019-04-01 执行SQL语句，自定义返回结果
        /// <summary>
        /// 执行SQL语句，自定义返回结果
        /// </summary>
        /// <typeparam name="T">返回结果类型</typeparam>
        /// <param name="sql">执行SQL</param>
        /// <param name="func">自定义方式返回</param>
        /// <param name="sqlParameters">SQL参数集合</param>
        /// <returns></returns>
        private T ExecuteSQL<T>(string sql, Func<SqlCommand, T> func, List<SqlParameter> sqlParameters)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    sqlParameters.ForEach(p =>
                    {
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input)
                        && p.Value == null)
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    });
                    return func(command);
                }
            }
        }
        #endregion

        #region kongge 2019-04-01 使用事务执行SQL语句，自定义返回结果
        /// <summary>
        /// 使用事务执行SQL语句，自定义返回结果
        /// </summary>
        /// <typeparam name="T">返回结果类型</typeparam>
        /// <param name="sql">执行SQL</param>
        /// <param name="conn">数据库链接</param>
        /// <param name="func">自定义方式返回</param>
        /// <param name="listParameter">SQL参数集合</param>
        /// <returns></returns>
        private T ExecuteTranSQL<T>(string sql, SqlConnection conn, Func<SqlCommand, T> func, List<SqlParameter> listParameter)
        {
            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                command.CommandType = CommandType.Text;
                if (listParameter != null && listParameter.Count > 0)
                {
                    listParameter.ForEach(l =>
                    {
                        if ((l.Direction == ParameterDirection.InputOutput || l.Direction == ParameterDirection.Input) &&
                            (l.Value == null))
                        {
                            l.Value = DBNull.Value;
                        }
                        command.Parameters.Add(l);
                    });
                }
                return func(command);
            }
        }
        #endregion

        #region kongge 2019-04-01 根据SQL获取SqlDataReader
        /// <summary>
        /// 根据SQL获取SqlDataReader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T ExexuteDataReader<T>(string sql, Func<SqlDataReader, T> func, List<SqlParameter> parameters = null)
        {
            return ExecuteSQL<T>(sql, command =>
            {
                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    return func(reader);
                }
            }, parameters);
        }
        #endregion

        #region kongge 2019-04-01 根据SQL获取DataTable
        /// <summary>
        /// 根据SQL获取DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExexuteDataTable(string sql, List<SqlParameter> parameters = null)
        {
            return ExecuteSQL<DataTable>(sql, command =>
            {
                using (SqlDataAdapter da = new SqlDataAdapter(command))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "ds");
                    command.Parameters.Clear();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                    return null;
                }
            }, parameters);
        }
        #endregion

        #region kongge 2019-04-01 根据SQL获取返回的结果集中第一行的第一列
        /// <summary>
        /// 根据SQL获取返回的结果集中第一行的第一列
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">sql中的参数</param>
        /// <returns></returns>
        public object ExexuteScalar(string sql, List<SqlParameter> parameters = null)
        {
            return ExecuteSQL<object>(sql, command =>
            {
                return command.ExecuteScalar();
            }, parameters);
        }
        #endregion

        #region kongge 2019-04-01 执行数据增删改方法
        /// <summary>
        /// 执行数据增删改方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">sql中的参数</param>
        /// <param name="tran">事务</param>
        /// <returns></returns>
        public int ExexuteSql(string sql, List<SqlParameter> parameters = null)
        {
            //return 0;
            return ExecuteSQL<int>(sql, command =>
            {
                return command.ExecuteNonQuery();
            }, parameters);
        }
        #endregion

        #region kongge 2019-04-01 使用事务执行数据增删改方法
        /// <summary>
        /// 使用事务执行数据增删改方法
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="conn">数据库链接</param>
        /// <param name="tran">事务</param>
        /// <param name="parameters">sql中的参数</param>
        /// <returns></returns>
        public int ExexuteSql(string sql, SqlConnection conn, SqlTransaction tran, List<SqlParameter> parameters = null)
        {
            //return 0;
            return ExecuteTranSQL<int>(sql, conn, command =>
            {
                if (tran != null)
                {
                    command.Transaction = tran;
                }
                return command.ExecuteNonQuery();
            }, parameters);
        }
        #endregion
    }
}
