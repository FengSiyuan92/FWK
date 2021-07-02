using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.RawDefine;
using Channel.Define;
using Enum = Channel.Enum;
using System.Text.RegularExpressions;
namespace Channel
{
    public class Compiler
    {

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
                field.FieldName = filedName;
                field.OutputType = GetOutputType(rawField.OutputType);
                field.IsKey = CheckIsKey(rawField.AppendDef);
                field.AliasRefPos = GetAliasPos(rawField.AppendDef);
                field.OriginalDefaultValue = 
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
                    GetAlias(fielddef.AppendDef)
                    );
            }
        }

        static OutputType GetOutputType(string rawType)
        {
            switch (rawType)
            {
                case "c":
                    return OutputType.OnlyClient;
                case "s":
                    return OutputType.ClientAndServer;
                case "cs":
                case "sc":
                case "":
                    return OutputType.ClientAndServer;
                default:
                    break;
            }
            return OutputType.ClientAndServer;
        }


        static Regex AliasContentReg = new Regex(@"alias=([^\|\s]*)\|*", RegexOptions.IgnoreCase);
        static string GetAlias(string appendDef)
        {
            if (string.IsNullOrEmpty(appendDef))
            {
                return string.Empty;
            }

            var res = AliasContentReg.Match(appendDef);
            if (res.Success && res.Groups.Count > 1)
            {
                return res.Groups[1].ToString();
            }
            return "";
        }


        static Regex AliasPosReg = new Regex(@"alias=?([^\|\s]*)\|*", RegexOptions.IgnoreCase);
        static string GetAliasPos(string appendDef)
        {
            if (string.IsNullOrEmpty(appendDef))
            {
                return string.Empty;
            }
            var res = AliasPosReg.Match(appendDef);
            if (res.Success)
            {

                if (res.Groups.Count > 1)
                {
                    var pos = res.Groups[1].ToString();
                    return string.IsNullOrEmpty(pos) ? "default" : pos;
                }
                return "default";
            }

            return string.Empty;
        }


        static Regex IsKeyReg = new Regex(@"key\d?", RegexOptions.IgnoreCase);
        static bool CheckIsKey(string appendDef)
        {
            if (string.IsNullOrEmpty(appendDef))
            {
                return false;
            }
            var res = IsKeyReg.Match(appendDef);
            return res.Success && res.Groups.Count > 0;
        }

        static Regex DefValueReg = new Regex(@"default=?([^\|\s]*)\|*", RegexOptions.IgnoreCase);
        static string GetDefaultValue(string appendDef)
        {

            if (string.IsNullOrEmpty(appendDef))
            {
                return string.Empty;
            }
            var res = DefValueReg.Match(appendDef);
            if (res.Success)
            {
                if (res.Groups.Count > 1)
                {
                    var pos = res.Groups[1].ToString();
                    return string.IsNullOrEmpty(pos) ? "default" : pos;
                }
                return "default";
            }
            return string.Empty;
        }

    }
}
