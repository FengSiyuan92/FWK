using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Define.Converter
{

    internal class FloatConverter : Converter
    {
        public override Type GetResultType()
        {
            return typeof(float);
        }

        public override object Convert(string originalValue, string defaultValue, params object[] pms)
        {
            float _;
            if (float.TryParse(originalValue, out _))
            {
                return _;
            }
            if (float.TryParse(defaultValue, out _))
            {
                return _;
            }
            return 0f;
        }

    }

}
