using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel;
using Channel.Reader;
using Channel.OutputDefine;

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
            ExcelReader.LoadObjectDefine(ItemPart1);
            ExcelReader.LoadObjectDefine(ItemPart2);
            ExcelReader.LoadObjectDefine(TestUseItempath);
            XMLReader.LoadEnumDefine(EnumXML);
            XMLReader.LoadObjectDefine(ObjXML);
            Lookup.Test();

            var a = 0;
        }
    }
}
