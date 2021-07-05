using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Define.Converter
{
    internal class StringConverter : Converter
    {
        public override Type GetResultType()
        {
            return typeof(string);
        }

        public override object Convert(string originalValue, string defaultValue, params object[] pms)
        {
            if (string.IsNullOrEmpty(originalValue))
            {
                if (originalValue.Equals(ConstString.STR_NIL, StringComparison.OrdinalIgnoreCase) ||
                    originalValue.Equals(ConstString.STR_NULL, StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                return defaultValue;
            }

            return originalValue;
        }


    }

}
