using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI;
using NPOI.SS.UserModel;
using System.IO;
using Channel.OutputDefine;

namespace Channel.Reader
{
    class ExcelReader
    {
        /// <summary>
        /// 有效数据表的标记符号
        /// </summary>
        const string VALID_SHEET_SIGN = "**";

        const string FieldNameTitle = "字段名";
        const string FieldTypeTitle = "类型";
        const string OutputTypeTitle = "导出";
        const string AppendDefTitle = "附加定义";
        const string CheckRuleTitle = "检查规则";
        enum FieldRowOrder
        {
            Name = 0,
            Type,
            OutputType,
            AppendDef,
            CheckRule, 
            Max
        }

        static readonly Dictionary<string, FieldRowOrder> TITLE_ORDER = new Dictionary<string, FieldRowOrder>()
        {
            {FieldNameTitle,  FieldRowOrder.Name},
            {FieldTypeTitle,  FieldRowOrder.Type},
            {OutputTypeTitle,  FieldRowOrder.OutputType},
            {AppendDefTitle,  FieldRowOrder.AppendDef},
            {CheckRuleTitle,  FieldRowOrder.CheckRule},
        };

        static Dictionary<string, ObjectDefine> allLoadedDef = new Dictionary<string, ObjectDefine>();

        /// <summary>
        /// 检查一个sheet表格是否是有效的数据表格
        /// 需要A1是VALID_SHEET_SIGN(**)
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        /// 
        static bool IsValidSheet(ISheet sheet)
        {
            if (sheet.LastRowNum == 0 ) return false;

            IRow row = sheet.GetRow(0);
            if (row.LastCellNum == 0) return false;

            ICell cell = row.GetCell(0);
            return cell.StringCellValue == VALID_SHEET_SIGN;
        }

        /// <summary>
        /// 加载一个xlsx表格的数据
        /// 加载完成后,将会生成excel对应的ObjectDefine数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assign"></param>
        /// <returns></returns>
        public static bool Load(string fileName)
        {
            IWorkbook workBook = CreateBook(fileName);
            if (workBook == null) return false;

            for (int i = 0; i < workBook.NumberOfSheets; i++)
            {
                ISheet sheet = workBook.GetSheetAt(i);
                // 不是有效数据表继续排查
                if (!IsValidSheet(sheet)) continue;

                var tabName = Utils.GetObjectTypeName(fileName);
                ObjectDefine objDef = new ObjectDefine();
                var rows = CollectFieldRows(sheet);


            }


            return true;
        }

        static Dictionary<FieldRowOrder, IRow> CollectFieldRows(ISheet sheet)
        {
            var rows = sheet.GetRowEnumerator();
            // 跳过A1的 '**'
            rows.MoveNext();
            // 这里主要是支持row的乱序,可以通过需求自定义字段头的顺序,以方便阅读
            // TODO: 池子
            Dictionary<FieldRowOrder, IRow> fieldRows = new Dictionary<FieldRowOrder, IRow>((int)FieldRowOrder.Max);

            // 遍历一次查到所有的有效行
            while (rows.MoveNext())
            {
                var currentRow = rows.Current as IRow;
                var title = currentRow.GetCell(0).StringCellValue;

                var skip = false;
                switch (title)
                {
                    // 空行继续
                    case "": case " ": break;
                    // 结束符
                    case VALID_SHEET_SIGN:
                        skip = true;
                        break;
                    default:
                        FieldRowOrder order = default(FieldRowOrder);
                        if (TITLE_ORDER.TryGetValue(title, out order)) fieldRows.Add(order, currentRow);
                        else throw new Exception("未支持的解析类型行");
                        break;

                }
                if (skip) break;
            }

            return fieldRows;
        }


        static void InjectObjectDef(string name, ObjectDefine def, ISheet sheet)
        {
            var max = (int)FieldRowOrder.Max;
            for (int i = 0; i < max; i++)
            {
                FieldRowOrder order = (FieldRowOrder)i;
                IRow row;
                if (!fieldRows.TryGetValue(order, out row)) continue;
                switch (order)
                {
                    case FieldRowOrder.Name:
                        break;
                    case FieldRowOrder.Type:
                        break;
                    case FieldRowOrder.OutputType:
                        break;
                    case FieldRowOrder.AppendDef:
                        break;
                    case FieldRowOrder.CheckRule:
                        break;
                    case FieldRowOrder.Max:
                        break;
                    default:
                        break;
                }
            }
        }

        static void CreateAndFillFieldDefine(IRow fieldNameRow, ObjectDefine def)
        {
            // i = 0 是title,遍历从1开始
            for (int i = 1; i < fieldNameRow.LastCellNum; i++)
            {
                var cell = fieldNameRow.GetCell(i);
                if (cell != null && string.IsNullOrWhiteSpace(cell.StringCellValue))
                {
                    FieldDefine fDef = new FieldDefine();
                    fDef.fieldName = cell.StringCellValue;
                    def.AddFieldDefine(fDef);
                }
            }
        }


        /// <summary>
        /// 尝试创建一个workBook对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static IWorkbook CreateBook(string fileName)
        {
            // 临时文件不读取
            var f = Path.GetFileName(fileName);
            if (f.StartsWith("~$") || f.StartsWith("~"))
            {
                return null;
            }
            // 参数传递进来的 忽略文件不读取
            if (GlobalArgs.IsIgnoreFile(fileName))
            {
                return null;
            }
            // TODO:多线程处理,加入try,catch处理
            // 使用io共享打开
            IWorkbook workBook = null;
            using (FileStream file = new FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                workBook = WorkbookFactory.Create(file);
            }
            return workBook;
        }

    }
}
