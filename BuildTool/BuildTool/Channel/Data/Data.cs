using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define.Class;

namespace Channel.Data
{
    public class DataObject
    { 
        public string ClassName { get; private set; }
        public string KeyToString { get; private set;}

        // 内容存储器
        Hashtable ori;

        CustomType t;

        internal DataObject(string className)
        {
            ClassName = className;
            ori = new Hashtable();
            t = Lookup.CustomType[className];
        }

        internal void SetKV(Dictionary<string, string> originalKV)
        {
            foreach (var item in originalKV)
            {
                var fieldName = item.Key;
                var original = item.Value;
                var fieldType = t[fieldName];
                var res = fieldType.Convert.Convert(item.Value, fieldType);
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


        public override string ToString()
        {
            var res = "";
            foreach (var item in ori)
            {
                res += item.ToString() + " ";
            }
            return res;
        }


    }
}
