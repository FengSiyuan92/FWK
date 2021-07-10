using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define.Converter;
using Channel.Define;

namespace Channel
{
    class InnerCheck_ConvertValid
    {
        const string enumerrortip = "枚举类型{0}不存在,对应类型的数据将被忽略,无法解析与导出=>{1}";
        const string customerrortip = "类型{0}不存在,对应类型的数据将被忽略,无法解析与导出=>{1}";
        /// <summary>
        /// 检查扩展的类型转换器是否可用
        /// </summary>
        public static void CheckConverterValid(List<Rule.RuleInfo> infos)
        {

            // 深度遍历所有customType的字段转换器,如果转换器不存在则标记不可用并提示
            var customs = Lookup.ClassInfo.AllName();
            Utils.Parallel(customs, CheckCustomConvertValid);

        }

        static void CheckCustomConvertValid(string name)
        {
            var t = Lookup.ClassInfo[name];
            foreach (var item in t.fields)
            {
                var field = item.Value;
                var convert = field.Convert;
                convert.Valid = CheckConverter(convert, field);
            }
        }

        static bool CheckConverter(Converter convert, Field field)
        {
            if (convert is EnumConverter)
            {
                var e = convert as EnumConverter;
                if (Lookup.Enum[e.Name] == null)
                {
                    CLog.LogError(enumerrortip, e.Name, field.Source());
                    return false;
                }

            }
            else if (convert is DataObjectConverter)
            {
                var c = convert as DataObjectConverter;
                if (Lookup.ClassInfo[c.Name] == null)
                {
                    CLog.LogError(customerrortip, c.Name, field.Source());
                    return false;
                }
            }
            else if (convert is DataArrayConverter)
            {
                var l = convert as DataArrayConverter;
                var sub = l.element;
                return CheckConverter(sub, field);
            }
            return true;
        }

    }
}
