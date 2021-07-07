using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define.Class;
namespace Channel.Define.Converter
{
    internal class ListConverter : Converter
    {

        Converter element;
        DataArray defArray;
        Type arrType;
        char defSep;
        public ListConverter( Converter elementConvert, char defSep = ConstString.SEP_LEVEL_2)
        {
            element = elementConvert;
            defArray = DataArray.CreateInstance(element.GetResultType(), 0);
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
        public override object Convert(Data.DataObject original, string originalValue, Field template, int depth = 0)
        {
            if (string.IsNullOrEmpty(originalValue))
            {
                if (template.OriginalDefaultValue.Equals(ConstString.STR_NIL, StringComparison.OrdinalIgnoreCase)||
                    template.OriginalDefaultValue.Equals(ConstString.STR_NULL, StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
                return _ConvertArray(original, template.OriginalDefaultValue, template, depth);
            }

            return _ConvertArray(original, originalValue, template, depth);
        }

        object _ConvertArray(Data.DataObject original, string value, Field template, int depth)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defArray;
            }

            var sep = Utils.GetCustomSep(template, depth, defSep);
          
            var slice = Utils.Split(value, sep);

            var res = DataArray.CreateInstance(element.GetResultType(), slice.Count);
   
            for (int i = 0; i < res.Length; i++)
            {
                var subValue = Utils.TrimSign(slice[i]);
                res.SetValue(element.Convert(original, subValue, template, depth + 1), i);
            }

            return res;
        }
        internal override int SepLevel()
        {
            return 1 + element.SepLevel();
        }

    }
}
