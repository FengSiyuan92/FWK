using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Define.Converter
{

    internal class IntConverter : Converter
    {
        public override Type GetResultType()
        {
            return typeof(long);
        }

        public override object Convert(string originalValue, Field template, int depth = 0)
        {
            long _;
            if (long.TryParse(originalValue, out _))
            {
                return _;
            }
            if (depth == 0)
            {
                if (long.TryParse(template.OriginalDefaultValue, out _))
                {
                    return _;
                }
            }
            
            return 0L;
        }
        internal override int SepLevel()
        {
            return 0;
        }

    }

}
