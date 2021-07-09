using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPOI.SS.UserModel;
using System.IO;
using System.Threading;
using Channel.Define;
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

        public static decimal ParseFloat(string content, string source = ConstString.STR_EMPTY)
        {
            if (content.Equals(ConstString.STR_EMPTY) || string.IsNullOrEmpty(content))
            {
                return 0M;
            }
            var res = 0M;
            if (!decimal.TryParse(content, out res))
            {
                CLog.LogError("尝试把'{0}'转换成数值并失败 => {1}", content, source);
            }
            return res;
        }

        static char[] seps = new char[]
        {
            ConstString.SEP_LEVEL_1,
            ConstString.SEP_LEVEL_2,
            ConstString.SEP_LEVEL_3,
        };


        internal static char GetCustomSep(Field field, int depth, char defaultSep = ConstString.SEP_LEVEL_2)
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

        /// <summary>
        /// 从一个容器中检查是否存在相同的配置数据,比如Vector\和DataObject,即使不是同一个引用,也有可能是完全相等内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="vessel"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static bool EqualsContains<T>(IEnumerable<T> vessel, T a)
        {
            foreach (var item in vessel)
            {
                if (a == null)
                {
                    if (item == null) return true;
                }
                else
                {
                    if (a.Equals(item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static HashSet<char> valieFieldNameChar = new HashSet<char>();
   
        public static bool FieldNameValid(string fieldName)
        {
            fieldName = fieldName.Trim();
            if (string.IsNullOrEmpty(fieldName))
            {
                return false;
            }

            for (int i = 0; i < fieldName.Length; i++)
            {
                var c = fieldName[i];
                if (!valieFieldNameChar.Contains(c))
                {
                    CLog.LogError("字段名包含违法字符'{0}'", c);
                    return false;
                }
            }

            if ( fieldName[0] <= 57 && fieldName[0] >= 48)
            {
                CLog.LogError("字段名'{0}'不能以数值开头",fieldName);
                return false;
            }
            return true;
        }

        internal static void Parallel<T>(IEnumerable<T> items, Action<T> action)
        {
            if (GlobalArgs.ASYNC)
            {
                var resetEvents = new List<ManualResetEvent>();
                foreach (var item in items)
                {
                    var evt = new ManualResetEvent(false);
                    resetEvents.Add(evt);
                    ThreadPool.QueueUserWorkItem((i) =>
                    {
                        action((T)i);
                        evt.Set();
                    }, item);
                }
                WaitHandle.WaitAll(resetEvents.ToArray());
                Thread.CurrentThread.Priority = ThreadPriority.Normal;
            }
            else
            {
                foreach (var item in items)
                {
                    action(item);
                }
            }
        }


        static Utils()
        {
            for (int i = 'a'; i <= 'z'; i++)
            {
                valieFieldNameChar.Add((char)i);
            }
            for (int i = 'A'; i <= 'Z'; i++)
            {
                valieFieldNameChar.Add((char)i);
            }
            for (int i = '0'; i <= '9'; i++)
            {
                valieFieldNameChar.Add((char)i);
            }

            valieFieldNameChar.Add('_');
        }
    }
}
