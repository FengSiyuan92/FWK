using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enum = Channel.Define.Class.Enum;
namespace Channel
{
    public static partial class Lookup
    {
        public class EnumViewer
        {
            internal Dictionary<string, Enum> enums = new Dictionary<string, Enum>();
            /// <summary>
            /// 编译完成后,可以通过该接口查看一个枚举信息
            /// </summary>
            /// <param name="enumName"></param>
            /// <returns></returns>
            public Enum this[string enumName]
            {
                get
                {
                    if (string.IsNullOrEmpty(enumName))
                    {
                        return null; 
                    }
                    Enum e = null;
                    enums.TryGetValue(enumName, out e);
                    return e;
                }
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

        static EnumViewer enumLookupInstance = new EnumViewer();
        /// <summary>
        /// 枚举查看器
        /// </summary>
        public static EnumViewer Enum
        {
            get
            {
                return enumLookupInstance;
            }
        }

    }
}
