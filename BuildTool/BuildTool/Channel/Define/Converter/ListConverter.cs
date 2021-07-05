using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Define.Converter
{
    internal class ListConverter : Converter
    {

        Converter element;
        Array defArray;
        Type arrType;
        char defSep;
        public ListConverter( Converter elementConvert, char defSep = ',')
        {
            element = elementConvert;
            defArray = Array.CreateInstance(element.GetResultType(), 0);
            arrType = defArray.GetType();
            this.defSep = defSep;
        }

        public override Type GetResultType()
        {
            return arrType;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalValue"></param>
        /// <param name="defaultValue"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public override object Convert(string originalValue, Field template, int depth = 0)
        {
            if (string.IsNullOrEmpty(originalValue))
            {
                if (template.OriginalDefaultValue.Equals(ConstString.STR_NIL, StringComparison.OrdinalIgnoreCase)||
                    template.OriginalDefaultValue.Equals(ConstString.STR_NULL, StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
                return _ConvertArray(template.OriginalDefaultValue, template, depth);
            }

            return _ConvertArray(originalValue, template, depth);
        }

        object _ConvertArray(string value, Field template, int depth)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defArray;
            }

            var sep = defSep;
            if (template.Seps != null)
            {
                sep = template.Seps[depth];
            }
            var slice = value.Split(sep);

            var res = Array.CreateInstance(element.GetResultType(), slice.Length);
   
            for (int i = 0; i < res.Length; i++)
            {
                res.SetValue(element.Convert(slice[i], template, depth + 1), i);
            }

            return res;
        }

    }
}
