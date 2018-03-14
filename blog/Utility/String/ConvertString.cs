using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.String
{
    public static class ConvertString
    {
        public static bool ConvertToBool(string value)
        {
            if (value.Equals("true"))
                return true;
            return false;
        }
        public static int? ConvertToInt(string value)
        {
            if(value.Equals("false"))
            {
                return null;
            }
            else
            {
                int? result = int.Parse(value);
                return result;
            }
        }
    }
}
