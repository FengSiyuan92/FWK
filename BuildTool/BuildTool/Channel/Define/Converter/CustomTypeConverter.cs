using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define.Class;

namespace Channel.Define.Converter
{
    internal class CustomTypeConverter : Converter
    {
        internal static List<CustomTypeConverter> delayComileType = new List<CustomTypeConverter>();

        public override Type GetResultType()
        {
            return typeof(CustomType);
        }

        internal string Name;
        public CustomTypeConverter(string name)
        {
            delayComileType.Add(this);
            this.Name = name;
        }
    }
}
