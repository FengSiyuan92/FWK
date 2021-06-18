using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.OutputDefine;

namespace Channel.Data
{
    public class ObjectData
    {
        // 所属table
        public Table owner;

        internal class Field
        {
            public FieldDefine fieldDefine;
            public string originalValue;

            public override string ToString()
            {
                return string.Format("({0})={1}", fieldDefine.fieldType, originalValue);
            }
        }

        public string Key;
        Dictionary<string, Field> fields = new Dictionary<string, Field>();

        internal void AddField(Field field)
        {
            fields.Add(field.fieldDefine.FieldName, field);
            var keyIndex = field.fieldDefine.IsKey;
            switch (keyIndex)
            {
                case 1:
                    Key = field.originalValue;
                    break;
                //case 2:
                //    Key2 = field.originalValue;
                    //break;
                default:
                    break;
            }
        }
    }
}
