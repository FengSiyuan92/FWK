using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.RawDefine
{

    internal enum RawObjType
    {
        ERROR = 0,
        ENUM,
        OBJECT,
    }


    internal static class RawFieldType
    {
        public const string Int = "int";

    }

    /// <summary>
    /// 原生对象定义
    /// </summary>
    internal class RawObjDef 
    {
        /// <summary>
        /// 类名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 定义类型,目前分为枚举和obj
        /// </summary>
        public RawObjType DefType { get; set; }

        Dictionary<string, RawFieldDef> defines = new Dictionary<string, RawFieldDef>();

        /// <summary>
        ///  创建一个对象的原生定义
        /// </summary>
        /// <param name="name">定义对象类名</param>
        /// <param name="type">定义对象类型</param>
        public RawObjDef(string name, RawObjType type)
        {
            this.Name = name;
            this.DefType = type;
        }

        /// <summary>
        /// 通过字段名获取该对象中的字段定义
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public RawFieldDef this[string k]
        {
            get
            {
                RawFieldDef res = null;
                defines.TryGetValue(k, out res);
                return res;
            }
        }

        /// <summary>
        /// 获取所有原生字段定义名称
        /// </summary>
        /// <returns></returns>
        public string[] GetAllRawFieldName()
        {
            return defines.Keys.ToArray();
        }

        /// <summary>
        /// 支持合并两个名称相同的对象定义,类似partial
        /// </summary>
        /// <param name="otherDef"></param>
        public virtual void Merge(RawObjDef otherDef)
        {
  
            if (otherDef == null) return;

            foreach (var define in otherDef.defines)
            {
                RawFieldDef oldDefine = null;
                if (!defines.TryGetValue(define.Key, out oldDefine))
                {
                    defines.Add(define.Key, define.Value);
                }
            }
        }

        /// <summary>
        /// 向对象的原生定义中,添加一个字段定义
        /// </summary>
        /// <param name="def"></param>
        public virtual void AddFieldDefine(RawFieldDef def)
        {
            if (def == null) return;

            var key = def.FieldName;
            if (defines.ContainsKey(key))
            {
                CLog.LogError("重复添加相同的字段名");
            }

            defines.Add(key, def);
        }
    }

    /// <summary>
    /// 原生字段定义
    /// </summary>
    internal class RawFieldDef
    {
        /// <summary>
        /// 原生字段名
        /// </summary>
        public string FieldName = string.Empty;
        /// <summary>
        /// 原生字段类型(string),编译后才会转换为正确的类型
        /// </summary>
        public string FieldType = string.Empty;

        /// <summary>
        /// 原生字段导出类型(string), 编译后才会转换为正确的类型
        /// </summary>
        public string OutputType = string.Empty;

        /// <summary>
        ///  原生字段内容描述的附加信息(string),主要用来指定填入内容的一些特点
        /// </summary>
        public string AppendDef = string.Empty;

        /// <summary>
        /// 原生字段内容描述的检查规则,编译后支持自定义类型添加
        /// </summary>
        public string CheckRule = string.Empty;

        /// <summary>
        /// 该字段在创建新的对象实例时提供的原生默认值,编译后才会转为类型正确的信息
        /// </summary>
        public string DefaultValue = string.Empty;

        /// <summary>
        /// TODO:该字段对应的内容指定索引,部分功能需要使用
        /// </summary>
        public int DefIndex = 0;
    }
}
