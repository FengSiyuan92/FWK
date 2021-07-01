using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;

namespace Channel.Agent.Excel
{
   internal static class Extend
   {
        public static string GetValue(this ICell cell)
        {
            return cell.ToString();
        }
    }
}
