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


            //FileAgent.RegisterFile(EnumXML);
            //FileAgent.RegisterFile(ItemPart1);


            //Compiler.StartCompile();


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





            var a = GetAlias("alias|key1");
            var b = GetAlias("key|alias=Item.name");
            var c = GetAlias("key");
            var d = GetAlias("alias");

            Console.ReadKey();
            //var a = 0;
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
