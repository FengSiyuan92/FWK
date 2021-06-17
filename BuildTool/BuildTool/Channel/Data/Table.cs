using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.OutputDefine;

namespace Channel.Data
{
    class Table
    {
        public string Name;

        public ObjectDefine define;
        
        public Table(string name)
        {
            this.Name = name;
            this.define = Lookup.LookObjectDefine(name);
        }

        public void AddConent(string fieldName, string content)
        {

        }

        public void Merge(Table otherTable)
        {

        }
    }
}
