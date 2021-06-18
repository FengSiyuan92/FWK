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
    public static class ExcelReader
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
            var s = cell.ToString();
            return cell!= null && cell.GetValue() == VALID_SHEET_SIGN;
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


        static string GetValue(this ICell cell)
        {
            return cell.ToString();
        }




        static Dictionary<FieldRowOrder, IRow> CollectFieldRows(ISheet sheet)
        {
            var rows = sheet.GetRowEnumerator();
            // 跳过A1的 '**'
            rows.MoveNext();
            
            // TODO: 池子
            Dictionary<FieldRowOrder, IRow> fieldRows = new Dictionary<FieldRowOrder, IRow>((int)FieldRowOrder.Max);

            // 遍历一次查到所有的有效行
            while (rows.MoveNext())
            {
                var currentRow = rows.Current as IRow;
                if (currentRow == null || currentRow.LastCellNum <= 0) continue;

                var titleCell = currentRow.GetCell(0);
                if (titleCell == null || string.IsNullOrEmpty(titleCell.GetValue())) continue;

                var title = titleCell.GetValue(); ;

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
                        else throw new Exception("表头未支持的解析类型行:" + title);
                        break;

                }
                if (skip) break;
            }

            return fieldRows;
        }

        static void InjectObjectDef(ObjectDefine def, Dictionary<FieldRowOrder, IRow> validRows)
        {
            IRow fieldNameRow = validRows[FieldRowOrder.Name];
      

            // i = 0 是title,遍历从1开始
            for (int i = 1; i < fieldNameRow.LastCellNum; i++)
            {
                var cell = fieldNameRow.GetCell(i);
                if (cell == null || string.IsNullOrEmpty(cell.GetValue())) continue;
                //创建字段定义并添加到object定义中
                FieldDefine fDef = new FieldDefine();
               
                // 开始执行定义赋值
                // 字段名称
                fDef.FieldName = cell.GetValue();
                def.AddFieldDefine(fDef);

                // 字段类型
                fDef.fieldType = GetAppiontCellValue(validRows[FieldRowOrder.Type], i);

                // 导出类型
                fDef.outType = GetAppiontCellValue(validRows[FieldRowOrder.OutputType], i);

                // 内容描述
                fDef.valueAppend = GetAppiontCellValue(validRows[FieldRowOrder.AppendDef], i);

                // 检查规则
                fDef.checkRule = GetAppiontCellValue(validRows[FieldRowOrder.CheckRule], i);
            }
        }
        /// <summary>
        /// 获取指定索引对应的单元格中的值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        static string GetAppiontCellValue(IRow row, int index, string defaultValue = "")
        {
            if (row == null) return defaultValue;
            var contentCell = row.GetCell(index);
            string value = contentCell != null ? contentCell.GetValue() : string.Empty;
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


                    // 收集所有有效数据行(**start--------end**)
                    // 这里主要是支持表头的乱序,可以通过需求自定义字段头的顺序,以方便阅读
                    var rows = CollectFieldRows(sheet);

                    // 使用收集好的IRow数据,为ObjectDef填充数据
                    // 检查是否有字段名的一行,没有的话没法执行
                    IRow fieldNameRow = rows[FieldRowOrder.Name];
                    if (fieldNameRow == null)
                    {
                        CLog.LogError("{0}@{1}电子表格中的表头没有有效的字段名标志符,需要在对应字段名行的第一列中声明'{2}'"
                            , filePath, sheet.SheetName, FieldNameTitle);
                        continue;
                    }

                    /*     数据校验合格,开始执行定义生成        */

                    // 对sheet创建一个Object定义,并赋值Object名称
                    ObjectDefine objDef = new ObjectDefine();
                    objDef.Name = Utils.GetObjectTypeName(filePath);
                    // 使用表头行填充定义
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
                if (titleCell == null || string.IsNullOrEmpty(titleCell.GetValue())) continue;

                if (titleCell.GetValue() == VALID_SHEET_SIGN)
                {
                    signCount++;
                    // 匹配结束
                    if (signCount == 2) break;
                }
                else if (titleCell.GetValue() == FieldNameTitle)
                {
                    // 注册int和字段名
                    for (int i = 1; i < row.LastCellNum; i++)
                    {
                        var cell = row.GetCell(i);
                        if (cell == null) continue;
                        var value = cell.GetValue();
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
                    Table table = new Table(tabName, filePath+"@" + sheet.SheetName);

                    // 选择标题头,并跳过表头信息
                    var rows = sheet.GetRowEnumerator();
                    var titles = SkipAndSelectTitle(rows);
                    //逐行将excel组织成key value的形式,key为title value为对应的内容
                    Dictionary<string, string> titleAndContent =
                         new Dictionary<string, string>(titles.Count);
                    while (rows.MoveNext())
                    {
                        titleAndContent.Clear();
                        var currentRow = rows.Current as IRow;
       
                        for (int j = 1; j < currentRow.LastCellNum; j++)
                        {
                            string fieldName = string.Empty;
                            ICell cell = currentRow.GetCell(j);
                            if (cell == null || !titles.TryGetValue(j, out fieldName)) continue;
                            titleAndContent.Add(fieldName, cell.GetValue());
                        }
                        table.AddObjectData(titleAndContent);
                    }
                    Lookup.AddTable(table);
                }
                workBook.Close();
            }
            return true;
        }
    }
}
