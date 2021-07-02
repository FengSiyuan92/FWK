using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel
{
    public static partial class Lookup
    {
        public class EnumViewer
        {
            internal Dictionary<string, Channel.Enum> enums = new Dictionary<string, Channel.Enum>();
            /// <summary>
            /// 编译完成后,可以通过该接口查看一个枚举信息
            /// </summary>
            /// <param name="enumName"></param>
            /// <returns></returns>
            public Channel.Enum this[string enumName]
            {
                get
                {
                    if (string.IsNullOrEmpty(enumName))
                    {
                        return null; 
                    }
                    Channel.Enum e = null;
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


        /// <summary>
        /// 内部通过该接口注册进一个枚举定义
        /// </summary>
        /// <param name="enumObj"></param>
        internal static void AddEnumDefine(Channel.Enum enumObj)
        {
            Enum.enums.Add(enumObj.Name, enumObj);
        }

        static EnumViewer enumLookupInstance;
        /// <summary>
        /// 枚举查看器
        /// </summary>
        public static EnumViewer Enum
        {
            get
            {
                enumLookupInstance = enumLookupInstance ?? new EnumViewer();
                return enumLookupInstance;
            }
        }

    }
}
