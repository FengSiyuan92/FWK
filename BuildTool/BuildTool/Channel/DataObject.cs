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
        public string ClassName { get; private set; }
        public string KeyToString { get; private set;}

        // 内容存储器
        Hashtable ori;

        public CustomClass t { get; private set; }

        internal DataObject(string className, string sourceInfo)
        {
            ClassName = className;
            ori = new Hashtable();
            t = Lookup.CustomType[className];
            SourceInfo = sourceInfo;
        }

        internal void SetKV(Dictionary<string, string> originalKV)
        {
            foreach (var item in originalKV)
            {
                var fieldName = item.Key;
                var original = item.Value;
                var fieldType = t[fieldName];
                var convert = fieldType.Convert;
                if (!convert.Valid)
                {
                    continue;
                }
                var res = convert.Convert(this, item.Value, fieldType);
                ori.Add(fieldName, res);
                if (fieldType.IsKey)
                {
                    KeyToString = res.ToString();
                }
            }
        }

        public object this[string fieldName]
        {
            get
            {
                return ori[fieldName];
            }
        }


        string SourceInfo;
        public string Source()
        {
            return SourceInfo;
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

            if (t != target.t) return false;
            var allfieldName = t.AllFieldName();

            for (int i = 0; i < allfieldName.Length; i++)
            {
                var fieldName = allfieldName[i];
                var v1 = this[fieldName];
                var v2 = target[fieldName];
                if (!v1.Equals(v2)) return false;
            }
            return true;
        }
    }
}
