using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.OutputDefine
{
    class EnumFieldDefine : IFieldDefine
    {
        public string FieldName { get; set; }
        public string alias;
        public int value;
    }
}
