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
    class ObjectDefine : IObjDefine
    {
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
        }

        bool parsed = false;
        

        public void ParseDefine()
        {
            if(parsed)  return;
            foreach (var item in defines)
            {
                item.Value.AnalysisOriginalDefine();
            }
            parsed = true;
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
                }
                else
                {
                    oldDefine.Merge(define.Value);
                }
            }
        }
    }

}
