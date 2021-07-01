using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Define
{
    /// <summary>
    /// 字段导出类型
    /// </summary>
    public enum OutputType
    {
        ClientAndServer = 0,
        OnlyClient,
        OnlyServer,
    }


    public class Field<T>
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; internal set; }

        /// <summary>
        /// 字段导出类型
        /// </summary>
        public OutputType OutputType;

        /// <summary>
        /// 该字段填入的值,是否需要作为table data的key
        /// </summary>
        public bool IsKey { get; internal set; }

        /// <summary>
        /// 该字段对应的内容是否使用的是别名
        /// </summary>
        public bool UseAlias { get; internal set; }

        /// <summary>
        /// 原始填入的默认值
        /// </summary>
        public string OriginalDefaultValue { get; set; }


        public T FieldType;


    }






}
