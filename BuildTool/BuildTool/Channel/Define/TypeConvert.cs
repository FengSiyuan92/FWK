using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Define
{
    public class TypeConvert
    {
        public virtual int ToIntValue(string value)
        {
            return int.Parse(value);
        }

        public virtual float ToFloatValue(string value)
        {
            return float.Parse(value);
        }

        public virtual string ToStringValue(string value)
        {
            return value.ToString();
        }

        public virtual T ToCustomObjValue<T>(string value)
        {
            return default(T);
        }
    }

}
