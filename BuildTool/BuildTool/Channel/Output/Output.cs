using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Channel
{

    public static class CodeWriter
    {
        public enum CodeType
        {
            Lua = 0,

        }
        public static void WriteCode(string outputDir, CodeType codeType)
        {

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            switch (codeType)
            {
                case CodeType.Lua:
                    Output.LuaOutput.LuaWriter.Write(outputDir);
                    break;
                default:
                    break;
            }
        }
    }
}
