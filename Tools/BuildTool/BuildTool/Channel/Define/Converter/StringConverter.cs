using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define;

namespace Channel.Define.Converter
{
    internal class StringConverter : Converter
    {
        public override Type GetResultType()
        {
            return typeof(string);
        }

        public override object Convert(DataObject original, string originalValue, Field template, int depth = 0)
        {
            // 原值
            if (originalValue != ConstString.STR_EMPTY)
            {
                return originalValue;
            }
            // 最上层的默认值
            else if(depth ==0)
            {
                if (template.OriginalDefaultValue.Equals(ConstString.STR_NIL, StringComparison.OrdinalIgnoreCase) ||
                    template.OriginalDefaultValue.Equals(ConstString.STR_NULL, StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                return template.OriginalDefaultValue;
            }
            else
            {
                return string.Empty;
            }
        }

        internal override int SepLevel()
        {
            return 2;
        }
    }

}
