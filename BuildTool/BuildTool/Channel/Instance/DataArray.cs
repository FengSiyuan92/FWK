using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel
{
    public class DataArray
    {
        Array array;
        Type t;
        /// <summary>
        /// 创建一个对应type的数据
        /// </summary>
        /// <param name="t"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        internal static DataArray CreateInstance(Type t, int length)
        {
            var classArray = new DataArray();
            classArray.array = Array.CreateInstance(t, length);
            classArray.t = t;
            return classArray;
        }

        /// <summary>
        /// 元素类型
        /// </summary>
        public Type ElementType => t;


        /// <summary>
        /// 获取数组索引对应的值,可以使用索引器代替
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object GetValue(int index)
        {
            return array.GetValue(index);
        }

        /// <summary>
        /// 设置数组索引对应的值,可以使用索引器代替
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public void SetValue(object value, int index)
        {
            array.SetValue(value, index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 数组的长度,可能为0
        /// </summary>
        public int Length => array.Length;

        /// <summary>
        /// 两个数组里的内容是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
