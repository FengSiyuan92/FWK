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
    internal class ObjectDefine : IObjDefine
    {

        List<string> fieldName = new List<string>();
        Dictionary<string, FieldDefine> defines = new Dictionary<string, FieldDefine>();


        public string Name { get; set; }
        public IFieldDefine this[string key, bool alias = false]
        {
            get
            {
                FieldDefine def;
                defines.TryGetValue(key, out def);
                return def;
            }
        }

        public void AddFieldDefine(IFieldDefine def)
        {
            var define = def as FieldDefine;
            if (define == null) return;

            var key = define.FieldName;
            if (defines.ContainsKey(key))
            {
                throw new Exception("重复添加相同的字段名");
            }
            defines.Add(key, define);
            fieldName.Add(key);
        }


        /// <summary>
        /// 支持将多个ObjectDefine进行合并
        /// </summary>
        /// <param name="otherDefine"></param>
        internal void Merge(ObjectDefine otherDefine)
        {
            foreach (var define in otherDefine.defines)
            {
                FieldDefine oldDefine = null;
                if (!defines.TryGetValue(define.Key, out oldDefine))
                {
                    defines.Add(define.Key, define.Value);
                    fieldName.Add(define.Key);
                }
                else
                {
                    oldDefine.Merge(define.Value);
                }
            }
        }
    }

}
