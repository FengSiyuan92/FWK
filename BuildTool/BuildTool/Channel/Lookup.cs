using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.OutputDefine;

namespace Channel
{
    public class Lookup
    {
        public static void Test()
        {
            var a = 0;
        }

        static Dictionary<string, ObjectDefine> ObjCollect = new Dictionary<string, ObjectDefine>();

        internal static void AddObjectDefine(ObjectDefine def)
        {
            var key = def.Name;
            ObjectDefine oldDef = null;
            lock (ObjCollect)
            {
                if (ObjCollect.TryGetValue(key, out oldDef))
                {
                    oldDef.Merge(def);
                }
                else
                {
                    ObjCollect.Add(key, def);
                }
            }
        }


        static Dictionary<string, EnumDefine> EnumCollect = new Dictionary<string, EnumDefine>();

        internal static void AddEnumDefine(EnumDefine def)
        {
            lock (EnumCollect)
            {
                EnumCollect.Add(def.Name, def);
            }

        }
    }
}
