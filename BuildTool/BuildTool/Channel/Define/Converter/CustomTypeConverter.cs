using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define.Class;

namespace Channel.Define.Converter
{
    internal class CustomTypeConverter : ExtendConverter
    {
        public override Type GetResultType()
        {
            return typeof(CustomType);
        }

        public string Name { get; private set; }
        public CustomTypeConverter(string name):base()
        {
            this.Name = name;
        }

        public override object Convert(string originalValue, Field template, int depth = 0)
        {
            throw new NotImplementedException();
        }
    }
}
