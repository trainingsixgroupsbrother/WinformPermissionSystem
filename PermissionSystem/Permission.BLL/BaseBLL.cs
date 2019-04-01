using Permission.Common;
using Permission.DAL;
using Permission.IBLL;
using Permission.IDAL;
using Permission.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Permission.BLL
{
    public class BaseBLL : IBaseBLL
    {
        private IBaseDAL dal = null;
        public BaseBLL(Enums.DBEnum _db)
        {
            dal = new BaseDAL(_db);
        }

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
            return dal.Add(t);
        }
        /// <summary>
        /// 返回新增ID，主键必须是自增长主键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public object AddScalar<T>(T t) where T : BaseModel
        {
            return dal.AddScalar(t);
        }
        #endregion

        #region kongge 2019-04-01 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="dic">修改条件</param>
        /// <returns></returns>
        public int Upd<T>(Expression<Func<T, T>> expCloumn, Expression<Func<T, bool>> expWhere) where T : BaseModel
        {
            return dal.Upd(expCloumn, expWhere);
        }
        public int Upd<T>(T t, Expression<Func<T, bool>> expWhere) where T : BaseModel
        {
            return dal.Upd(t, expWhere);
        }
        #endregion

        #region kongge 2019-04-01 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="dic">删除条件</param>
        /// <returns></returns>
        public int Del<T>(Expression<Func<T, bool>> exp) where T : BaseModel
        {
            return dal.Del<T>(exp);
        }
        #endregion

        #region kongge 2019-04-01 获取实体对象
        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetModel<T>(Expression<Func<T, bool>> exp) where T : BaseModel
        {
            return dal.GetModel<T>(exp);
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
            return dal.GetModelToList(exp);
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
            return dal.GetCount<T>(exp);
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
            return dal.GetList<T>(exp, order, list);
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
        public DataTable GetListByPage<T>(int pageIndex, int pageSize, Expression<Func<T, bool>> exp, string order = "", params string[] list) where T : BaseModel
        {
            Tuple<int, int> tu = Tools.CalPageIndex(pageIndex, pageSize);
            return dal.GetListByPage<T>(exp, tu.Item1, tu.Item2, order, list);
        }
        #endregion
    }
}
