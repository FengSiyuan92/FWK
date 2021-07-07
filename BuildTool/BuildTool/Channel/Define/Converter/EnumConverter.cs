using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define.Class;

namespace Channel.Define.Converter
{
    internal class EnumConverter : Converter, ISource
    {
        public override Type GetResultType()
        {
            return typeof(int);
        }

        int TryGetValue(string name)
        {
            int v = 0;
            if (int.TryParse(name, out v))
            {
                return v;
            }
            v = e.GetValueByAlias(name);
            if (v != -1) return v;

            v = e.GetValueByFieldName(name);
            return v;
        }

        const string errortip = "把数据'{0}'转换成枚举类型'{1}'失败,将返回错误值-1 => {2}.{3}";
        public override object Convert(Data.DataObject original, string originalValue, Field template, int depth = 0)
        {
            if (!string.IsNullOrEmpty(originalValue))
            {
                var value = originalValue.Trim();
                var res = TryGetValue(value);
                if (res == -1)
                {
                    CLog.LogError(errortip, originalValue, e.Name, original.Source(), template.FieldName);
                }
                return res;
            }
            else if  (depth == 0)
            {
                var value = template.OriginalDefaultValue.Trim();
                var res = TryGetValue(value);
                if (res == -1 && 
                    !(value.Equals(ConstString.STR_NIL) || value.Equals(ConstString.STR_NULL)))
                {
                    CLog.LogError(errortip, value,e.Name, original.Source(), template.FieldName);
                }
                else
                {
                    return 0;
                }
                return res;
            }
            return 0;
        }

        internal override int SepLevel()
        {
            return 0;
        }

        internal string SourceInfo;
        public string Source()
        {
            return SourceInfo;
        }

        Channel.Define.Class.Enum e = null;
        public string Name { get; private set; }
        public EnumConverter(string enumName):base()
        {
            this.Name = enumName;
            e = Lookup.Enum[enumName];
        }
    }

}
