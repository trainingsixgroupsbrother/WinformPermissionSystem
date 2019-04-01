using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Permission.Common.ExtendAttributes.ValidateExtend
{
    /// <summary>
    /// 验证邮箱
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class EmailAttribute : AbstractValidateAttribute
    {
        private static string EmailRegular = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        public EmailAttribute(string _msg)
            : base(o =>
            {
                if (o != null && Regex.IsMatch(o.ToString(), EmailRegular))
                {
                    return "";
                }
                return _msg;
            })
        {
        }
    }
}
