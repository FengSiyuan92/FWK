using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Define.Class
{
    public class CustomType
    {
        public string Name { get; private set; }

        public CustomType(string name)
        {
            Name = name;
            Lookup.AddObjDefine(this);
        }

        Dictionary<string, Field> fields = new Dictionary<string, Field>();

        public void AddField(Field field)
        {
            if (fields.ContainsKey(field.FieldName))
            {
                CLog.LogError("{0}出现重复的字段名:{1}", Name, field.FieldName);
            }
            fields.Add(field.FieldName, field);
        }


    }

}
