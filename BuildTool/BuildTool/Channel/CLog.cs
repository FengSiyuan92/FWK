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

        const string split = "********************{0}************************";

        public static void Output(string title= ConstString.STR_EMPTY)
        {
            foreach (var item in log)
            {
                Console.WriteLine(item);
            }
            if (log.Count > 0)
            {
                Console.WriteLine();
            }
       
            foreach (var item in warning)
            {
                Console.WriteLine(item);
            }
            if (warning.Count > 0)
            {
                Console.WriteLine();
            }
            foreach (var item in error)
            {
                Console.WriteLine(item);
            }
        
            Console.WriteLine(string.Format(split, title));
        
        }

        public static void Clear()
        {
            log.Clear();
            warning.Clear();
            error.Clear();
        }

        public static void Log(string fmt, params object[] pms)
        {
            var s = string.Format(fmt, pms);
            lock (log)
            {
                log.Add(s);
            }
        }

        const string Warning = "Warning: ";
        public static void LogWarning(string fmt, params object[] pms)
        {
            var s = string.Format(Warning + fmt, pms);
            lock (warning)
            {
                warning.Add(s);
            }
        }

        const string Error = "Error: ";
        public static void LogError(string fmt, params object[] pms)
        {
            var s = string.Format(Error + fmt, pms);
            lock (error)
            {
                error.Add(s);
            }
        }
    }
}
