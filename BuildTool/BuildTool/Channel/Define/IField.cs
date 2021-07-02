using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define.CompileType;

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


    public class Field
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
        /// 别名的指向,为string.Empty时说明不使用别名
        /// </summary>
        internal string AliasRefPos { get; set; }

        /// <summary>
        /// 原始填入的默认值
        /// </summary>
        internal string OriginalDefaultValue { get; set; }

        /// <summary>
        /// 数据编译类型
        /// </summary>
        internal CompileType.CompileType FieldType { get; set; }
       
    }






}
