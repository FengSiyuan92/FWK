using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Define
{

    public class ObjectDefine
    {
        public string Name { get; private set; }


        public ObjectDefine(string name)
        {
            Name = name;

        }


        Dictionary<string, Field> fields = new Dictionary<string, Field>();

        public void AddField(Field field)
        {
            fields.Add(field.FieldName, field);
        }
    }

}
