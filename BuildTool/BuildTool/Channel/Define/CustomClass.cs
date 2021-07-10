using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Define
{
    public class CustomClass : ISource
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; private set; }

        internal RawDefine.RawObjDef RawDefine;
        public CustomClass(string name)
        {
            Name = name;
            Lookup.AddObjDefine(this);
        }

        internal Dictionary<string, Field> fields { get; private set; } = new Dictionary<string, Field>();
        internal List<string> sortedFieldNams = new List<string>();
        /// <summary>
        /// 需要作为key的字段信息
        /// </summary>
        public Field KeyField { get; private set; }
        /// <summary>
        /// 需要作为key的字段名称
        /// </summary>
        public string KeyFieldName { get; private set; }

        object fieldKeysLock = new object();
     
        internal void AddField(Field field)
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
                KeyFieldName = field.FieldName;
            }

            fields.Add(field.FieldName, field);
            sortedFieldNams.Add(field.FieldName);
        }

   
        public Field this[string fieldName]=> GetFieldInfoByFieldName(fieldName);

        /// <summary>
        /// 通过字段名称索引该类型下的字段信息,可以使用索引器替换该方法
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Field GetFieldInfoByFieldName(string fieldName)
        {
            Field field = null;
            fields.TryGetValue(fieldName, out field);
            return field;
        }

        /// <summary>
        /// 获取所有字段的名称
        /// </summary>
        /// <returns></returns>
        public string[] AllFieldName()
        {
            return sortedFieldNams.ToArray();
        }

        public override bool Equals(object obj)
        {
            var target = obj as CustomClass;
            if (target == null) return false;

            if (!Name.Equals(target.Name, StringComparison.OrdinalIgnoreCase)) return false;

            // 同类型字段名称必然相等
            return true;
        }

        public string Source()
        {
            return RawDefine.Source() ;
        }
    }

}
