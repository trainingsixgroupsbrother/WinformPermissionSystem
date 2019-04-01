using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Common.ExtendAttributes.ValidateExtend
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public abstract class AbstractValidateAttribute : Attribute
    {
        private Func<object, string> _Func;
        public AbstractValidateAttribute(Func<object, string> func)
        {
            this._Func = func;
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="oValue">验证值</param>
        /// <returns></returns>
        public string ValidateSelf(object oValue)
        {
            return this._Func.Invoke(oValue);
        }
    }
}
