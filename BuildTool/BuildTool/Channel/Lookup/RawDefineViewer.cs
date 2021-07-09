using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.RawDefine;

namespace Channel
{
    namespace Channel.Viewer
    {
        internal class RawDefineViewer
        {
            internal static Dictionary<string, RawObjDef> RawDefineCollect = new Dictionary<string, RawObjDef>();
            /// <summary>
            /// 内部通过该接口查看原生定义
            /// </summary>
            /// <param name="objTypeName"></param>
            /// <returns></returns>
            public RawObjDef this[string objTypeName] => GetRawDefineByClassName(objTypeName);

            public RawObjDef GetRawDefineByClassName(string objTypeName)
            {
                RawObjDef def = null;
                RawDefineCollect.TryGetValue(objTypeName, out def);
                return def;
            }

            /// <summary>
            /// 内部通过该接口获取所有原生定义的类型
            /// </summary>
            /// <returns></returns>
            public string[] AllName()
            {
                return RawDefineCollect.Keys.ToArray();
            }
        }

    }
    public static partial class Lookup
    {
       
        static object rawDefLock = new object();
        /// <summary>
        /// 内部通过该接口注册原生定义到查看器
        /// </summary>
        /// <param name="def"></param>
        internal static void AddRawDef(RawObjDef def)
        {
            lock(rawDefLock)
            {
                var key = def.Name;
                RawObjDef oldDef = null;
                if (Channel.Viewer.RawDefineViewer.RawDefineCollect.TryGetValue(key, out oldDef))
                {
                    oldDef.Merge(def);
                }
                else
                {
                    Channel.Viewer.RawDefineViewer.RawDefineCollect.Add(key, def);
                }
            }
        }

        static Channel.Viewer.RawDefineViewer rawdefViewerInstance = new Channel.Viewer.RawDefineViewer();
        /// <summary>
        /// 原生定义查看器
        /// </summary>
        internal static Channel.Viewer.RawDefineViewer RawDefine
        {
            get
            {
                return rawdefViewerInstance;
            }
        }
    }
}
