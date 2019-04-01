using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Permission.Common.ExtendAttributes.ModelExtend
{
    /// <summary>
    /// 设置表名
    /// </summary>
    [AttributeUsage( AttributeTargets.Class)]
    public class TableAttribute:Attribute
    {
        public string TableName { get; private set; }
        public TableAttribute(string _TableName) 
        {
            this.TableName = _TableName;
        }
    }
}
