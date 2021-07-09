using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define;

namespace Channel.Define.Converter
{
    internal abstract class Converter
    {

        public abstract object Convert(DataObject original, string originalValue, Field template, int depth = 0);

        public abstract System.Type GetResultType();

        /// <summary>
        ///  获取作为list子转换器时,默认的分隔符
        /// </summary>
        /// <returns></returns>
        public virtual char ElementSplit()
        {
            return ',';
        }

        internal abstract int SepLevel();

        public bool Valid { get; internal set; } = true;

    }

    //public class ListConverter : Converter
    //{
    //    Converter elementConverter;
    //    Type t;
    //    string defAsep = ",";
    //    Array array;

    //    public ListConverter(Converter eleConverter)
    //    {
    //        this.elementConverter = eleConverter;
    //        array = Array.CreateInstance(eleConverter.GetResultType(), 0);
    //        t = array.GetType();
    //        defAsep = ",";
    //        if (eleConverter is StringConverter)
    //        {
    //            defAsep = "|";
    //        }
    //    }

    //    public override Type GetResultType()
    //    {
    //        return t;
    //    }

    //    public override object Convert(string originalValue, string defaultValue)
    //    {
    //        return Convert(originalValue, defaultValue, ConstString.STR_EMPTY, ConstString.STR_EMPTY);
    //    }


    //    public override object Convert<T>(string originalValue, string defaultValue, T p)
    //    {
    //        return Convert(originalValue, defaultValue, p, ConstString.STR_EMPTY);
    //    }

    //    public override object Convert<T, K>(string originalValue, string defaultValue, T p1, K p2)
    //    {

    //        var asep = p1 as string;
    //        asep = asep == null ? defAsep : asep;
    //        asep = string.IsNullOrEmpty(asep) ? defAsep : asep;


    //        var sep = p2 as string;
    //        sep = sep == null ? defAsep : sep;
    //        sep = string.IsNullOrEmpty(sep) ? defAsep : sep;

    //        var v = string.IsNullOrEmpty(originalValue) ? defaultValue : originalValue;
    //        return Splite(v, asep, sep);


    //    }

    //    Array Splite(string value, string sep1, string sep2)
    //    {
    //        // a,b,c
    //        var splits = value.Split(new String[] { sep1}, StringSplitOptions.None);
    //        var array = Array.CreateInstance(elementConverter.GetResultType(), splits.Length);
    //        for (int i = 0; i < splits.Length; i++)
    //        {
    //            array.SetValue(elementConverter.Convert(splits[i], null, sep2), i);
    //        }
    //        return array;
    //    }
    //}

    //public class VectorListConverter : Converter
    //{
    //    int count = 0;
    //    Converter eleConverter;
    //    public VectorListConverter(int vectorCount)
    //    {
    //        count = vectorCount;
    //        eleConverter = new VectorConverter(vectorCount);
    //    }

    //    public override Type GetResultType()
    //    {
    //        return typeof(Array);
    //    }

    //    public override object Convert(string originalValue, string defaultValue)
    //    {
    //        return Convert(originalValue, defaultValue, ConstString.STR_EMPTY, ConstString.STR_EMPTY);
    //    }

    //    public override object Convert<T>(string originalValue, string defaultValue, T p)
    //    {
    //        return Convert(originalValue, defaultValue, p, ConstString.STR_EMPTY);
    //    }

    //    public override object Convert<T, K>(string originalValue, string defaultValue, T p1, K p2)
    //    {
    //        // 支持配置方式(x1,y1,z1)(x2,y2,z2)
    //        var v = string.IsNullOrEmpty(originalValue) ? defaultValue : originalValue;

    //        var asep = p1 as string;
    //        if (asep == null || string.IsNullOrEmpty(asep))
    //        {
    //            string defSep = string.Empty;
    //            if (v.Contains(")"))
    //            {
    //                v = v.Replace("(", "");
    //            }
    //            else
    //            {
    //                defSep = "|";
    //            }
    //            asep = defSep;
    //        }

    //        return Splite(v, asep);
    //    }

    //    object Splite(string value, string sep)
    //    {
    //        var splits = value.Split(new String[] { sep }, StringSplitOptions.None);
    //        var array = Array.CreateInstance(eleConverter.GetResultType(), splits.Length);
    //        for (int i = 0; i < splits.Length; i++)
    //        {
    //            array.SetValue(eleConverter.Convert(splits[i], null), i);
    //        }
    //        return array;
    //    }
    //}


    //public class EnumListConverter : Converter
    //{
    //    public override Type GetResultType()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}



    //public class CustomTypeListConvert : Converter
    //{
    //    int count = 0;
    //    Converter eleConverter;
    //    public CustomTypeListConvert(string subName)
    //    {
    //        eleConverter = new CustomTypeConverter(subName);
    //    }

    //    public override Type GetResultType()
    //    {
    //        return typeof(Array);
    //    }

    //    public override object Convert(string originalValue, string defaultValue)
    //    {
    //        return Convert(originalValue, defaultValue, ConstString.STR_EMPTY, ConstString.STR_EMPTY);
    //    }

    //    public override object Convert<T>(string originalValue, string defaultValue, T p)
    //    {
    //        return Convert(originalValue, defaultValue, p, ConstString.STR_EMPTY);
    //    }

    //    public override object Convert<T, K>(string originalValue, string defaultValue, T p1, K p2)
    //    {
    //        // 支持配置方式(x1,y1,z1)(x2,y2,z2)
    //        var v = string.IsNullOrEmpty(originalValue) ? defaultValue : originalValue;

    //        var asep = p1 as string;
    //        if (asep == null || string.IsNullOrEmpty(asep))
    //        {
    //            string defSep = string.Empty;
    //            if (v.Contains(")"))
    //            {
    //                v = v.Replace("(", "");
    //            }
    //            else
    //            {
    //                defSep = "|";
    //            }
    //            asep = defSep;
    //        }

    //        return Splite(v, asep);
    //    }

    //    object Splite(string value, string sep)
    //    {
    //        var splits = value.Split(new String[] { sep }, StringSplitOptions.None);
    //        var array = Array.CreateInstance(eleConverter.GetResultType(), splits.Length);
    //        for (int i = 0; i < splits.Length; i++)
    //        {
    //            array.SetValue(eleConverter.Convert(splits[i], null), i);
    //        }
    //        return array;
    //    }
    //}





}
