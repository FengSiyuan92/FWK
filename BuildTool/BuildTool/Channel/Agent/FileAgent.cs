using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Channel.Agent.XML;
using Channel.Agent.Excel;
using System.Diagnostics;

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
        static void RegisterFile(string filePath)
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
        /// 注册路径集合,可以传入文件夹或者文件具体路径(需要包括文件后缀名)
        /// 如果集合中包含文件夹则将会深度递归注册文件夹里所有有效文件
        /// </summary>
        /// <param name="filePathCollect"></param>
        public static void Register(IEnumerable<string> pathCollect)
        {
            foreach (var item in pathCollect)
            {
                RegisterInternal(item);
            }
            CLog.OutputAndClearCache("文件注册完成");
        }

        /// <summary>
        /// 注册文件路径,可以传入文件夹或者文件具体路径(需要包括文件后缀名)
        /// 如果传入文件夹,则将会深度递归注册文件夹里所有有效文件
        /// </summary>
        /// <param name="path"></param>
        public static void Register(string path)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            RegisterInternal(path);
            sw.Stop();
            CLog.OutputAndClearCache(string.Format("文件注册完成,耗时{0}s", sw.Elapsed.TotalSeconds));
        }

        static void RegisterInternal(string path)
        {
            if (Directory.Exists(path))
            {
                RegisterDir(path);
            }
            else if (File.Exists(path))
            {
                RegisterFile(path);
            }
        }

        /// <summary>
        /// 注册一个文件夹,里面所有支持的类型文件(xml\xlsx)都将被导入
        /// </summary>
        /// <param name="dirPath"></param>
        static void RegisterDir(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                CLog.LogError("注册的文件夹路径不存在=>" + dirPath);
                return;
            }
            HashSet<string> filePath = new HashSet<string>();
            FindDirValidFile(dirPath, filePath);
            Utils.Parallel(filePath, RegisterFile);
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
