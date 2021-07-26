using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel
{
    public class CLog
    {
        static HashSet<string> log = new HashSet<string>();
        static HashSet<string> warning = new HashSet<string>();
        static HashSet<string> error = new HashSet<string>();

        public static void OutputAndClearCache(string title)
        {
            Output(title);
            Clear();
        }

        const string split = "******************** {0} ************************";

        public static void Output(string title= ConstString.STR_EMPTY)
        {
            Console.ForegroundColor = ConsoleColor.White;
            foreach (var item in log)
            {
                Console.WriteLine(item);
            }
            if (log.Count > 0)
            {
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var item in warning)
            {
                Console.WriteLine(item);
            }
            if (warning.Count > 0)
            {
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var item in error)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(string.Format(split, title));
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Clear()
        {
            log.Clear();
            warning.Clear();
            error.Clear();
        }

        public static void Log(string fmt, params object[] pms)
        {
            string s =  fmt;
            if (pms != null && pms.Length != 0)
            {
                s = string.Format(s, pms);
            }
            lock (log)
            {
                log.Add(s);
            }
        }

        const string Warning = "Warning: ";
        public static void LogWarning(string fmt, params object[] pms)
        {

            string s = Warning + fmt;
            if (pms != null && pms.Length != 0)
            {
                s = string.Format(s, pms);
            }

        
            lock (warning)
            {
                warning.Add(s);
            }
        }

        const string Error = "Error: ";
        public static void LogError(string fmt, params object[] pms)
        {
            string s = Error+fmt;
            if (pms != null&& pms.Length != 0)
            {
                s = string.Format(s, pms);
            }

            lock (error)
            {
                error.Add(s);
            }
        }
    }
}
