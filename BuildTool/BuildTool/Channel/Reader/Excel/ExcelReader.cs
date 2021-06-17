using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI;
using NPOI.SS.UserModel;
using System.IO;
using Channel.OutputDefine;
using Channel.Data;
using Table = Channel.Data.Table;

namespace Channel.Reader
{
    public class ExcelReader
    {
        /// <summary>
        /// 有效数据表的标记符号
        /// </summary>
        const string VALID_SHEET_SIGN = "**";

        const string FieldNameTitle = "字段名";
        const string FieldTypeTitle = "类型";
        const string OutputTypeTitle = "导出";
        const string ValueAppendTitle = "内容描述";
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
            {ValueAppendTitle,  FieldRowOrder.AppendDef},
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
            // 空表
            if (sheet == null || sheet.LastRowNum == 0) return false;

            //第一行没有数据
            IRow row = sheet.GetRow(0);
            if (row == null || row.LastCellNum == 0) return false;

            //第一行数据是否为可用表的标志符号
            ICell cell = row.GetCell(0);
            return cell!= null && cell.StringCellValue == VALID_SHEET_SIGN;
        }

        /// <summary>
        /// 检查文件路径是否是有效的excel文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static bool IsValidExcelFile(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }
            // 临时文件不读取
            var f = Path.GetFileName(path);
            if (f.StartsWith("~$") || f.StartsWith("~"))
            {
                return false;
            }
            // 参数传递进来的 忽略文件不读取
            if (GlobalArgs.IsIgnoreFile(path))
            {
                return false;
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

        static void InjectObjectDef(ObjectDefine def, Dictionary<FieldRowOrder, IRow> validRows)
        {
            IRow fieldNameRow = validRows[FieldRowOrder.Name];
            if (fieldNameRow == null) throw new Exception("没有查找到定义字段名称的有效行");

            // i = 0 是title,遍历从1开始
            for (int i = 1; i < fieldNameRow.LastCellNum; i++)
            {
                var cell = fieldNameRow.GetCell(i);
                if (cell != null && string.IsNullOrEmpty(cell.StringCellValue)) continue;
                //创建字段定义并添加到object定义中
                FieldDefine fDef = new FieldDefine();
               
                // 开始执行定义赋值
                // 字段名称
                fDef.FieldName = cell.StringCellValue;
                def.AddFieldDefine(fDef);

                // 字段类型
                fDef.fieldType = GetCellValue(validRows[FieldRowOrder.Type], i);

                // 导出类型
                fDef.outType = GetCellValue(validRows[FieldRowOrder.OutputType], i);

                // 内容描述
                fDef.valueAppend = GetCellValue(validRows[FieldRowOrder.AppendDef], i);

                // 检查规则
                fDef.checkRule = GetCellValue(validRows[FieldRowOrder.CheckRule], i);
            }
        }

        static string GetCellValue(IRow row, int index, string defaultValue = "")
        {
            if (row == null) return defaultValue;
            var contentCell = row.GetCell(index);
            string value = contentCell != null ? contentCell.StringCellValue : string.Empty;
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }
        /// <summary>
        /// 加载生成excel对应的ObjectDefine数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assign"></param>
        /// <returns></returns>
        public static bool LoadObjectDefine(string filePath)
        {

            if (!IsValidExcelFile(filePath)) return false;
            // TODO:多线程处理,加入try,catch处理
            // 使用io共享打开
            IWorkbook workBook = null;
            using (FileStream file = new FileStream(filePath, System.IO.FileMode.Open,
                System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                workBook = WorkbookFactory.Create(file);
                for (int i = 0; i < workBook.NumberOfSheets; i++)
                {
                    ISheet sheet = workBook.GetSheetAt(i);
                    // 不是有效数据表继续排查
                    if (!IsValidSheet(sheet)) continue;

                    // 对sheet创建一个Object定义,赋值Object名称
                    ObjectDefine objDef = new ObjectDefine();
                    objDef.Name = Utils.GetObjectTypeName(filePath);

                    // 收集所有有效数据行(**start--------end**)
                    var rows = CollectFieldRows(sheet);
                    // 使用收集好的IRow数据,为ObjectDef填充数据
                    InjectObjectDef(objDef, rows);

                    Lookup.AddObjectDefine(objDef);
                }
                workBook.Close();
            }
            return true;
        }

        // 跳过标题行,并且搜集下来标题的信息数据.用做和当前表格做对照
        static Dictionary<int, string> SkipAndSelectTitle(System.Collections.IEnumerator rows)
        {
            Dictionary<int, string> title = new Dictionary<int, string>();

            int signCount = 0;

            while (rows.MoveNext())
            {
                var row = rows.Current as IRow;

                // 尝试分析是否是
                var titleCell = row.GetCell(0);
                // 跳过空行(可能是策划的备注行)
                if (titleCell == null || string.IsNullOrEmpty(titleCell.StringCellValue)) continue;

                if (titleCell.StringCellValue == VALID_SHEET_SIGN)
                {
                    signCount++;
                    // 匹配结束
                    if (signCount == 2) break;
                }
                else if (titleCell.StringCellValue == FieldNameTitle)
                {
                    // 注册int和字段名
                    for (int i = 1; i < row.LastCellNum; i++)
                    {
                        var cell = row.GetCell(i);
                        var value = cell.StringCellValue;
                        if (string.IsNullOrEmpty(value) || value.StartsWith("#")) continue;
                        title.Add(i, value);
                    }
                }     
            }

            return title;
        }


        public static bool LoadObjectConent(string filePath)
        {
            if (!IsValidExcelFile(filePath)) return false;
            // TODO:多线程处理,加入try,catch处理
            // 使用io共享打开
            IWorkbook workBook = null;
            using (FileStream file = new FileStream(filePath, System.IO.FileMode.Open,
                System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                workBook = WorkbookFactory.Create(file);
                for (int i = 0; i < workBook.NumberOfSheets; i++)
                {
                    ISheet sheet = workBook.GetSheetAt(i);
                    // 不是有效数据表继续排查
                    if (!IsValidSheet(sheet)) continue;
                    
                    // table 类型名称
                    var tabName = Utils.GetObjectTypeName(filePath);
                    // 创建一个table,引用ContentObject的定义
                    Table table = new Table(tabName);

                    // 选择标题头,并跳过表头信息
                    var rows = sheet.GetRowEnumerator();
                    var titles = SkipAndSelectTitle(rows);
                    while (rows.MoveNext())
                    {
                        var currentRow = rows.Current as IRow;
                        for (int j = 1; j < currentRow.LastCellNum; j++)
                        {
                            string fieldName = string.Empty;
                            ICell cell = currentRow.GetCell(i);
                            if (cell == null || !titles.TryGetValue(j, out fieldName)) continue;
                            // 将配置表的内容注入到table中
                            table.AddConent(fieldName, cell.StringCellValue);
                        }
                    }
                    Lookup.AddTable(table);
                }
                workBook.Close();
            }
            return true;
        }
    }
}
