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

        public override object Convert(string originalValue, string defaultValue, params object[] pms)
        {
            var v = string.IsNullOrEmpty(originalValue) ? defaultValue : originalValue;
            Vector2 vec = null;
            if (string.IsNullOrEmpty(v))
            {
                vec = new Vector2();
            }
            else
            {
                var sp = v.Split(',');
                var x = sp.Length > 0 ? float.Parse(sp[0]) : 0;
                var y = sp.Length > 1 ? float.Parse(sp[1]) : 0;
                vec = new Vector2(x, y);
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

        public override object Convert(string originalValue, string defaultValue, params object[] pms)
        {
            var v = string.IsNullOrEmpty(originalValue) ? defaultValue : originalValue;
            Vector3 vec = null;
            if (string.IsNullOrEmpty(v))
            {
                vec = new Vector3();
            }
            else
            {
                var sp = v.Split(',');
                var x = sp.Length > 0 ? float.Parse(sp[0]) : 0;
                var y = sp.Length > 1 ? float.Parse(sp[1]) : 0;
                var z = sp.Length > 2 ? float.Parse(sp[2]) : 0;
                vec = new Vector3(x, y, z);
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

        public override object Convert(string originalValue, string defaultValue, params object[] pms)
        {
            var v = string.IsNullOrEmpty(originalValue) ? defaultValue : originalValue;
            Vector4 vec = null;
            if (string.IsNullOrEmpty(v))
            {
                vec = new Vector4();
            }
            else
            {
                var sp = v.Split(',');
                var x = sp.Length > 0 ? float.Parse(sp[0]) : 0;
                var y = sp.Length > 1 ? float.Parse(sp[1]) : 0;
                var z = sp.Length > 2 ? float.Parse(sp[2]) : 0;
                var w = sp.Length > 3 ? float.Parse(sp[3]) : 0;
                vec = new Vector4(x, y, z);
            }
            return vec;
        }
    }
}
