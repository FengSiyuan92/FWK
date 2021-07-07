using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define.Class;

namespace Channel.Define.Converter
{
    internal class CustomTypeConverter : ExtendConverter , ISource
    {
        public override Type GetResultType()
        {
            return typeof(Data.DataObject);
        }

        public string Name { get; private set; }
        public CustomTypeConverter(string name):base()
        {
            this.Name = name;
        }

        Data.DataObject GenObj(Data.DataObject original, string content, Field template, int depth)
        {
            var sep = Utils.GetCustomSep(template, depth, ConstString.SEP_LEVEL_3);
            var t = Lookup.CustomType[Name];
            if (t== null)
            {
                return null;
            }

            var allFieldNames = t.AllFieldName();
            var slice = Utils.Split(content, sep);;
            Dictionary<string, string> kv = new Dictionary<string, string>();
            foreach (var fieldName in allFieldNames)
            {
                var field = t[fieldName];
                var targetIndex = field.FieldIndex;
                var v = slice.Count > targetIndex ? Utils.TrimSign(slice[targetIndex]) : ConstString.STR_EMPTY;
                kv.Add(fieldName, v);
            }
            var obj = new Data.DataObject(Name);
            obj.SourceInfo = original.Source();
            obj.SetKV(kv);
            return obj;
        }

        internal override int SepLevel()
        {
            var t = Lookup.CustomType[Name];
            var max = 0;
            foreach (var item in t.AllFieldName())
            {
                var c = t[item].Convert;
                var o = c.SepLevel();
                if (o > max)
                {
                    max = o;
                }
            }
            return max + 1;
        }

        public override object Convert(Data.DataObject original, string originalValue, Field template, int depth = 0)
        {
            Data.DataObject obj = null;
            if (!string.IsNullOrEmpty(originalValue))
            {
                obj = GenObj(original, originalValue, template, depth);
            }
            else if(depth == 0)
            {
                if (template.OriginalDefaultValue.Equals(ConstString.STR_NIL, StringComparison.OrdinalIgnoreCase) ||
                    template.OriginalDefaultValue.Equals(ConstString.STR_NULL, StringComparison.OrdinalIgnoreCase)||
                    string.IsNullOrEmpty(template.OriginalDefaultValue))
                {
                    return null;
                }
                obj = GenObj(original, template.OriginalDefaultValue, template, depth);
            }

            return obj;
        }

        internal string SourceInfo;

        public string Source()
        {
            return SourceInfo;
        }
    }
}
