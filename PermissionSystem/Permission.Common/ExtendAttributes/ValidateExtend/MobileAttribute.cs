using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Permission.Common.ExtendAttributes.ValidateExtend
{
    /// <summary>
    /// 验证手机号
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MobileAttribute : AbstractValidateAttribute
    {
        private static string MobileRegular = @"^[1]+\d{10}";
        public MobileAttribute(string _msg)
            : base(o =>
            {
                if (o != null && Regex.IsMatch(o.ToString(), MobileRegular))
                {
                    return "";
                }
                return _msg;
            })
        {
        }
    }
}
