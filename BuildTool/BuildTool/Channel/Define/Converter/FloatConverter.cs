using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define;

namespace Channel.Define.Converter
{

    internal class FloatConverter : Converter
    {
        public override Type GetResultType()
        {
            return typeof(decimal);
        }

        public override object Convert(DataObject original, string originalValue, Field template, int depth = 0)
        {
            decimal _;
            if (decimal.TryParse(originalValue, out _))
            {
                return _;
            }
            if (depth == 0)
            {
                if (decimal.TryParse(template.OriginalDefaultValue, out _))
                {
                    return _;
                }
            }
            return default(decimal);
        }

        internal override int SepLevel()
        {
            return 0;
        }

    }

}
