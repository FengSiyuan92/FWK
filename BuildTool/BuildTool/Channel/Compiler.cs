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
            var defineNames = Lookup.LookAllRawDefineName();

            // 第一次编译,编译初始定义的类型信息
            foreach (var objTypeName in defineNames)
            {
               RawObjDef def = Lookup.LookupRawObjDef(objTypeName);
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

        static void CompileObjDefine1(RawObjDef def)
        {

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

        const string AliasMath = @"alias=(.*)\|*";

        static Regex aliasReg = new Regex(AliasMath, RegexOptions.IgnoreCase);
        static string GetAlias(string appendDef)
        {
            if (string.IsNullOrEmpty(appendDef))
            {
                return string.Empty;
            }

            var res = aliasReg.Match(appendDef);
            if (res.Success &&res.Groups.Count > 1)
            {
                return res.Groups[1].ToString();
            }
            return "";
        }

    }
}
