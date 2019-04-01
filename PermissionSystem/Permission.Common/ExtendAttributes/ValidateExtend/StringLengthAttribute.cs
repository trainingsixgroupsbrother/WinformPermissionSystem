using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Common.ExtendAttributes.ValidateExtend
{
    /// <summary>
    /// 验证字符串长度
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class StringLengthAttribute : AbstractValidateAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">最小长度</param>
        /// <param name="max">最大长度</param>
        /// <param name="_msg"></param>
        public StringLengthAttribute(int min, int max, string _msg)
            : base(o =>
            {
                if (o != null && o.ToString().Length >= min && o.ToString().Length <= max)
                {
                    return "";
                }
                return _msg;
            })
        {
        }
    }
}
