using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define.Class;

namespace Channel.Define.Converter
{
    class Vector2Converter : Converter
    {
        public override Type GetResultType()
        {
            return typeof(Vector2);
        }

        static Vector2 Parse(Data.DataObject original, string content, Field template, int depth = 0)
        {
            var sep = Utils.GetCustomSep(template, depth);
            var slice = Utils.Split(content, sep);
            var xs = slice.Count > 0 ? slice[0].Trim() : ConstString.STR_EMPTY;
            var ys = slice.Count > 1 ? slice[1].Trim() : ConstString.STR_EMPTY;
            var source = string.Format("{0}.{1}", original.Source(), template.FieldName);
            return new Vector2(
                Utils.ParseFloat(xs, source),
                Utils.ParseFloat(ys, source)
               );
        }

        public override object Convert(Data.DataObject original, string originalValue, Field template, int depth = 0)
        {
            Vector2 vec = null;
            if (!string.IsNullOrEmpty(originalValue))
            {
                vec = Parse(original, originalValue, template, depth);
            }
            else if (depth == 0 && !string.IsNullOrEmpty(template.OriginalDefaultValue))
            {
                if (template.OriginalDefaultValue.Equals(ConstString.STR_NIL)||
                    template.OriginalDefaultValue.Equals(ConstString.STR_NULL))
                {
                    return null;
                }
                vec = Parse(original,template.OriginalDefaultValue, template, depth);
            }
            else
            {
                vec = new Vector2();
            }

            return vec;
        }

        internal override int SepLevel()
        {
            return 1;
        }
    }

    class Vector3Converter : Converter
    {
        internal override int SepLevel()
        {
            return 1;
        }
        public override Type GetResultType()
        {
            return typeof(Vector3);
        }

        static Vector3 Parse(Data.DataObject original, string content, Field template, int depth = 0)
        {
            var sep = Utils.GetCustomSep(template, depth);
            var slice = Utils.Split(content, sep);

            var xs = slice.Count > 0 ? slice[0].Trim() : ConstString.STR_EMPTY;
            var ys = slice.Count > 1 ? slice[1].Trim() : ConstString.STR_EMPTY;
            var zs = slice.Count > 2 ? slice[2].Trim() : ConstString.STR_EMPTY;
            var source = string.Format("{0}.{1}", original.Source(), template.FieldName);
            return new Vector3(
                Utils.ParseFloat(xs, source),
                Utils.ParseFloat(ys, source),
                Utils.ParseFloat(zs, source)
               );
        }

        public override object Convert(Data.DataObject original, string originalValue, Field template, int depth = 0)
        {
            Vector3 vec = null;
            if (!string.IsNullOrEmpty(originalValue))
            {
                vec = Parse(original, originalValue, template, depth);
            }
            else if (depth == 0 && !string.IsNullOrEmpty(template.OriginalDefaultValue))
            {
                if (template.OriginalDefaultValue.Equals(ConstString.STR_NIL) ||
                    template.OriginalDefaultValue.Equals(ConstString.STR_NULL))
                {
                    return null;
                }
                vec = Parse(original, template.OriginalDefaultValue, template, depth);
            }
            else
            {
                vec = new Vector3();
            }
            return vec;
        }
    }


    class Vector4Converter : Converter
    {
        internal override int SepLevel()
        {
            return 1;
        }
        public override Type GetResultType()
        {
            return typeof(Vector4);
        }

        static Vector4 Parse(Data.DataObject original, string content, Field template, int depth = 0)
        {
            var sep = Utils.GetCustomSep(template, depth);
            var slice = Utils.Split(content, sep);

            var xs = slice.Count > 0 ? slice[0].Trim() : ConstString.STR_EMPTY;
            var ys = slice.Count > 1 ? slice[1].Trim() : ConstString.STR_EMPTY;
            var zs = slice.Count > 2 ? slice[2].Trim() : ConstString.STR_EMPTY;
            var ws = slice.Count > 3 ? slice[3].Trim() : ConstString.STR_EMPTY;

            var source = string.Format("{0}.{1}", original.Source(), template.FieldName);
            return new Vector4(
                Utils.ParseFloat(xs, source),
                Utils.ParseFloat(ys, source),
                Utils.ParseFloat(zs, source),
                Utils.ParseFloat(ws, source)
               ); ;
        }

        public override object Convert(Data.DataObject original,string originalValue, Field template, int depth = 0)
        {
            Vector4 vec = null;
            if (!string.IsNullOrEmpty(originalValue))
            {
                vec = Parse(original, originalValue, template, depth);
            }
            else if (depth == 0 && !string.IsNullOrEmpty(template.OriginalDefaultValue))
            {
                if (template.OriginalDefaultValue.Equals(ConstString.STR_NIL) ||
                 template.OriginalDefaultValue.Equals(ConstString.STR_NULL))
                {
                    return null;
                }
                vec = Parse(original, template.OriginalDefaultValue, template, depth);
            }
            else
            {
                vec = new Vector4();
            }
            return vec;
        }
    }

}
