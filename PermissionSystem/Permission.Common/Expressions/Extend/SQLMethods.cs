using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Permission.Common.Expressions.Extend
{
    public static class SQLMethods
    {
        public static bool DB_In<T>(this T t, List<T> list)  // in
        {
            return true;
        }
    }
}
