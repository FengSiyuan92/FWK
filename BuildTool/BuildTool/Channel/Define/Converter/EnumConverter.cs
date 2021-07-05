using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define.Class;

namespace Channel.Define.Converter
{
    internal class EnumConverter : ExtendConverter
    {
        public override Type GetResultType()
        {
            return typeof(int);
        }

        public override object Convert(string originalValue, Field template, int depth = 0)
        {
            if (!string.IsNullOrEmpty(originalValue))
            {
                var value = originalValue;
                int v = 0;
                if (int.TryParse(value, out v))
                {
                    return v;
                }
                var isAlias = !string.IsNullOrEmpty(template.AliasRefPos);
                var res = isAlias ? e.GetValueByAlias(value) : e.GetValueByFieldName(value);
                if (res == -1)
                {
                    CLog.LogError("枚举转换失败:{0}=>{1}", e.Name, v);
                }
            }
            else if  (depth != 0)
            {
                var value = template.OriginalDefaultValue;
                int v = 0;
                if (int.TryParse(value, out v))
                {
                    return v;
                }
                var isAlias = !string.IsNullOrEmpty(template.AliasRefPos);
                var res = isAlias ? e.GetValueByAlias(value) : e.GetValueByFieldName(value);
                if (res == -1)
                {
                    CLog.LogError("枚举转换失败:{0}=>{1}", e.Name, v);
                }
            }
            return 0;
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
