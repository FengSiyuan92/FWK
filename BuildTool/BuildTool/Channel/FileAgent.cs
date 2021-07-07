using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Channel.Agent.XML;
using Channel.Agent.Excel;

namespace Channel
{
    interface IFileAgent
    {
        void LoadDefine();
        void LoadContent();
        bool Valid { get; }
    }

    public class FileAgent
    {
        static string[] ValidExtend = new string[]
        {
            "*.xml",
            "*.xlsx",
        };

        static Dictionary<string, IFileAgent> allAgents = new Dictionary<string, IFileAgent>();
        /// <summary>
        /// 注册一个源文件,可以使用多线程启动注册
        /// </summary>
        /// <param name="filePath"></param>
        public static void RegisterFile(string filePath)
        {

            var fileName = Path.GetFileName(filePath);
            if (fileName.StartsWith("$") || fileName.StartsWith("~"))
            {
                return;
            }

            var ex = Path.GetExtension(filePath);
            IFileAgent agent = null;
            switch (ex)
            {
                case ".xml":
                    agent = new XMLAgent(filePath);
                    break;

                case ".xlsx": case ".xls":
                    agent = new ExcelAgent(filePath);
                    break;
                default:
                    CLog.LogError("暂时未支持解析{0}类型的文件:所在路径{1}", ex, filePath);
                    break;
            }

            if (agent == null) return;
 

            if (agent.Valid)
            {
                lock (allAgents)
                {
                    allAgents.Add(filePath, agent);
                }
            }
            else
            {
                CLog.LogError("{0}不是有效文件路径,请检查", filePath);
            }
        }

        /// <summary>
        /// 注册多个文件,建议使用这种方法,内部会根据GlobalArgs配置的,决定是否进行并发文件IO
        /// </summary>
        /// <param name="filePathCollect"></param>
        public static void RegisterFiles(IEnumerable<string> filePathCollect)
        {
            Utils.Parallel(filePathCollect, RegisterFile);
        }

        /// <summary>
        /// 注册一个文件夹,里面所有支持的类型文件(xml\xlsx)都将被导入
        /// </summary>
        /// <param name="dirPath"></param>
        public static void RegisterDir(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                CLog.LogError("注册的文件夹路径不存在=>" + dirPath);
                return;
            }
            HashSet<string> filePath = new HashSet<string>();
            FindDirValidFile(dirPath, filePath);
            RegisterFiles(filePath);
            CLog.OutputAndClearCache("文件读取完成");
        }

        static void FindDirValidFile(string dirpath, HashSet<string> vessel)
        {
            DirectoryInfo dir = new DirectoryInfo(dirpath);

            for (int i = 0;  i < ValidExtend.Length; i++)
            {
                var files = dir.GetFiles(ValidExtend[i], SearchOption.AllDirectories).Select(f=>f.FullName);
                vessel.UnionWith(files);
            }
        }

        internal static void LoadAllDefine()
        {
            Utils.Parallel(allAgents.Values, LoadItemDefine);
        }

        internal static void LoadContent()
        {
            Utils.Parallel(allAgents.Values, LoadItemContent);
        }

        static void LoadItemDefine(IFileAgent agent)
        {
            agent.LoadDefine();
        }

        static void LoadItemContent(IFileAgent agent)
        {
            agent.LoadContent();
        }
    }
}
