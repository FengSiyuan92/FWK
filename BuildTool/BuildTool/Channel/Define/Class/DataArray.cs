using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
namespace Channel.Define.Class
{
    public class DataArray
    {
        Array array;
        Type t;
        public static DataArray CreateInstance(Type t, int length)
        {
            var classArray = new DataArray();
            classArray.array = Array.CreateInstance(t, length);
            classArray.t = t;
            return classArray;
        }

        public object GetValue(int index)
        {
            return array.GetValue(index);
        }

        public void SetValue(object value, int index)
        {
            array.SetValue(value, index);
        }

        public object this[int index]
        {
            get
            {
                return array.GetValue(index);
            }
            set
            {
                array.SetValue(value, index);
            }
        }

        public int Length => array.Length;


        public override bool Equals(object obj)
        {
            var target = obj as DataArray;
            if (target == null) return false;

            if (Length != target.Length)
            {
                return false;
            }

            for (int i = 0; i < Length; i++)
            {
                var av = GetValue(i);
                var bv = target.GetValue(i);
                if (av == null && bv != null || bv == null && av!= null || !av.Equals(bv))
                {
                    return false;
                }
            }
            return true;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(t.ToString());
            sb.Append("[");
            sb.Append(Length.ToString());
            sb.Append("]={");

            for (int i = 0; i < Length -1; i++)
            {
                sb.Append(array.GetValue(i).ToString());
                sb.Append(",");
            }
            if (Length != 0)
            {
                sb.Append(array.GetValue(Length - 1).ToString());
            }
           
            sb.Append('}');

            return sb.ToString();
        }
    }
}
