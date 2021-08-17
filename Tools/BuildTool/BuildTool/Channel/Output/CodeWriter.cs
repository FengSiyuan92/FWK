using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Channel
{

    public static class CodeWriter
    {
        public enum CodeType
        {
            // 开发版本的lua代码. 注重可读性,不考虑文件体积压缩\common拆分等问题
            DevLua = 0,
            // 长字符走引用,重复table走引用,删除空格,使用数组存储数据而非kv形式,压缩配置文件大小.
            Lua = 1,
        }

        public static void WriteCode(string outputDir, CodeType codeType)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();
            switch (codeType)
            {
                case CodeType.DevLua:
                    Output.LuaOutput.DevLuaWriter.Write(outputDir);
                    break;
                case CodeType.Lua:
                    Output.LuaOutput.LuaWriter.Write(outputDir);
                    break;
                
                default:
                    break;
            }
            sw.Stop();
            CLog.OutputAndClearCache(string.Format("写入文件结束, 写入文件耗时{0}s", sw.Elapsed.TotalSeconds) );
        }
    }
}
