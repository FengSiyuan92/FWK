using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Channel.TransitionData;

namespace Channel.OutputDefine
{
    /// <summary>
    /// 导出表对象定义
    /// </summary>
    class ObjectDefine
    {
        Dictionary<string, FieldDefine> defines = new Dictionary<string, FieldDefine>();

        public void AddFieldDefine(FieldDefine define)
        {
            var key = define.fieldName;
            if (defines.ContainsKey(key))
            {
                throw new Exception("重复添加相同的字段名");
            }
            defines.Add(key, define);
        }


        /// <summary>
        /// 支持将多个ObjectDefine进行合并
        /// </summary>
        /// <param name="otherDefine"></param>
        public void Merge(ObjectDefine otherDefine)
        {
            foreach (var define in otherDefine.defines)
            {
                FieldDefine oldDefine = null;
                if (!defines.TryGetValue(define.Key, out oldDefine))
                {
                    defines.Add(define.Key, define.Value);
                }
                else
                {
                    oldDefine.Merge(define.Value);
                }
            }
        }
    }

}
