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

        internal static ObjectDefine LookObjectDefine(string objTypeName)
        {
            ObjectDefine def = null;
            ObjCollect.TryGetValue(objTypeName, out def);
            return def;
        }


        static Dictionary<string, EnumDefine> EnumCollect = new Dictionary<string, EnumDefine>();

        internal static void AddEnumDefine(EnumDefine def)
        {
            lock (EnumCollect)
            {
                EnumCollect.Add(def.Name, def);
            }
        }

        internal static EnumDefine LookEnumDefine(string enumTypeName)
        {
            EnumDefine def = null;
            EnumCollect.TryGetValue(enumTypeName, out def);
            return def;
        }


        static Dictionary<string, Channel.Data.Table> TableCollect = new Dictionary<string, Data.Table>();
        internal static void AddTable(Channel.Data.Table table)
        {
            lock (TableCollect)
            {
                Channel.Data.Table oldTab;
                if (TableCollect.TryGetValue(table.Name, out oldTab))
                {
                    oldTab.Merge(table);
                }
                else
                {
                    TableCollect.Add(table.Name, table);
                }
            }
        }
    }
}
