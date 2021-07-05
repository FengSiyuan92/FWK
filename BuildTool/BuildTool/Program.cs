using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel;
using System.Text.RegularExpressions;

namespace BuildTool
{
    class Program
    {

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


        static void Main(string[] args)
        {


            FileAgent.RegisterFile(EnumXML);
            //FileAgent.RegisterFile(IntTest);
            //FileAgent.RegisterFile(FloatTest);
            FileAgent.RegisterFile(StringTest);

            Compile.StartCompile();

            Parse.StartParse();

      
            //var res1 = Lookup.Datas["IntTest"][1];
            //var res2 = Lookup.Datas["IntTest"][2];
            //var res3 = Lookup.Datas["IntTest"][3];

            //var f1 = Lookup.Datas["FloatTest"][1];
            //var f2 = Lookup.Datas["FloatTest"][2];
            //var f3 = Lookup.Datas["FloatTest"][3];


            var s1 = Lookup.Datas["StringTest"]["第一个"];
            var s2 = Lookup.Datas["StringTest"]["第二个"];
            var s3 = Lookup.Datas["StringTest"]["第三个"];

            var a = 0;


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
            Console.ReadKey();
            //var a = 0;
        }



    }
}
