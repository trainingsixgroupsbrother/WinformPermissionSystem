using Permission.Common.Expressions.Extend;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Permission.Common.Expressions
{
    public class ExpressionAnalysis
    {
        private static List<SqlParameter> list = new List<SqlParameter>();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<SqlParameter> GetListParameter()
        {
            return list;
        }
        /// <summary>
        /// 获取where条件
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string GetSql(Expression exp)
        {
            return DealExpress(exp);
        }
        /// <summary>
        /// 表达式目录树解析入口
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private static string DealExpress(Expression exp)
        {
            string name = exp.GetType().Name;
            if (exp == null)
            {
                return "";
            }
            if (exp is LambdaExpression)
            {
                list.Clear();
                LambdaExpression lambda = exp as LambdaExpression;
                return DealExpress(lambda.Body);
            }
            if (exp is BinaryExpression)
            {
                return DealBinaryExpression(exp as BinaryExpression);
            }
            if (exp is MemberExpression)
            {
                return DealMemberExpression(exp as MemberExpression);
            }
            if (exp is ConstantExpression)
            {
                return DealConstantExpression(exp as ConstantExpression);
            }
            if (exp is UnaryExpression)
            {
                return DealUnaryExpression(exp as UnaryExpression);
            }
            if (exp is MethodCallExpression)
            {
                return DealMethodsCall(exp as MethodCallExpression);
            }
            if (exp is ListInitExpression)
            {
                return DealListInit(exp as ListInitExpression);
            }
            if (exp is MemberInitExpression)
            {
                return DealMemberInit(exp as MemberInitExpression);
            }
            return "";
        }
        /// <summary>
        /// 解析一元表达式
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private static string DealUnaryExpression(UnaryExpression exp)
        {
            UnaryExpression cast = Expression.Convert(exp.Operand, typeof(object));
            object obj = Expression.Lambda<Func<object>>(cast).Compile().Invoke();
            return GetValueFormat(obj);
            //return DealExpress(exp.Operand);
        }
        private static int i = 0;
        /// <summary>
        /// 解析二元表达式
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private static string DealBinaryExpression(BinaryExpression exp)
        {
            i++;
            string left = DealExpress(exp.Left);
            string oper = exp.NodeType.ToSqlOperator();
            string right = "";
            string _left = "", _right = "";
            if (exp.Left is MemberExpression)
            {
                _left = left;
            }
            if (exp.Right is ConstantExpression)
            {
                ConstantExpression constant = (ConstantExpression)exp.Right;
                _right = DealConstantExpression(constant);
                right = "@" + _left + i;
            }
            else if (exp.Right is MemberExpression)
            {
                _right = Eval(exp.Right as MemberExpression);
                if (!String.IsNullOrWhiteSpace(_right))
                {
                    right = "@" + _left + i;
                }
                else
                {
                    left = "";
                }
            }
            else
            {
                right = DealExpress(exp.Right);
            }
            if (!String.IsNullOrWhiteSpace(_left) && !String.IsNullOrWhiteSpace(_right))
            {
                list.Add(new SqlParameter("@" + _left + i, _right));
            }
            if (String.IsNullOrWhiteSpace(_right) && String.IsNullOrWhiteSpace(right))
            {
                return left;
            }
            return string.Format("({0})", left + oper + right);
        }


        /// <summary>
        /// 解析方法
        /// 支持自定义扩展方法
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private static string DealMethodsCall(MethodCallExpression exp)
        {
            i++;
            string _right = "", _left = "";
            if (exp.Method.DeclaringType == typeof(SQLMethods))
            {
                var exp1 = exp.Arguments[0];
                var exp2 = exp.Arguments[1];
                _left = DealExpress(exp1);
                if (exp2 is MemberExpression)
                {
                    _right = Eval(exp2 as MemberExpression);
                }
                else
                {
                    _right = DealExpress(exp2);
                    //_right=DealExpression(exp2);
                }
            }
            else
            {
                _left = DealExpress(exp.Object);
                var right = exp.Arguments[0];
                //_right = DealExpress(right);
                if (right is MemberExpression)
                {
                    _right = Eval(right as MemberExpression);
                }
                else
                {
                    _right = DealExpress(right);
                }
            }
            if (String.IsNullOrWhiteSpace(_right))
            {
                return "";
            }
            //string format;
            switch (exp.Method.Name)
            {
                //case "StartsWith":
                //    format = "({0} LIKE {1}%')";
                //    break;

                case "Contains":
                    //format = "({0} LIKE '%{1}%')";

                    list.Add(new SqlParameter("@" + _left + i, "%" + _right + "%"));
                    return _left + " LIKE @" + _left + i;
                case "DB_In":
                    //list.Add(new SqlParameter("@" + _left + i, _right));
                    return _left + " in (" + _right + ") ";

                //case "EndsWith":
                //    format = "({0} LIKE '%{1}')";
                //    break;

                default:
                    throw new NotSupportedException(exp.NodeType + " is not supported!");
            }
        }

        /// <summary>
        /// 解析属性
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private static string DealMemberExpression(MemberExpression exp)
        {
            return exp.Member.Name;
        }

        /// <summary>
        /// 解析常量
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private static string DealConstantExpression(ConstantExpression exp)
        {
            return GetValueFormat(exp.Value);
        }

        /// <summary>
        /// 解析变量，获取变量值
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private static string Eval(MemberExpression member)
        {
            UnaryExpression cast = Expression.Convert(member, typeof(object));
            object obj = Expression.Lambda<Func<object>>(cast).Compile().Invoke();
            if (obj != null && !String.IsNullOrWhiteSpace(obj.ToString()))
            {
                return obj.ToString();
            }
            return "";
        }
        /// <summary>
        /// 解析List集合
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private static string DealListInit(ListInitExpression exp)
        {
            UnaryExpression cast = Expression.Convert(exp, typeof(object));
            object obj = Expression.Lambda<Func<object>>(cast).Compile().Invoke();
            List<string> data = new List<string>();
            var list = obj as System.Collections.IEnumerable;
            foreach (var item in list)
            {
                data.Add(GetValueFormat(item, true));
            }
            return string.Join(",", data);
        }

        /// <summary>
        /// 解析初始化新对象的一个或多个成员
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private static string DealMemberInit(MemberInitExpression exp)
        {
            i++;
            List<string> member = new List<string>();
            foreach (MemberAssignment item in exp.Bindings)
            {

                object right = null;
                if (item.Expression is MemberExpression)
                {
                    right = Eval(item.Expression as MemberExpression);
                }
                else
                {
                    right = DealExpress(item.Expression);
                }
                member.Add(string.Format("{0}=@{1}", item.Member.Name, item.Member.Name));
                if (right == null || String.IsNullOrWhiteSpace(right.ToString()))
                {
                    right = DBNull.Value;
                }
                list.Add(new SqlParameter(string.Format("@{0}", item.Member.Name), right));
            }
            return string.Join(",", member);
        }

        private static string GetValueFormat(object vaule, bool b = false)
        {
            string v_str = string.Empty;
            if (vaule == null)
            {
                return "";
            }
            if (b && vaule is string)
            {
                v_str = string.Format("'{0}'", vaule.ToString());
                return v_str;
            }
            if (vaule is DateTime)
            {
                DateTime time = (DateTime)vaule;
                v_str = string.Format("{0}", time.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                v_str = vaule.ToString();
            }
            return v_str;
        }
    }
}
