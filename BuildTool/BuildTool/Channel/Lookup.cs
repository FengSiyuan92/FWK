using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.RawDefine;

namespace Channel
{
    public static class Lookup
    {

        public static void Test()
        {
            var a = 0;
        }

        static Dictionary<string, RawObjDef> RawDefineCollect = new Dictionary<string, RawObjDef>();

        /// <summary>
        /// 内部通过该接口注册原生定义到查看器
        /// </summary>
        /// <param name="def"></param>
        internal static void AddRawObjDef(RawObjDef def)
        {
            var key = def.Name;
            RawObjDef oldDef = null;
            lock (RawDefineCollect)
            {
                if (RawDefineCollect.TryGetValue(key, out oldDef))
                {
                    oldDef.Merge(def);
                }
                else
                {
                    RawDefineCollect.Add(key, def);
                }
            }
        }

        /// <summary>
        /// 内部通过该接口查看原生定义
        /// </summary>
        /// <param name="objTypeName"></param>
        /// <returns></returns>
        internal static RawObjDef LookupRawObjDef(string objTypeName)
        {
            RawObjDef def = null;
            RawDefineCollect.TryGetValue(objTypeName, out def);
            return def;
        }
        /// <summary>
        /// 内部通过该接口获取所有原生定义的类型
        /// </summary>
        /// <returns></returns>
        internal static List<string> LookAllRawDefineName()
        {
            return RawDefineCollect.Keys.ToList<string>();
        }

        static Dictionary<string, Channel.Enum> enums = new Dictionary<string, Channel.Enum>();

        /// <summary>
        /// 内部通过该接口注册进一个枚举定义
        /// </summary>
        /// <param name="enumObj"></param>
        internal static void AddEnumDefine(Channel.Enum enumObj)
        {
            enums.Add(enumObj.Name, enumObj);
        }

        /// <summary>
        /// 编译完成后,可以通过该接口查看一个枚举信息
        /// </summary>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static Channel.Enum LookEnum(string enumName)
        {
            if (string.IsNullOrEmpty(enumName))
            {
                return null; 
            }
            Channel.Enum e = null;
            enums.TryGetValue(enumName, out e);
            return e;
        }

        /// <summary>
        /// 编译完成后,可以通过该接口查看当前环境下一共有哪些名称的枚举
        /// </summary>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static string[] LookAllEnumName()
        {
            return enums.Keys.ToArray();
        }
    }
}
