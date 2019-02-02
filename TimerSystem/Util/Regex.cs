using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerSystem.Util
{
    class Regex
    {
        public static string RemoveIntegers(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, @"[\d]", string.Empty);
        }
    }
}
