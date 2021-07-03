using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define.Class;

namespace Channel.Define.Converter
{
    internal class EnumConverter : Converter
    {
        public override Type GetResultType()
        {
            return typeof(int);
        }

        public override object Convert(string originalValue, string defaultValue, params object[] pms)
        {
            var value = string.IsNullOrEmpty(originalValue) ? defaultValue : originalValue;
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            int v = 0;
            if (int.TryParse(value, out v))
            {
                return v;
            }
            var isAlias = pms != null && pms.Length > 0 && (bool)pms[0];
            var res= isAlias ? e.GetValueByAlias(value) : e.GetValueByFieldName(value);
            if (res == -1)
            {
                CLog.LogError("枚举转换失败:{0}=>{1}", e.Name, v);
            }
            return res;
        }

        Channel.Define.Class.Enum e = null;

        public EnumConverter(string enumName)
        {
            e = Lookup.Enum[enumName];
            if (e == null)
            {
                CLog.LogError("没有定义类型为{0}的枚举", enumName);
            }
        }
    }

}
