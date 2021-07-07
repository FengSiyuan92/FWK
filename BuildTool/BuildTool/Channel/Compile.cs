using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.RawDefine;
using Channel.Define;
using Enum = Channel.Define.Class.Enum;
using System.Text.RegularExpressions;
using Channel.Define.Converter;
using Channel.Define.Class;

namespace Channel
{
    public class Compile
    {
        // 基础数据类型
        // TODO: 支持DataTime
 
        static Dictionary<string, Converter> BaseConverter = new Dictionary<string, Converter>()
        {
            { "int", new IntConverter()},
            { "float", new FloatConverter()},
            { "string", new StringConverter()},
            { "vector2", new Vector2Converter()},
            { "vector3", new Vector3Converter()},
            { "vector4", new Vector4Converter()},

            { "vec2", new Vector2Converter()},
            { "vec3", new Vector3Converter()},
            { "vec4", new Vector4Converter()},

            { "int[]", new ListConverter(new IntConverter())},
            { "float[]", new ListConverter(new FloatConverter())},
            { "string[]", new ListConverter(new StringConverter(), ConstString.SEP_LEVEL_1)},
            { "vector2[]", new ListConverter( new Vector2Converter(),ConstString.SEP_LEVEL_1)},
            { "vector3[]", new ListConverter( new Vector3Converter(),ConstString.SEP_LEVEL_1)},
            { "vector4[]", new ListConverter( new Vector4Converter(),ConstString.SEP_LEVEL_1)},

            { "vec2[]", new ListConverter( new Vector2Converter(),ConstString.SEP_LEVEL_1)},
            { "vec3[]", new ListConverter( new Vector3Converter(),ConstString.SEP_LEVEL_1)},
            { "vec4[]", new ListConverter( new Vector4Converter(),ConstString.SEP_LEVEL_1)},
        };

        static Dictionary<string, EnumConverter> enumConverters = new Dictionary<string, EnumConverter>();
        static EnumConverter GetEnumConvert(string name, string sourceInfo)
        {
            EnumConverter target = null;
            lock(enumConverters)
            {
                if (!enumConverters.TryGetValue(name, out target))
                {
                    target = new EnumConverter(name);
                    target.SourceInfo = sourceInfo;
                    enumConverters.Add(name, target);
                }
            }
            return target;
        }

        static Dictionary<string, CustomTypeConverter> customConverters = new Dictionary<string, CustomTypeConverter>();
        static CustomTypeConverter GetCutomConvert(string name, string sourceInfo)
        {
            CustomTypeConverter target = null;
            lock(customConverters)
            {
                if (!customConverters.TryGetValue(name, out target))
                {
                    target = new CustomTypeConverter(name);
                    target.SourceInfo = sourceInfo;
                    customConverters.Add(name, target);
                }
            }
            
            return target;
        }


        static bool CheckIsEnum(string name)
        {
            return Lookup.Enum[name] != null;
        }

        public static void StartCompile()
        {

            FileAgent.LoadAllDefine();

            // 初始化定义完成后,遍历定义执行编译
            var defineNames = Lookup.RawDefine.AllName();

            // 第一次编译,编译初始定义的类型信息
            Utils.Parallel(defineNames, CompileRawDef);

            // 编译完成后的检查
            Check.CompileOverCheck();

            CLog.OutputAndClearCache("定义编译完成");
        }


        static void CompileRawDef(string objTypeName)
        {
            RawObjDef def = Lookup.RawDefine[objTypeName];
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


        static void GetFieldIndex(string ori)
        {
    
        }

        static void CompileObjDefine1(RawObjDef rawDef)
        {
            CustomType objDef = new CustomType(rawDef.Name);
            var filedNames = rawDef.GetAllRawFieldName();
            foreach (var filedName in filedNames)
            {
                var rawField = rawDef[filedName];
                Field field = new Field();
                // 关联原生定义
                field.RawDefine = rawField;
                // 字段名称
                field.FieldName = filedName;
                // 字段导出类型
                field.OutputType = GetOutputType(rawField.OutputType);
                // 字段是否需要作为key值
                field.IsKey = !string.IsNullOrEmpty(GetContent(rawField.AppendDef, ConstString.STR_KEY));
                // 字段值引用内容信息
                field.RefPos = GetContent(rawField.AppendDef, ConstString.STR_REF);
                // 字段默认值,用于excel不填写时的默认填充
                field.OriginalDefaultValue = GetContent(rawField.AppendDef, ConstString.STR_DEFAULT);
                // 内置字段
                field.FieldIndex = Math.Max(rawField.DefIndex - 1, 0);

                var seps = GetContent(rawField.AppendDef, ConstString.STR_SEP);
                if (!string.IsNullOrEmpty(seps))
                {
                    field.Seps = seps.ToCharArray();
                }

                // 字段编译类型
                Converter convert = null;
                // 检查是否是基础数据类型, 第一次编译只编译基础数据类型
                var rawType = rawField.FieldType;
                if (BaseConverter.TryGetValue(rawType, out convert))
                {
                    field.Convert = convert;
                }
                else if (rawType.EndsWith("[]"))
                {
                    var eleType = rawType.Remove(rawType.Length - 2);
                    if (CheckIsEnum(eleType))
                    {
                        field.Convert = new ListConverter(GetEnumConvert(eleType, field.Source()));
                    }
                    else
                    {
                        field.Convert = new ListConverter(GetCutomConvert(eleType, field.Source()));
                    }
                }
                else if (CheckIsEnum(rawType))
                {
                    field.Convert = GetEnumConvert(rawType, field.Source());
                }
                else
                {
                    field.Convert = GetCutomConvert(rawType, field.Source());
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

        const string MATCH_PATTERN = @"(=?[^\&\s]*)&*";
        static Regex GetContentRegex(string name)
        {
            Regex reg = null;
            lock(contentReg)
            {
                if (!contentReg.TryGetValue(name, out reg))
                {
                    reg = new Regex(name + MATCH_PATTERN, RegexOptions.IgnoreCase);
                    contentReg.Add(name, reg);
                }
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
