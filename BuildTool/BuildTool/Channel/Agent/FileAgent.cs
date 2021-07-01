using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Channel.FileAgent.Excel;
namespace Channel.FileAgent
{
    internal interface IFileAgent 
    {
        bool Valid { get; }

        void LoadDefine();

        void Compile();

        void LoadContent();

    }


    // todo 不能是public的
    public static  class Enter
    {
        static List<IFileAgent> agents = new List<IFileAgent>();

        public static void Start(List<string> filePaths)
        {
            EnterByFiles(filePaths);
            StartLoadDefine();
        }


        static void EnterByFiles(List<string> filePaths)
        {
            agents.Clear();
            for (int i = 0; i < filePaths.Count; i++)
            {
                var ex = Path.GetExtension(filePaths[i]);
                IFileAgent agent=  null;
                switch (ex)
                { 

                    case "xlsx":
                        agent = new ExcelAgent(filePaths[i]);
                        break;
                    case "xml":
                    case "txt":
                        agent = new ExcelAgent(filePaths[i]);
                        break;

                    default:
                        break;
                }
                if (agent != null)
                {
                    agents.Add(agent);
                }
            }
        }

        // togo 改为多线程并发
        static void StartLoadDefine()
        {
            foreach (var agent in agents)
            {
                if (agent.Valid)
                {
                    agent.LoadDefine();
                }
            }
        }

        static void StartCompile()
        {
            foreach (var agent in agents)
            {
                if (agent.Valid)
                {
                    agent.Compile();
                }
            }
        }


        static void LoadConent()
        {

        }
    }


}
