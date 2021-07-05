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

        Field KeyField;

        public void AddField(Field field)
        {
            if (fields.ContainsKey(field.FieldName))
            {
                CLog.LogError("{0}出现重复的字段名:{1}", Name, field.FieldName);
                return;
            }

            if (field.IsKey)
            {
                if (KeyField != null)
                {
                    CLog.LogError("不支持多个字段同时作为数据表的key,存在多个key配置的表名为:{0}", Name);
                    return;
                }
                KeyField = field;
            }
            fields.Add(field.FieldName, field);
        }


        public Field this[string fieldName]
        {
            get
            {
                Field field = null;
                fields.TryGetValue(fieldName, out field);
                return field;
            }
        }
    }

}
