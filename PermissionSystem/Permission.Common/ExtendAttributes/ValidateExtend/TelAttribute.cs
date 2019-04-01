using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Permission.Common.ExtendAttributes.ValidateExtend
{
    /// <summary>
    /// 验证电话
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TelAttribute : AbstractValidateAttribute
    {
        private static string TelRegular = @"(\d{2,4}-)\d{7,8}((-\d{1,6}))?";
        public TelAttribute(string _msg)
            : base(o =>
            {
                if (o != null && Regex.IsMatch(o.ToString(), TelRegular))
                {
                    return "";
                }
                return _msg;
            })
        {

        }
    }
}
