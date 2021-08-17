using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Channel.Output.LuaOutput
{
    class LuaWriter
    {
        static string targetPath;
        public static void Write(string targetDirPath)
        {
            targetPath = targetDirPath;
            CLog.Log("临时路径={0}", ConstString.TempPath);

            GenEnum();
            GenObjs();
        }

        static void GenEnum()
        {
            var sb = new StringBuilder();
            sb.AppendLine("-- 工具生成的dev版本lua代码,手动修改重新生成时将会被覆盖");
            sb.AppendLine("local Enum = {}");
            sb.AppendLine("");
            var allEnum = Lookup.Enum.AllName();
            foreach (var eName in allEnum)
            {
                sb.AppendFormat(string.Format("Enum.{0} = ", eName));
                sb.AppendLine("{");

                var e = Lookup.Enum[eName];
                var items = e.GetAllSortedItem();
                foreach (var item in items)
                {
                    var alias = item.Alias;
                    if (string.IsNullOrEmpty(alias))
                    {
                        sb.AppendLine(string.Format("    {0}\t\t=  {1}", item.Name, item.Value));
                    }
                    else
                    {
                        sb.AppendLine(string.Format("    {0}\t\t=  {1}\t-- {2}",
                            item.Name, item.Value, item.Alias));
                    }
                }

                sb.AppendLine("}");
                sb.AppendLine("");
            }

            sb.Append("return Enum");
            var targetEnumFilePath = Path.Combine(ConstString.TempPath, "Lua", "Enum.lua");
            var file = FileUtils.SafeCreateNewFile(targetEnumFilePath);
            file.Write(sb.ToString());
            file.Flush();
            file.Close();
        }

        static void GenObjs()
        {
            var custom = Lookup.Datas.AllName();

            Utils.Parallel(custom, GenSingleObj);
        }

        static void GenSingleObj(string objClassName)
        {
            var getter = Lookup.Datas[objClassName];
            if (!getter.KeyTable) return;
            var sb = new StringBuilder();

            sb.AppendLine("-- 工具生成的发布版本lua代码,手动修改重新生成时将会被覆盖");

        }
    }
}
