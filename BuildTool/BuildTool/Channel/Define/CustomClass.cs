﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Define
{
    public class CustomClass
    {
        public string Name { get; private set; }

        public CustomClass(string name)
        {
            Name = name;
            Lookup.AddObjDefine(this);
        }

        internal Dictionary<string, Field> fields { get; private set; } = new Dictionary<string, Field>();

        public Field KeyField { get; private set; }

        object fieldKeysLock = new object();
        string[] SortedFieldKeys;

        public string[] GetSortedFieldKeys()
        {
            lock (fieldKeysLock)
            {
                if (SortedFieldKeys == null)
                {
                    SortedFieldKeys = fields.Keys.ToArray();
                    Array.Sort(SortedFieldKeys, (a, b) => a.CompareTo(b));
                }
            }

            return SortedFieldKeys;
        }
       

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


        public string[] AllFieldName()
        {
            return fields.Keys.ToArray();
        }

        public override bool Equals(object obj)
        {
            var target = obj as CustomClass;
            if (target == null) return false;

            if (!Name.Equals(target.Name, StringComparison.OrdinalIgnoreCase)) return false;

            // 同类型字段名称必然相等

            return true;
        }
    }

}