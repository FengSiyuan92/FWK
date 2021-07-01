using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.RawDefine;
using Channel.Define;

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

        }

    }
}
