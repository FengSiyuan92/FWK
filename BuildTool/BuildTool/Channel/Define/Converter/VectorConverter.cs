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

        static Vector2 Parse(string content, Field template, int depth = 0)
        {
            var sep = ',';
            if (template.Seps != null && template.Seps.Length > depth)
            {
                sep = template.Seps[depth];
            }
            var slice = content.Split(sep);

            var x = slice.Length > 0 ? float.Parse(slice[0]) : 0;
            var y = slice.Length > 1 ? float.Parse(slice[1]) : 0;
            return new Vector2(x, y);
        }

        public override object Convert(string originalValue, Field template, int depth = 0)
        {
            Vector2 vec = null;
            if (!string.IsNullOrEmpty(originalValue))
            {
                Parse(originalValue, template, depth);
            }
            else if (depth == 0 && !string.IsNullOrEmpty(template.OriginalDefaultValue))
            {
                Parse(template.OriginalDefaultValue, template, depth);
            }
            else
            {
                vec = new Vector2();
            }

            return vec;
        }
    }

    class Vector3Converter : Converter
    {
        public override Type GetResultType()
        {
            return typeof(Vector3);
        }

        static Vector3 Parse(string content, Field template, int depth = 0)
        {
            var sep = ',';
            if (template.Seps != null && template.Seps.Length > depth)
            {
                sep = template.Seps[depth];
            }
            var slice = content.Split(sep);

            var x = slice.Length > 0 ? float.Parse(slice[0]) : 0;
            var y = slice.Length > 1 ? float.Parse(slice[1]) : 0;
            var z = slice.Length > 2 ? float.Parse(slice[2]) : 0;
            return new Vector3(x, y, z);
        }

        public override object Convert(string originalValue, Field template, int depth = 0)
        {
            Vector3 vec = null;
            if (!string.IsNullOrEmpty(originalValue))
            {
                Parse(originalValue, template, depth);
            }
            else if (depth == 0 && !string.IsNullOrEmpty(template.OriginalDefaultValue))
            {
                Parse(template.OriginalDefaultValue, template, depth);
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
        public override Type GetResultType()
        {
            return typeof(Vector4);
        }

        static Vector4 Parse(string content, Field template, int depth = 0)
        {
            var sep = ',';
            if (template.Seps != null && template.Seps.Length > depth)
            {
                sep = template.Seps[depth];
            }
            var slice = content.Split(sep);

            var x = slice.Length > 0 ? float.Parse(slice[0]) : 0;
            var y = slice.Length > 1 ? float.Parse(slice[1]) : 0;
            var z = slice.Length > 2 ? float.Parse(slice[2]) : 0;
            var w = slice.Length > 3 ? float.Parse(slice[3]) : 0;
            return new Vector4(x, y, z, w);
        }

        public override object Convert(string originalValue, Field template, int depth = 0)
        {
            Vector4 vec = null;
            if (!string.IsNullOrEmpty(originalValue))
            {
                Parse(originalValue, template, depth);
            }
            else if (depth == 0 && !string.IsNullOrEmpty(template.OriginalDefaultValue))
            {
                Parse(template.OriginalDefaultValue, template, depth);
            }
            else
            {
                vec = new Vector4();
            }
            return vec;
        }
    }

}
