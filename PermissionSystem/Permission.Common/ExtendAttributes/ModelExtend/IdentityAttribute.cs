using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Permission.Common.ExtendAttributes.ModelExtend
{
    /// <summary>
    /// 设置自增长列
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IdentityAttribute : Attribute
    {

    }
}
