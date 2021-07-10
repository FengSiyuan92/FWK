using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Channel
{
    internal class FileUtils
    {
        public static void FindDirValidFile(string dirpath, HashSet<string> vessel, params string[] validExtend)
        {
            DirectoryInfo dir = new DirectoryInfo(dirpath);

            for (int i = 0; i < validExtend.Length; i++)
            {
                var files = dir.GetFiles(validExtend[i], SearchOption.AllDirectories).Select(f => f.FullName);
                vessel.UnionWith(files);
            }
        }

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


        internal static StreamWriter SafeCreateNewFile(string filePath)
        {
            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return new FileInfo(filePath).CreateText();
        }
    }
}
