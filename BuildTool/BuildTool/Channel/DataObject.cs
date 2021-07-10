using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define;

namespace Channel
{
    public class DataObject : ISource
    { 
        /// <summary>
        /// 类型名
        /// </summary>
        public string ClassName { get; private set; }

        /// <summary>
        /// 该对象的key值
        /// </summary>
        public object Key { get; private set; }

        // 内容存储器
        Hashtable ori;
        /// <summary>
        /// 该对象的类型信息
        /// </summary>
        public CustomClass ClassInfo { get; private set; }

        internal DataObject(string className, string sourceInfo)
        {
            ClassName = className;
            ori = new Hashtable();
            ClassInfo = Lookup.CustomType[className];
            SourceInfo = sourceInfo;
        }

        internal void SetKV(Dictionary<string, string> originalKV)
        {
            foreach (var item in originalKV)
            {
                var fieldName = item.Key;
                var original = item.Value;
                var fieldType = ClassInfo[fieldName];
                var convert = fieldType.Convert;
                if (!convert.Valid)
                {
                    continue;
                }
                var res = convert.Convert(this, item.Value, fieldType);
                ori.Add(fieldName, res);
                if (fieldType.IsKey)
                {
                    Key = res;
                }
            }
        }

        public object this[string fieldName]=> GetFieldValuefoByName(fieldName);
        /// <summary>
        /// 通过字段名获取字段信息
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Field GetFieldInfoByName(string fieldName)
        {
            return ClassInfo[fieldName];
        }

        /// <summary>
        /// 通过字段名获取字段值
        /// </summary>
        public object GetFieldValuefoByName(string fieldName)
        {
            return ori[fieldName];
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ClassName);
            sb.Append("[");

            var targetCount = ori.Count;
            var cCount = 0;
            foreach (var item in ori.Keys)
            {
                var value = ori[item];
                sb.Append(string.Format("{0}={1}", item, value == null ? "null" : value.ToString()));
                if (++cCount != targetCount)
                {
                    sb.Append(',');
                }
            }
            sb.Append("]");
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            var target = obj as DataObject;
            if (target == null) return false;

            if (ClassInfo != target.ClassInfo) return false;
            var allfieldName = ClassInfo.AllFieldName();

            for (int i = 0; i < allfieldName.Length; i++)
            {
                var fieldName = allfieldName[i];
                var v1 = this[fieldName];
                var v2 = target[fieldName];
                if (!v1.Equals(v2)) return false;
            }
            return true;
        }
        KeyValuePair<string, object>[] result;

        /// <summary>
        /// 获取所有以字段名排好序的[字段名,值]数组
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<string, object>[] GetAllSortedField()
        {
            if (result != null)
            {
                return result;
            }

            var fieldName = ClassInfo.AllFieldName();
            result = new KeyValuePair<string, object>[fieldName.Length];
            for (int i = 0; i < fieldName.Length; i++)
            {
                result[i] = new KeyValuePair<string, object>(fieldName[i], ori[fieldName[i]]);
            }
            return result;
        }

        string SourceInfo;
        public string Source()
        {
            return SourceInfo;
        }


    }
}
