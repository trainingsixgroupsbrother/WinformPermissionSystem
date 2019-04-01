using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Permission.Common.ExtendAttributes.ModelExtend
{
    /// <summary>
    /// 设置列名
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string ColumnName { get; private set; }
        public ColumnAttribute(string _ColumnName)
        {
            this.ColumnName = _ColumnName;
        }
    }
}
