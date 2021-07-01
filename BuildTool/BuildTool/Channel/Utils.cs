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

    }
}
