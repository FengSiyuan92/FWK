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

        static void Main(string[] args)
        {


            FileAgent.RegisterFile(EnumXML);
            FileAgent.RegisterFile(TypeTestPath);

            Compiler.StartCompile();

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
