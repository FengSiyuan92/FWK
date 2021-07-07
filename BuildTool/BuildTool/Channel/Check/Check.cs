﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define.Converter;
using Channel.Define;
namespace Channel
{
    public class Check
    {
        public delegate void OnOverCompileEvent();
        public static event OnOverCompileEvent OnOverCompile;

        internal static bool CompileOverCheck()
        {
            try
            {
                OnOverCompile();
                return true;
            }
            catch (Exception e)
            {

                CLog.LogError(e.ToString());
            }
            return false;
        }


        public delegate void OnOverParseEvent();
        public static event OnOverParseEvent OnOverParse;


        internal static bool OverParse()
        {
            try
            {
                OnOverParse();
                return true;
            }
            catch (Exception e)
            {

                CLog.LogError(e.ToString());
            }
            return false;
        }

        static Check()
        {
            OnOverCompile += InnerCheckFunc.CheckConverterValid;

            OnOverParse += InnerCheckFunc.CheckReference;
        }
    }

    internal class InnerCheckFunc
    {

        const string errortip = "不存在名称为{0}的枚举类型,但是却有配置表要使用该类型=>{1}";
        /// <summary>
        /// 检查扩展的类型转换器是否可用
        /// </summary>
        public static void CheckConverterValid()
        {
            var allextend = ExtendConverter.allExtend;
            foreach (var item in allextend)
            {
                if (item is EnumConverter)
                {
                    var name = (item as EnumConverter).Name;
                    var e = Lookup.Enum[name];
                    if (e == null)
                    {
                        CLog.LogError(errortip, name);
                    }
                }
                else if (item is CustomTypeConverter)
                {
                    var name = (item as CustomTypeConverter).Name;
                    var c = Lookup.CustomType[name];
                    if (c == null)
                    {
                        CLog.LogError(errortip, name);
                    }
                }
            }
        }


        /// <summary>
        /// 检查字段引用是否合规
        /// </summary>
        public static void CheckReference()
        {
            // 缓存了所有需要检查的类名和下属字段名
            Dictionary<string, List<Field>> allCheckName = new Dictionary<string, List<Field>>();

            // 类型名 需要做内容备份的字段名
            Dictionary<string, HashSet<string>> contentIndex = new Dictionary<string, HashSet<string>>();
            // 获取所有的类型定义名称
            var allCustomType = Lookup.CustomType.AllName();
            List<Field> targetFieldName = new List<Field>();

            // 通过标记引用的字段索引内容字段
            foreach (var item in allCustomType)
            {
                // 取到对应类型定义
                var custom = Lookup.CustomType[item];
                // 取到该类型下的所有字段定义名称
                var fields = custom.AllFieldName();
                foreach (var fieldName in fields)
                {
                    // 取到字段定义
                    var field = custom[fieldName];
                    // 有引用信息
                    if (!string.IsNullOrEmpty(field.RefPos))
                    {
                        targetFieldName.Add(field);
                        var candf = field.RefPos.Split('.');
                        var targetClassName = candf[0];
                        HashSet<string> names = null;
                        if (!contentIndex.TryGetValue(targetClassName, out names))
                        {
                            names = new HashSet<string>();
                            contentIndex.Add(targetClassName, names);
                        }
                        names.Add(candf[1]);
                    }
                }
                if (targetFieldName.Count!= 0)
                {
                    allCheckName.Add(item, targetFieldName);
                    targetFieldName = new List<Field>();
                }
            }

            // 做引用内容备份 key classname key1 fieldname list objects
            Dictionary<string, Dictionary<string, HashSet<object>>> refContent = new Dictionary<string, Dictionary<string, HashSet<object>>>();
            foreach (var item in contentIndex)
            {
                var className = item.Key;
                var fields = item.Value;

                // fieldname 和所有有效数据
                Dictionary<string, HashSet<object>> c = new Dictionary<string, HashSet<object>>();
                foreach (var fieldname in fields)
                {
                    c.Add(fieldname, new HashSet<object>());
                }

                // 通过类名取到所有数据
                var datas = Lookup.Datas[className].AllDatas();
                // 遍历数据从里面拿所需字段对应的数据,并添加进去
                foreach (var data in datas)
                {
                    foreach (var fieldName in fields)
                    {
                        c[fieldName].Add(data[fieldName]);
                    }
                }

                refContent.Add(className, c);
            }

            Utils.Parallel(allCheckName, (i)=> CheckRef(i, refContent));
        }

        static void CheckRef(KeyValuePair<string, List<Field>> item,
            Dictionary<string, Dictionary<string, HashSet<object>>> refContent)
        {
            var className = item.Key;
            var fields = item.Value;

            var getter = Lookup.Datas[className];
            if (getter==null)
            {
                return;
            }
            var allData = getter.AllDatas();

            foreach (var field in fields)
            {
                var refPos = field.RefPos;
                var splitInfo = refPos.Split('.');
                var targetClassName = splitInfo[0];
                var targetFieldName = splitInfo[1];
                var content = refContent[targetClassName][targetFieldName];

                foreach (var data in allData)
                {
                    // obj自己的数据
                    var objData = data[field.FieldName];
                    if (!Utils.EqualsContains(content, objData))
                    {
                        CLog.LogError("{0}类型中不存在{1}={2}的数据,但是却想在=>{3}.{4}中使用",
                            targetClassName, targetFieldName, objData.ToString(),
                            data.Source(), field.FieldName);
                    }
                }
            }
        }
    }
}