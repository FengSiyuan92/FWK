using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
namespace Channel
{
    public class Test
    {
        //const string TestItemPath = @"D:\FWK\BuildTool\Config\Item.xlsx";
        //const string TestUseItempath = @"D:\FWK\BuildTool\Config\UseItem.xlsx";
        //const string ItemPart1 = @"D:\FWK\BuildTool\Config\Item(1-100).xlsx";
        //const string ItemPart2 = @"D:\FWK\BuildTool\Config\Item(1000-2000).xlsx";
        //const string EnumXML = @"D:\FWK\BuildTool\Config\CustomDef\EnumDefine.xml";
        //const string ObjXML = @"D:\FWK\BuildTool\Config\CustomDef\TypeDefine.xml";
        //const string TypeTestPath = @"D:\FWK\BuildTool\Config\TypeTest.xlsx";
        //const string IntTest = @"D:\FWK\BuildTool\Config\Test\IntTest.xlsx";
        //const string FloatTest = @"D:\FWK\BuildTool\Config\Test\FloatTest.xlsx";
        //const string StringTest = @"D:\FWK\BuildTool\Config\Test\StringTest.xlsx";
        //const string Vector2Test = @"D:\FWK\BuildTool\Config\Test\Vector2Test.xlsx";
        //const string EnumTest = @"D:\FWK\BuildTool\Config\Test\EnumTest.xlsx";
        //const string DefTest = @"D:\FWK\BuildTool\Config\Test\DefTest.xml";
        //const string CustomTest = @"D:\FWK\BuildTool\Config\Test\CustomTest.xlsx";
        //const string RefTest = @"D:\FWK\BuildTool\Config\Test\RefTest.xlsx";

        const string TestItemPath = @"F:\FWK\BuildTool\Config\Item.xlsx";
        const string TestUseItempath = @"F:\FWK\BuildTool\Config\UseItem.xlsx";
        const string ItemPart1 = @"F:\FWK\BuildTool\Config\Item(1-100).xlsx";
        const string ItemPart2 = @"F:\FWK\BuildTool\Config\Item(1000-2000).xlsx";
        const string EnumXML = @"F:\FWK\BuildTool\Config\CustomDef\EnumDefine.xml";
        const string ObjXML = @"F:\FWK\BuildTool\Config\CustomDef\TypeDefine.xml";
        const string TypeTestPath = @"F:\FWK\BuildTool\Config\TypeTest.xlsx";
        const string IntTest = @"F:\FWK\BuildTool\Config\Test\IntTest.xlsx";
        const string FloatTest = @"F:\FWK\BuildTool\Config\Test\FloatTest.xlsx";
        const string StringTest = @"F:\FWK\BuildTool\Config\Test\StringTest.xlsx";
        const string Vector2Test = @"F:\FWK\BuildTool\Config\Test\Vector2Test.xlsx";
        const string EnumTest = @"F:\FWK\BuildTool\Config\Test\EnumTest.xlsx";
        const string DefTest = @"F:\FWK\BuildTool\Config\Test\DefTest.xml";
        const string CustomTest = @"F:\FWK\BuildTool\Config\Test\CustomTest.xlsx";
        const string RefTest = @"F:\FWK\BuildTool\Config\Test\RefTest.xlsx";
        const string ConfigDir = @"F:\FWK\BuildTool\Config";


       public static void TestF()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            FileAgent.Register(ConfigDir);
            Parser.Compile();
            Parser.ParseData();
            sw.Stop();
            Console.WriteLine("测试结束,总用时=" + sw.Elapsed.TotalSeconds + "s");
            CodeWriter.WriteCode(@"F:\FWK\BuildTool\Config", CodeWriter.CodeType.DevLua);
        }

        const string LuaRuleDir = @"F:\FWK\BuildTool\Rule";

        public static void TestLua()
        {
            Checker.AddLuaRuleDirectory(LuaRuleDir);
        }

        public static void ParseTest()
        { 
            var namei = "arraydefaultsep";
            var i1 = Lookup.Datas["IntTest"][1][namei];
            var i2 = Lookup.Datas["IntTest"][2][namei];
            var i3 = Lookup.Datas["IntTest"][3][namei];

            var namef = "arraydefaultsep";
            var f1 = Lookup.Datas["FloatTest"][1][namef];
            var f2 = Lookup.Datas["FloatTest"][2][namef];
            var f3 = Lookup.Datas["FloatTest"][3][namef];

            var names = "arraydefaultsep";
            var s1 = Lookup.Datas["StringTest"]["第一个"][names];
            var s2 = Lookup.Datas["StringTest"]["第二个"][names];
            var s3 = Lookup.Datas["StringTest"]["第三个"][names];

            var namev= "arraydefaultsep";
            var v1 = Lookup.Datas["Vector2Test"]["第一个"][namev];
            var v2 = Lookup.Datas["Vector2Test"]["第二个"][namev];
            var v3 = Lookup.Datas["Vector2Test"]["第三个"][namev];

            var namee = "arraydefaultsep";
            var e1 = Lookup.Datas["EnumTest"]["第一个"][namee];
            var e2 = Lookup.Datas["EnumTest"]["第二个"][namee];
            var e3 = Lookup.Datas["EnumTest"]["第三个"][namee];

            var namec = "arraydefaultsep";
            var c1 = Lookup.Datas["CustomTest"]["第一个"][namec];
            var c2 = Lookup.Datas["CustomTest"]["第二个"][namec];
            var c3 = Lookup.Datas["CustomTest"]["第三个"][namec];

            var namer = "e";
            var r1 = Lookup.Datas["RefTest"]["第一个"][namer];
            var r2 = Lookup.Datas["RefTest"]["第二个"][namer];
            var r3 = Lookup.Datas["RefTest"]["第三个"][namer];

            var a = 0;
        }

   
    }
}
