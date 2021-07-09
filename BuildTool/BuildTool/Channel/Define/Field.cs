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

    public class Field:ISource
    {
        public CustomClass Belong;

        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; internal set; }

        /// <summary>
        /// 导出的数据类型
        /// </summary>
        public Type FieldType => Convert.GetResultType();

        /// <summary>
        /// 字段导出类型
        /// </summary>
        public OutputType OutputType { get; internal set; }

        /// <summary>
        /// 该字段填入的值,是否需要作为table data的key
        /// </summary>
        public bool IsKey { get; internal set; }

        /// <summary>
        /// 别名的指向,为string.Empty时说明不使用别名
        /// </summary>
        public string RefPos { get; internal set; }

        /// <summary>
        /// 原始填入的默认值
        /// </summary>
        public string OriginalDefaultValue { get; internal set; }

        /// <summary>
        /// 数据编译类型
        /// </summary>
        internal Converter.Converter Convert { get; set; }

        /// <summary>
        /// 自定义分隔符数组
        /// </summary>
        internal char[] Seps;
        /// <summary>
        /// 字段索引
        /// </summary>
        internal int FieldIndex;

        /// <summary>
        /// 原生定义关联
        /// </summary>
        internal RawDefine.RawFieldDef RawDefine;

        public string Source()
        {
            return RawDefine.SourceInfo;
        }
    }






}
