using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.OutputDefine
{
    /// <summary>
    ///  导出字段定义
    /// </summary>
    public class FieldDefine : IFieldDefine
    {
        #region 原始数据配置
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段转换类型
        /// </summary>
        public string fieldType;

        /// <summary>
        /// 字段导出类型
        /// </summary>
        public string outType;

        /// <summary>
        /// 附加定义
        /// 用来支持表引用,别名等
        /// </summary>
        public string valueAppend;

        /// <summary>
        /// 检查规则
        /// </summary>
        public string checkRule;
        #endregion


        #region 通过原生value解析出来的build用含义字段
        /// <summary>
        /// 该字段引用的其他类型的名称
        /// </summary>
        public string refType { get; private set; }

        /// <summary>
        /// 该字段引用其他类型的字段名称
        /// </summary>
        public string refField { get; private set; }

        /// <summary>
        /// 数据源中配置的数值,是否配置了对应类型的别名
        /// </summary>
        public bool valueIsAlias { get; private set; }

        /// <summary>
        /// 改字段是否需要做为key值. 如果是key值的话需要做几级key值
        /// </summary>
        public int IsKey { get; private set; }

        #endregion
        internal void Merge(FieldDefine otherDefine)
        {

        }
    }

   
}
