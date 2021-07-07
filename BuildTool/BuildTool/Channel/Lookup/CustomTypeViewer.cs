using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define.Class;
namespace Channel
{
    public static partial class Lookup
    {

        static object customLock = new object();

        public class CustomTypeViewer
        {
            internal Dictionary<string, CustomType> defines = new Dictionary<string, CustomType>();
            /// <summary>
            /// 编译完成后,可以通过该接口查看一个自定义类型的信息
            /// </summary>
            /// <param name="enumName"></param>
            /// <returns></returns>
            public CustomType this[string typeName]
            {
                get
                {
                    if (string.IsNullOrEmpty(typeName))
                    {
                        return null;
                    }
                    CustomType def = null;
                    defines.TryGetValue(typeName, out def);
                    return def;
                }
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

        /// <summary>
        /// 内部通过该接口注册进一个枚举定义
        /// </summary>
        /// <param name="enumObj"></param>
        internal static void AddObjDefine(CustomType objDef)
        {
            lock (customLock)
            {
                CustomType.defines.Add(objDef.Name, objDef);
            }
        }

        static CustomTypeViewer customTypeViewer;
        /// <summary>
        /// 自定义类型查看器
        /// </summary>
        public static CustomTypeViewer CustomType
        {
            get
            {
                customTypeViewer = customTypeViewer ?? new CustomTypeViewer();
                return customTypeViewer;
            }
        }
    }
}
