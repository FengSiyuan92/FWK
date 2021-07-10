using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel;
using System.Text.RegularExpressions;
using System.IO;
namespace BuildTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Channel.Test.TestF();
            //HashSet<string> set = new HashSet<string>();
            var custom = Lookup.CustomType[""];
            var e = Lookup.Enum[""];
            var d = Lookup.Datas[""];
            Console.ReadKey();
        }

    }
}
