using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enum = Channel.Define.Enum;

namespace Channel
{
    namespace Channel.Viewer
    {
        public class EnumViewer
        {
            internal Dictionary<string, Enum> enums = new Dictionary<string, Enum>();
            /// <summary>
            /// 编译完成后,可以通过该接口查看一个枚举信息
            /// </summary>
            /// <param name="enumName"></param>
            /// <returns></returns>
            public Enum this[string enumName] => GetEnumInfoByClassName(enumName);

            /// <summary>
            /// 编译完成后,可以通过该接口查看某个类型的的枚举信息, 该接口可以直接使用索引器代替
            /// </summary>
            /// <param name="enumName"></param>
            /// <returns></returns>
            public Enum GetEnumInfoByClassName(string enumName)
            {
                if (string.IsNullOrEmpty(enumName))
                {
                    return null;
                }
                Enum e = null;
                enums.TryGetValue(enumName, out e);
                return e;
            }

            /// <summary>
            /// 编译完成后,可以通过该接口查看当前环境下一共有哪些名称的枚举
            /// </summary>
            /// <param name="enumName"></param>
            /// <returns></returns>
            public string[] AllName()
            {
                return enums.Keys.ToArray();
            }
        }

    }

    public static partial class Lookup
    {
        static object enumLock = new object();

        /// <summary>
        /// 内部通过该接口注册进一个枚举定义
        /// </summary>
        /// <param name="enumObj"></param>
        internal static void AddEnumDefine(Enum enumObj)
        {
            lock (enumLock)
            {
                Enum.enums.Add(enumObj.Name, enumObj);
            }
        }

        static Channel.Viewer.EnumViewer enumLookupInstance = new Channel.Viewer.EnumViewer();
        /// <summary>
        /// 枚举查看器
        /// </summary>
        public static Channel.Viewer.EnumViewer Enum => enumLookupInstance;

    }
}
