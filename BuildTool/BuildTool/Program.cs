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

        static void Main(string[] args)
        {
            //ExcelReader.LoadObjectDefine(ItemPart1);
            //ExcelReader.LoadObjectDefine(ItemPart2);
            //ExcelReader.LoadObjectDefine(TestUseItempath);
            //XMLReader.LoadEnumDefine(EnumXML);
            //XMLReader.LoadObjectDefine(ObjXML);

            // Lookup.InitDefine();

            //ExcelReader.LoadObjectConent(ItemPart1);
            //Lookup.Test();
            //ExcelReader.LoadObjectConent(ItemPart2);
            //Lookup.Test();

            //ExcelReader.LoadObjectConent(TestUseItempath);
            //Lookup.Test();

            FileAgent.RegisterFile(EnumXML);
            FileAgent.RegisterFile(ItemPart1);


            Compiler.StartCompile();


            var allEnums = Lookup.LookAllEnumName();
            foreach (var enumName in allEnums)
            {
                var e = Lookup.LookEnum(enumName);
                var fieldName = e.GetAllItemName();
                foreach (var item in fieldName)
                {
                    CLog.LogError(e.GetItemByFieldName(item).ToString());
                }
            }

            foreach (var enumName in allEnums)
            {
                var e = Lookup.LookEnum(enumName);
                CLog.Log(e.ToString());
            }
            Console.ReadKey();
            //var a = 0;
        }


        
    }
}
