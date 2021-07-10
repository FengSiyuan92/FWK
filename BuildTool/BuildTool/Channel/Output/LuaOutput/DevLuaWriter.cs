using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Channel.Define;

namespace Channel.Output.LuaOutput
{
    internal class DevLuaWriter
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
            var targetEnumFilePath = Path.Combine(ConstString.TempPath, "DevLua", "Enum.lua");
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
            if (objClassName == "CustomTest")
            {
                var a = 0;
            }
            var getter = Lookup.Datas[objClassName];
            if (!getter.KeyTable) return;

            var sb = new StringBuilder();
            sb.AppendLine("-- 工具生成的dev版本lua代码,手动修改重新生成时将会被覆盖");
            sb.AppendFormat("local {0} = ", objClassName);
            sb.AppendLine("{");
            var datas = getter.GetAllSortedDatas();
            foreach (var data in datas)
            {
                var k = data.ClassInfo.KeyField;
                var key = data[k.FieldName];

  
                sb.AppendLine(string.Format("\t[{0}] = {1},",
                   GetFieldString(key),
                   GetObjInfo(data, 1)));
            }
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("}");
            sb.AppendLine("");

            sb.AppendLine(string.Format("return {0}", objClassName));

            var targetEnumFilePath = Path.Combine(ConstString.TempPath, "DevLua", objClassName+".lua");
            var file = FileUtils.SafeCreateNewFile(targetEnumFilePath);
            file.Write(sb.ToString());
            file.Flush();
            file.Close();
        }

        static string GetObjInfo(DataObject data, int depth = 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            var pre = CharRepeat('\t', depth);

            var dt = data.GetAllSortedField();
            foreach (var item in dt)
            {
                var v = GetFieldString(item.Value, depth + 1);

                var k = item.Key;
                var alias = data.ClassInfo[k].Alias;
                if (!string.IsNullOrEmpty(alias))
                {
                    k = string.Format("{0}--[[1]]", k, alias);
                }

                if (v != null)
                {
                    sb.AppendLine(pre + "\t" + k + " = " + v + ",");
                }
               
            }
            sb.Append(pre + "}");

            var res = sb.ToString();
            return res;
        }

        static string GetFieldString(object v, int depth = 0, bool focus = false)
        {
            var pre = CharRepeat('\t', depth);
            var vcontent = focus ? pre: "";
            // TODO: 改为switch case
            if (v == null)
            {
                return null;
            }
            if (v is Vector2)
            {
                var vec = v as Vector2;
                vcontent += "{" + string.Format("x={0}, y={1}", vec.x.ToString(), vec.y.ToString()) + "}";
            }
            else if (v is Vector3)
            {
                var vec = v as Vector3;
                vcontent += "{"+ string.Format("x = {0}, y = {1}, z = {2}",
                    vec.x.ToString(), vec.y.ToString(), vec.z.ToString()) + "}";
            }
            else if (v is Vector4)
            {
                var vec = v as Vector4;
                vcontent += "{"+ string.Format("x = {0}, y = {1}, z = {2}, w = {3}",
                    vec.x.ToString(), vec.y.ToString(), vec.z.ToString(), vec.w.ToString()) + "}";
            }
            else if (v is DataArray)
            {
                var array = v as DataArray;
                if (array.Length == 0)
                {
                    vcontent = "{}";
                }
                else
                {
                    vcontent += "{\n";
                    var cpre = pre + '\t';
                    for (int i = 0; i < array.Length; i++)
                    {
                        vcontent += GetFieldString(array[i], depth + 1, true) + ",\n";
                    }
                    vcontent += pre + "}";
                }
            }
            else if (v is DataObject)
            {
                var data = v as DataObject;
                vcontent += GetObjInfo(data, depth);
            }
            else if (v is string)
            {
                var vs = v as string;
                var c1 = vs.Contains("\"");
                var c2 = vs.Contains("\'");
                if (c1 && c2)
                {
                    vcontent += "[[" + vs + "]]";

                }
                else if (c1)
                {
                    vcontent += "\'" + vs + "\'";
                }
                else if (c2)
                {
                    vcontent += "\"" + vs + "\"";
                }
                else
                {
                    vcontent += "\"" + vs + "\"";
                }
            }
            else
            { 
                vcontent += v.ToString();
            }

            return vcontent;
        }

        static string CharRepeat(char c, int repeat)
        {
            var res = "";
            for (int i = 0; i < repeat; i++)
            {
                res += c;
            }
            return res;
        }
    }
}
