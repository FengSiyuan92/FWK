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
    class FieldDefine
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string fieldName;

        /// <summary>
        /// 字段转换类型
        /// </summary>
        public string fieldType;

        /// <summary>
        /// 数据填充的是否是别名
        /// </summary>
        public bool useAlise;

        /// <summary>
        /// 字段导出类型
        /// </summary>
        public string outType;

        /// <summary>
        /// 附加定义
        /// 用来支持表引用,别名等
        /// </summary>
        public string append;

        /// <summary>
        /// 检查规则
        /// </summary>
        public string checkRule;

        public void Merge(FieldDefine otherDefine)
        {

        }
    }

   
}
