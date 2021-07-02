using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Define.CompileType
{
    public abstract class CompileType
    {

        public virtual object Convert(string originalValue, string defaultValue)
        {
            return null;
        }


        public virtual object Convert<T>(string originalValue, string defaultValue, T p)
        {
            return null;
        }

        public virtual object Convert<T,K>(string originalValue, string defaultValue, T p1, K p2)
        {
            return null;
        }


        public abstract System.Type CType();

    }

    public class IntType : CompileType
    {
        public override Type CType()
        {
            return typeof(int);
        }

        public override object Convert(string originalValue, string defaultValue)
        {
            int _;
            if (int.TryParse(originalValue, out _))
            {
                return _;
            }
            if (int.TryParse(defaultValue, out _))
            {
                return _;
            }
            return 0;
        }


        public override object Convert<T>(string originalValue, string defaultValue, T p)
        {
            return Convert(originalValue, defaultValue);
        }

        public override object Convert<T, K>(string originalValue, string defaultValue, T p1, K p2)
        {
            return Convert(originalValue, defaultValue);
        }
    }

    public class FloatType : CompileType
    {
        public override Type CType()
        {
            return typeof(float);
        }

        public override object Convert(string originalValue, string defaultValue)
        {
            float _;
            if (float.TryParse(originalValue, out _))
            {
                return _;
            }
            if (float.TryParse(defaultValue, out _))
            {
                return _;
            }
            return 0f;
        }


        public override object Convert<T>(string originalValue, string defaultValue, T p)
        {
            return Convert(originalValue, defaultValue);
        }

        public override object Convert<T, K>(string originalValue, string defaultValue, T p1, K p2)
        {
            return Convert(originalValue, defaultValue);
        }
    }

    public class StringType : CompileType
    {
        public override Type CType()
        {
            return typeof(string);
        }

        public override object Convert(string originalValue, string defaultValue)
        {
            return string.IsNullOrEmpty(originalValue) ? defaultValue : originalValue;
        }


        public override object Convert<T>(string originalValue, string defaultValue, T p)
        {
            return Convert(originalValue, defaultValue);
        }

        public override object Convert<T, K>(string originalValue, string defaultValue, T p1, K p2)
        {
            return Convert(originalValue, defaultValue);
        }

    }

    public class ListType : CompileType
    {
        CompileType elementType;
        Type t;
        string defAsep = ",";
        Array array;
        public ListType(CompileType elementType)
        {
            this.elementType = elementType;
            array = Array.CreateInstance(elementType.CType(), 0);
            t = array.GetType();
            defAsep = ",";
            if (elementType is StringType)
            {
                defAsep = "|";
            }
        }

        public override Type CType()
        {
            return t;
        }

        public override object Convert(string originalValue, string defaultValue)
        {
            return Convert(originalValue, defaultValue, null, null);
        }


        public override object Convert<T>(string originalValue, string defaultValue, T p)
        {
            
        }

        public override object Convert<T, K>(string originalValue, string defaultValue, T p1, K p2)
        {
            if (string.IsNullOrEmpty(originalValue))
            {
                if (string.IsNullOrEmpty(defaultValue))
                {

                }
            }

            var asep = p as string;
            asep = asep == null ? defAsep : asep;
            asep = string.IsNullOrEmpty(asep) ? defAsep : asep;

        }
    }


}
