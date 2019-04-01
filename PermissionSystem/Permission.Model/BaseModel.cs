using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Model
{
    public class BaseModel
    {
        
    }

    public enum SqlOperAtion
    {
        Add,
        Upd,
        Del
    }
    public class TranModel
    {
        public BaseModel baseModel { get; set; }
        public SqlOperAtion sqlOperAtion { get; set; }
        public Dictionary<string, object> dic { get; set; }
    }
}
