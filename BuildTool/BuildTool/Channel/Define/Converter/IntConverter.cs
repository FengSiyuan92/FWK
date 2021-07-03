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
            return typeof(int);
        }

        public override object Convert(string originalValue, string defaultValue, params object[] pms)
        {
            int _;
            if (int.TryParse(originalValue, out _))
            {
                return _;
            }
            if (int.TryParse(defaultValue, out _))
            {
                return _;
            }
            return 0;
        }

    }

}
