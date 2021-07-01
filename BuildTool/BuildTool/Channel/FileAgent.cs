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
        static Dictionary<string, IFileAgent> allAgents = new Dictionary<string, IFileAgent>();
        /// <summary>
        /// 注册一个源文件,可以使用多线程启动注册
        /// </summary>
        /// <param name="filePath"></param>
        public static void RegisterFile(string filePath)
        {
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


        internal static void LoadAllDefine()
        {
            foreach (var item in allAgents)
            {
                item.Value.LoadDefine();
            }
        }
    }
}
