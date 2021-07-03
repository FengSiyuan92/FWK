using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Define.Class
{
    public class CustomType
    {
        public string Name { get; private set; }

        public CustomType(string name)
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
