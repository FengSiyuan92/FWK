using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.TransitionData;

namespace Channel.OutputDefine
{

    class EnumDefine :IObjDefine
    {
        /// <summary>
        /// 枚举定义的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 枚举元素
        /// </summary>

        /// <summary>
        /// 用字段名的缓存
        /// </summary>
        Dictionary<string, EnumFieldDefine> fieldMap = new Dictionary<string, EnumFieldDefine>();
        /// <summary>
        /// 用别名的缓存
        /// </summary>
        Dictionary<string, EnumFieldDefine> aliasMap = new Dictionary<string, EnumFieldDefine>();


        /// <summary>
        /// 添加一个枚举值字段定义
        /// </summary>
        public void AddFieldDefine(IFieldDefine fieldDef)
        {
            var enumField = fieldDef as EnumFieldDefine;
            if (enumField == null) return;

            fieldMap.Add(enumField.FieldName, enumField);
            if (!string.IsNullOrEmpty(enumField.alias))
            {
                aliasMap.Add(enumField.alias, enumField);
            }
        }


        /// <summary>
        /// 取值,key可以是字段名或者是别名,当alias = true时就是表明要用别名索引,默认该值为false
        /// </summary>
        /// <param name="key"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public IFieldDefine this[string key, bool alias = false]
        {
            get
            {
                var target = alias ? fieldMap : aliasMap;
                EnumFieldDefine res ;
                target.TryGetValue(key, out res);
                return res;
            }
        }

    }
}
