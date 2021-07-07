using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel
{
    internal class GlobalArgs
    {
        /// <summary>
        /// 忽略读取的文件路径
        /// </summary>
        public static List<string> ignoreFiles = new List<string>();

        public static bool IsIgnoreFile(string filePath)
        {
            return ignoreFiles.Contains(filePath);
        }

        public static string targetPlatform = string.Empty;
        /// <summary>
        /// 是否是有效的配置平台
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool IsValidPlatform(string platform)
        {
            if (platform.StartsWith("#"))
            {
                return false;
            }
            if (string.IsNullOrEmpty(targetPlatform) || string.IsNullOrEmpty(platform))
            {
                return true;
            }
            return platform.Contains(targetPlatform);
        }


        public static bool ASYNC = false;
    }
}
