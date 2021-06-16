using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel
{
    class GlobalArgs
    {
        /// <summary>
        /// 忽略读取的文件路径
        /// </summary>
        public static List<string> ignoreFiles = new List<string>();

        public static bool IsIgnoreFile(string filePath)
        {
            return ignoreFiles.Contains(filePath);
        }
    }
}
