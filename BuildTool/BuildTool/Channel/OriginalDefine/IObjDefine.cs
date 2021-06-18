using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.OutputDefine
{
     interface IObjDefine
    {
        string Name { get; set; }

        void AddFieldDefine(IFieldDefine fieldDef);

        IFieldDefine this[string key, bool alias = false] { get; }

        void ParseDefine();
    }

     interface IFieldDefine
    {
        string FieldName { get; set; }
    }
}
