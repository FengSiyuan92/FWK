using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define;
namespace Channel
{
    namespace Channel.Viewer
    {
        public class ClassInfoViewer
        {
            internal Dictionary<string, CustomClass> defines = new Dictionary<string, CustomClass>();
            /// <summary>
            /// 编译完成后,可以通过该接口查看一个自定义类型的信息
            /// </summary>
            /// <param name="enumName"></param>
            /// <returns></returns>
            public CustomClass this[string typeName] => GetTypeInfoByClassName(typeName);

            /// <summary>
            /// 通过类型名称, 获取自定义类型信息
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public CustomClass GetTypeInfoByClassName(string typeName)
            {
                if (string.IsNullOrEmpty(typeName))
                {
                    return null;
                }
                CustomClass def = null;
                defines.TryGetValue(typeName, out def);
                return def;
            }

            /// <summary>
            /// 编译完成后,可以通过该接口查看当前环境下一共有哪些名称的枚举
            /// </summary>
            /// <param name="enumName"></param>
            /// <returns></returns>
            public string[] AllName()
            {
                return defines.Keys.ToArray();
            }
        }
    }
    public static partial class Lookup
    {

        static object customLock = new object();

        /// <summary>
        /// 内部通过该接口注册进一个枚举定义
        /// </summary>
        /// <param name="enumObj"></param>
        internal static void AddObjDefine(CustomClass objDef)
        {
            lock (customLock)
            {
                ClassInfo.defines.Add(objDef.Name, objDef);
            }
        }

        static Channel.Viewer.ClassInfoViewer customTypeViewer = new Channel.Viewer.ClassInfoViewer();
        /// <summary>
        /// 自定义类型查看器
        /// </summary>
        public static Channel.Viewer.ClassInfoViewer ClassInfo => customTypeViewer;


    }
}
