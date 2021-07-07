using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel
{
    public class Parse
    {
        public static void StartParse()
        {
            FileAgent.LoadContent();
            Check.OverParse();
            CLog.OutputAndClearCache("数据解析完成");
        }
    }
}
