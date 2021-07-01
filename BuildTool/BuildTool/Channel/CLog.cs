using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel
{
    public class CLog
    {
        public static void Log(string fmt, params object[] pms)
        {
  
            Console.WriteLine(fmt, pms);
    
        }

        public static void LogWarning(string fmt, params object[] pms)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(fmt, pms);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void LogError(string fmt, params object[] pms)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(fmt, pms);
            Console.ForegroundColor = ConsoleColor.White;
        }


    }
}
