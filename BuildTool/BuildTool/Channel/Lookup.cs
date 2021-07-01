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

        internal static RawObjDef LookupRawObjDef(string objTypeName)
        {
            RawObjDef def = null;
            RawDefineCollect.TryGetValue(objTypeName, out def);
            return def;
        }

        internal static List<string> LookAllRawDefineName()
        {
            return RawDefineCollect.Keys.ToList<string>();
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

        public static Channel.Data.Table LookTable(string tableName)
        {
            Channel.Data.Table table;
            TableCollect.TryGetValue(tableName, out table);
            return table;
        }
    }
}
