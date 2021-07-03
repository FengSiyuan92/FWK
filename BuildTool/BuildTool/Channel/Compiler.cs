using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.RawDefine;
using Channel.Define;
using Enum = Channel.Define.Enum;
using System.Text.RegularExpressions;

using Channel.Define.CompileType;

namespace Channel
{
    public class Compiler
    {
        // 基础数据类型
        // TODO: 支持DataTime
 
        static Dictionary<string, Define.CompileType.Converter> BaseConverter = new Dictionary<string, Define.CompileType.Converter>()
        {
            { "int", new IntConverter()},
            { "float", new FloatConverter()},
            { "string", new StringConverter()},
            { "int[]", new ListConverter(new IntConverter())},
            { "float[]", new ListConverter(new FloatConverter())},
            { "string[]", new ListConverter(new StringConverter())},
            { "vector2", new VectorConverter(2)},
            { "vector3", new VectorConverter(3)},
            { "vector4", new VectorConverter(4)},
            { "vector2[]", new VectorListConverter(2)},
            { "vector3[]", new VectorListConverter(3)},
            { "vector4[]", new VectorListConverter(4)},
        };

        static bool CheckIsEnum(string name)
        {
            return Lookup.Enum[name] == null;
        }

        public static void StartCompile()
        {
            // 后面尝试改成多线程loaddefine和异步
            FileAgent.LoadAllDefine();

            Lookup.Test();
            // 初始化定义完成后,遍历定义执行编译

            var defineNames = Lookup.RawDefine.AllName();

            // 第一次编译,编译初始定义的类型信息
            foreach (var objTypeName in defineNames)
            {
                RawObjDef def = Lookup.RawDefine[objTypeName];  
                CompileRawDef(def);
            }

            // 检查一遍所有的自定义类型是否存在
            foreach (var item in CustomTypeConverter.delayComileType)
            {
                if (Lookup.CustomType[item.Name] == null)
                {
                    CLog.LogError("{0}类型的定义不存在,请检查", item.Name);
                }
            }
        }

        static void CompileRawDef(RawObjDef def)
        {
            switch (def.DefType)
            {
                case RawObjType.ENUM:
                    CompileEnumDefine1(def);
                    break;
                case RawObjType.OBJECT:
                    CompileObjDefine1(def);
                    break;
                default:
                    break;
            }
        }

        static void CompileObjDefine1(RawObjDef rawDef)
        {
            CustomType objDef = new CustomType(rawDef.Name);
            var filedNames = rawDef.GetAllRawFieldName();
            foreach (var filedName in filedNames)
            {
                var rawField = rawDef[filedName];
                Field field = new Field();
                // 字段名称
                field.FieldName = filedName;
                // 字段导出类型
                field.OutputType = GetOutputType(rawField.OutputType);
                // 字段是否需要作为key值
                field.IsKey = string.IsNullOrEmpty(GetContent(rawField.AppendDef, ConstString.STR_KEY));
                // 字段值引用的别名信息
                field.AliasRefPos = GetContent(rawField.AppendDef, ConstString.STR_ALIAS);
                // 字段默认值,用于excel不填写时的默认填充
                field.OriginalDefaultValue = GetContent(rawField.AppendDef, ConstString.STR_DEFAULT);

                // 字段编译类型
                Define.CompileType.Converter type = null;
                // 检查是否是基础数据类型, 第一次编译只编译基础数据类型
                var rawType = rawField.FieldType;
                if (BaseConverter.TryGetValue(rawType, out type))
                {
                    field.Convert = type;
                }
                else if (rawType.EndsWith("[]"))
                {
                    var eleType = rawType.Remove(rawType.Length - 2);
                    if (CheckIsEnum(eleType))
                    {
                        field.Convert = new EnumListConverter(rawType);
                    }
                    else
                    {
                        field.Convert = new CustomTypeConverter(rawType);
                    }
                }
                else if (CheckIsEnum(eleType))
                {
                    field.Convert = new CustomTypeListConvert();
                }
                else
                {
                    field.Convert = new CustomTypeConverter(rawType);
                }

                objDef.AddField(field);
            }
        }

        static void CompileEnumDefine1(RawObjDef def)
        {
            Enum en = new Enum(def.Name);
            var filedNames = def.GetAllRawFieldName();

            foreach (var filedName in filedNames)
            {
                var fielddef = def[filedName];
                if (fielddef == null) continue;
                en.AddEnumItem(
                    fielddef.FieldName,
                    int.Parse(fielddef.DefaultValue),
                    GetOutputType(fielddef.OutputType),
                    GetContent(fielddef.AppendDef, ConstString.STR_ALIAS)
                    );
            }
        }

        static OutputType GetOutputType(string rawType)
        {
            switch (rawType)
            {
                case ConstString.STR_C:
                    return OutputType.OnlyClient;
                case ConstString.STR_S:
                    return OutputType.ClientAndServer;
                case ConstString.STR_SC:
                case ConstString.STR_CS:
                case ConstString.STR_EMPTY:
                    return OutputType.ClientAndServer;
                default:
                    break;
            }
            return OutputType.ClientAndServer;
        }

        static Dictionary<string, Regex> contentReg = new Dictionary<string, Regex>();

        const string MATCH_PATTERN = @"(=?[^\|\s]*)\|*";
        static Regex GetContentRegex(string name)
        {
            Regex reg = null;
            if (!contentReg.TryGetValue(name, out reg))
            {
                reg = new Regex(name + MATCH_PATTERN, RegexOptions.IgnoreCase);
                contentReg.Add(name, reg);
            }
            return reg;
        }

        static string GetContent(string original, string name)
        {
            if (string.IsNullOrEmpty(original))
            {
                return string.Empty;
            }
            var reg = GetContentRegex(name);
            var res = reg.Match(original);
            if (!res.Success)
            {
                return string.Empty;
            }

            if (res.Groups.Count == 1)
            {
                return ConstString.STR_DEFAULT;
            }

            var value = res.Groups[1].ToString();
            if (value.StartsWith(ConstString.STR_EQ))
            {
                return value.Substring(1);
            }

            return ConstString.STR_DEFAULT;
        }



        


    }
}
