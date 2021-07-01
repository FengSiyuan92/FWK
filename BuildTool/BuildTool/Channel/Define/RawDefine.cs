using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.RawDefine
{

    internal enum RawObjType
    {
        ERROR = 0,
        ENUM,
        OBJECT,
    }


    internal static class RawFieldType
    {
        public const string Int = "int";

    }


    internal class RawObjDef
    {
        public string Name { get; set; }

        public string ObjType { get; set; }

        public RawObjType DefType { get; set; }

        Dictionary<string, RawFieldDef> defines = new Dictionary<string, RawFieldDef>();

        public RawObjDef(string name, RawObjType type)
        {
            this.Name = name;
            this.DefType = type;
        }

        public virtual void Merge(RawObjDef otherDef)
        {
  
            if (otherDef == null) return;

            foreach (var define in otherDef.defines)
            {
                RawFieldDef oldDefine = null;
                if (!defines.TryGetValue(define.Key, out oldDefine))
                {
                    defines.Add(define.Key, define.Value);
                }
            }
        }

        public virtual void AddFieldDefine(RawFieldDef def)
        {
            if (def == null) return;

            var key = def.FieldName;
            if (defines.ContainsKey(key))
            {
                CLog.LogError("重复添加相同的字段名");
            }

            defines.Add(key, def);
        }

        public RawFieldDef this[string k]
        {
            get
            {
                RawFieldDef res = null;
                defines.TryGetValue(k, out res);
                return res;
            }
        }
    }

    internal class RawFieldDef
    {
        public string FieldName { get; set; }

        public string FieldType { get; set; }

        public string OutputType { get; set; }

        public string AppendDef { get; set; }

        public string CheckRule { get; set; }

        public string DefaultValue { get; set; }
    }
}
