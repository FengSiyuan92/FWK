using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.RawDefine;
using Channel.Define;
using Enum = Channel.Enum;
using System.Text.RegularExpressions;

using Channel.Define.CompileType;

namespace Channel
{
    public class Compiler
    {
        // 基础数据类型
        // TODO: 支持DataTime
 
        static Dictionary<string, CompileType> BaseType = new Dictionary<string, CompileType>()
        {
            { "int", new IntType()},
            { "float", new FloatType()},
            { "string", new StringType()},
            { "int[]", new ListType(new IntType())},
            { "float[]", new ListType(new FloatType())},
            { "string[]", new ListType(new StringType())},
        };

       
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

            // 对所有编译类型执行嵌套的二次编译,用于关联自定义类型
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
            ObjectDefine objDef = new ObjectDefine(rawDef.Name);
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
              
                CompileType type = null;

                // 检查是否是基础数据类型, 第一次编译只编译基础数据类型
                if (BaseType.TryGetValue(rawField.FieldType, out type))
                {
                    field.FieldType = type;
           
                }

    

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
