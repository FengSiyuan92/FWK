using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Test
{
    public class Test
    {
        const string TestItemPath = @"D:\FWK\BuildTool\Config\Item.xlsx";
        const string TestUseItempath = @"D:\FWK\BuildTool\Config\UseItem.xlsx";
        const string ItemPart1 = @"D:\FWK\BuildTool\Config\Item(1-100).xlsx";
        const string ItemPart2 = @"D:\FWK\BuildTool\Config\Item(1000-2000).xlsx";
        const string EnumXML = @"D:\FWK\BuildTool\Config\CustomDef\EnumDefine.xml";
        const string ObjXML = @"D:\FWK\BuildTool\Config\CustomDef\TypeDefine.xml";
        const string TypeTestPath = @"D:\FWK\BuildTool\Config\TypeTest.xlsx";
        const string IntTest = @"D:\FWK\BuildTool\Config\Test\IntTest.xlsx";
        const string FloatTest = @"D:\FWK\BuildTool\Config\Test\FloatTest.xlsx";
        const string StringTest = @"D:\FWK\BuildTool\Config\Test\StringTest.xlsx";
        const string Vector2Test = @"D:\FWK\BuildTool\Config\Test\Vector2Test.xlsx";
        const string EnumTest = @"D:\FWK\BuildTool\Config\Test\EnumTest.xlsx";
        const string DefTest = @"D:\FWK\BuildTool\Config\Test\DefTest.xml";
        const string CustomTest = @"D:\FWK\BuildTool\Config\Test\CustomTest.xlsx";
        const string RefTest = @"D:\FWK\BuildTool\Config\Test\RefTest.xlsx";


        public static void TypeDefineTest()
        {
            List<string> filePath = new List<string>
            {
                EnumXML,
               IntTest,FloatTest,StringTest,Vector2Test,DefTest,EnumTest,CustomTest,RefTest
            };

            FileAgent.RegisterFiles(filePath);
            Compile.StartCompile();
        }


        public static void ParseTest()
        {
            TypeDefineTest();
            Parse.StartParse();

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

        public static void TestFunc()
        {

            //var allEnums = Lookup.Enum.AllName();
            //foreach (var enumName in allEnums)
            //{
            //    var e = Lookup.Enum[enumName];  //Lookup.LookEnum(enumName);

            //    var fieldName = e.GetAllItemName();
            //    foreach (var item in fieldName)
            //    {
            //        CLog.LogError(e.GetItemByFieldName(item).ToString());
            //    }
            //}

            //foreach (var enumName in allEnums)
            //{
            //    var e = Lookup.Enum[enumName];
            //    CLog.Log(e.ToString());
            //}



            //var s1 = "alias|key1";
            //var s2 = "key|default=25|alias=Item.name";
            //var s3 = "key";
            //var s4 = "alias";

            //var t = typeof(Channel.Define.CompileType.IntConverter);
            //var i = (new Channel.Define.CompileType.IntConverter() )as Channel.Define.CompileType.Converter;
            //var t2 = i.GetType();
            //Console.ReadKey();
            //var a = 0;
        }
    }
}
