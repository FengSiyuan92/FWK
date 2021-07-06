using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPOI.SS.UserModel;
using System.IO;
namespace Channel
{
    public class Utils
    {
        public static string GetObjectTypeName(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var subTabSignIndex = fileName.IndexOf("(");
            if (subTabSignIndex != -1)
            {
                fileName = fileName.Substring(0, subTabSignIndex);
            }
            return fileName;
        }

        public static decimal ParseFloat(string content)
        {
            if (content.Equals(ConstString.STR_EMPTY) || string.IsNullOrEmpty(content))
            {
                return 0M;
            }
            var res = 0M;
            if (!decimal.TryParse(content, out res))
            {
                CLog.LogError("尝试把{0}转换成数值并失败", content);
            }
            return res;
        }

        static char[] seps = new char[]
        {
            ConstString.SEP_LEVEL_1,
            ConstString.SEP_LEVEL_2,
            ConstString.SEP_LEVEL_3,
        };


        internal static char GetCustomSep(Define.Field field, int depth, char defaultSep = ConstString.SEP_LEVEL_2)
        {
            if (field.Seps!= null && field.Seps.Length > depth)
            {
                return field.Seps[depth];
            }
            return defaultSep;
        }


       public static string TrimSign(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return ConstString.STR_EMPTY;
            }
            var length = content.Length;
            var start = 0;
            var signCount = 0;
            if (content[content.Length - 1] == ConstString.GROUP_SIGN_RIGHT)
            {
                length -= 1;
                signCount = signCount + 1;
            }
            if (content[0] == ConstString.GROUP_SIGN_LEFT)
            {
                signCount = signCount + 1;
                length -= 1;
                start = 1;
            }
            if (signCount == 2)
            {
                return content.Substring(start, length);
            }
            return content;
        }

        // todo: 使用该函数的自己提供一个list,从池子里取,用完了clear
        public static List<string> Split(string content, char sep)
        {
            int index = 0;
            List<string> res = new List<string>();
            bool cansub = true;
            for (int i = 0; i < content.Length; i++)
            {
                var c = content[i];
                if (c == ConstString.GROUP_SIGN_LEFT)
                {
                    cansub = false;
                }
                else if (c == ConstString.GROUP_SIGN_RIGHT)
                {
                    cansub = true;
                }
                else if (c == sep && cansub)
                {
                    if (index == i)
                    {
                        res.Add(ConstString.STR_EMPTY);
                    }
                    else
                    {
                        res.Add(content.Substring(index, i - index));
                    }
                    index = i + 1;
                }
            }

            if (index <content.Length)
            {
                res.Add(content.Substring(index, content.Length- index));
            }
            else if (index == content.Length)
            {
                res.Add(ConstString.STR_EMPTY);
            }
            return res;
        }

    }
}
