using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Common.ExtendAttributes.ValidateExtend
{
    /// <summary>
    /// 验证非空
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredAttribute : AbstractValidateAttribute
    {
        public RequiredAttribute(string _msg)
            : base(o =>
            {
                if (o != null && !string.IsNullOrWhiteSpace(o.ToString()))
                {
                    return "";
                }
                return _msg;
            })
        {
        }
    }
}
