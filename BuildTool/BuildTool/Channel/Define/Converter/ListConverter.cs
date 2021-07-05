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
        public ListConverter( Converter elementConvert)
        {
            element = elementConvert;
            defArray = Array.CreateInstance(element.GetResultType(), 0);
            arrType = defArray.GetType();
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
        public override object Convert(string originalValue, string defaultValue, params object[] pms)
        {


            if (string.IsNullOrEmpty(originalValue))
            {
                if (defaultValue.Equals(ConstString.STR_NIL, StringComparison.OrdinalIgnoreCase)||
                    defaultValue.Equals(ConstString.STR_NULL, StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
                return _ConvertArray(defaultValue, pms);
            }

            return _ConvertArray(originalValue, pms);
        }

        object _ConvertArray(string value, params object[] pms)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defArray;
            }
            var apointSep = pms != null && pms.Length > 0 ? pms[0] : null;

            var sep = apointSep as char?;
            var aSep = sep == null ? element.ElementSplit() : (char)sep;
            var slice = value.Split(aSep);

            var res = Array.CreateInstance(element.GetResultType(), slice.Length);
            object[] cutPms = null;
            if (pms!= null && pms.Length > 1)
            {
                cutPms = new object[pms.Length - 1];
                for (int i = 0; i < pms.Length; i++)
                {
                    cutPms[i - 1] = pms[i];
                }
            }

            for (int i = 0; i < res.Length; i++)
            {
                res.SetValue(element.Convert(slice[i], null, cutPms), i);
            }

            return res;
        }

    }
}
