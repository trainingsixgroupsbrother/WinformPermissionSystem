using Permission.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Permission.IBLL
{
    public interface IBaseBLL
    {
        #region kongge 2019-04-01 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        int Add<T>(T t) where T : BaseModel;
        /// <summary>
        /// 返回新增ID，主键必须是自增长主键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        object AddScalar<T>(T t) where T : BaseModel;
        #endregion

        #region kongge 2019-04-01 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="dic">修改条件</param>
        /// <returns></returns>
        int Upd<T>(Expression<Func<T, T>> expCloumn, Expression<Func<T, bool>> expWhere) where T : BaseModel;

        int Upd<T>(T t, Expression<Func<T, bool>> expWhere) where T : BaseModel;
        #endregion

        #region kongge 2019-04-01 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="dic">删除条件</param>
        /// <returns></returns>
        int Del<T>(Expression<Func<T, bool>> exp) where T : BaseModel;
        #endregion

        #region kongge 2019-04-01 获取实体对象
        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetModel<T>(Expression<Func<T, bool>> exp) where T : BaseModel;
        #endregion

        #region kongge 2019-04-01 获取实体对象集合
        /// <summary>
        /// 获取实体对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetModelToList<T>(Expression<Func<T, bool>> exp) where T : BaseModel;
        #endregion

        #region kongge 2019-04-01 获取总数据量
        /// <summary>
        /// 获取总数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        int GetCount<T>(Expression<Func<T, bool>> exp) where T : BaseModel;
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
        DataTable GetList<T>(Expression<Func<T, bool>> exp, string order = "", params string[] list) where T : BaseModel;
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
        DataTable GetListByPage<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> exp, string order = "", params string[] list) where T : BaseModel;
        #endregion
    }
}
