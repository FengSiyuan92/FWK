using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using System.IO;
using Channel.RawDefine;
using Channel.Data;

namespace Channel.Agent.Excel
{
    class ExcelAgent :IDisposable, IFileAgent
    {
        /// <summary>
        /// 有效数据表的标记符号
        /// </summary>

        enum FieldRowOrder
        {
            Name = 0,
            Type,
            DefaultValue,
            OutputType,
            AppendDef,
            CheckRule,
            Max
        }

        static readonly Dictionary<string, FieldRowOrder> TITLE_ORDER = new Dictionary<string, FieldRowOrder>()
        {
            {ConstString.EXL_FIELD_NAME_TITLE,  FieldRowOrder.Name},
            {ConstString.EXL_FIELD_TYPE_TITLE,  FieldRowOrder.Type},
            {ConstString.EXL_FIELD_OUTPUT_TYPE_TITLE,  FieldRowOrder.OutputType},
            {ConstString.EXL_FIELD_APPEND_TITLE,  FieldRowOrder.AppendDef},
            {ConstString.EXL_CHECK_RULE_TITLE,  FieldRowOrder.CheckRule},
        };
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
            return cell != null && cell.GetValue() == ConstString.VALID_SHEET_SIGN;
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

        string filePath;
        IWorkbook workBook;
 

        public bool Valid { get; private set; }

        public ExcelAgent(string filePath)
        {
            this.filePath = filePath;
            if (!IsValidExcelFile(filePath))
            {
                Valid = false;
                return;
            }
            try
            {
                FileStream file = new FileStream(filePath, System.IO.FileMode.Open,
                        System.IO.FileAccess.Read, FileShare.ReadWrite);
                workBook = WorkbookFactory.Create(file);
            }
            catch
            {
                Valid = false;
                return;
            }
        
            if (workBook == null)
            {
                Valid = false;
                return;
            }
            Valid = true;
        }

        /// <summary>
        /// 加载生成excel对应的ObjectDefine数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assign"></param>
        /// <returns></returns>
        public void LoadDefine()
        {
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
                        , filePath, sheet.SheetName, ConstString.EXL_FIELD_NAME_TITLE);
                    continue;
                }

                /*     数据校验合格,开始执行定义生成        */

                // 对sheet创建一个Object定义,并赋值Object名称
                RawObjDef objDef = new RawObjDef(Utils.GetObjectTypeName(filePath), RawObjType.OBJECT);
                // 使用表头行填充定义
                InjectObjectDef(objDef, rows);
                Lookup.AddRawDef(objDef);
            }
        }

        public void Dispose()
        {
            workBook.Close();
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
                    case ConstString.VALID_SHEET_SIGN:
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

        static void InjectObjectDef(RawObjDef def, Dictionary<FieldRowOrder, IRow> validRows)
        {
            IRow fieldNameRow = validRows[FieldRowOrder.Name];

            // i = 0 是title,遍历从1开始
            for (int i = 1; i < fieldNameRow.LastCellNum; i++)
            {
                var cell = fieldNameRow.GetCell(i);
                if (cell == null || string.IsNullOrEmpty(cell.GetValue())) continue;
                //创建字段定义并添加到object定义中
                RawFieldDef fDef = new RawFieldDef();

                // 开始执行定义赋值
                // 字段名称
                fDef.FieldName = cell.GetValue();
                def.AddFieldDefine(fDef);

                // 字段类型
                fDef.FieldType = GetAppiontCellValue(validRows[FieldRowOrder.Type], i);

                // 导出类型
                fDef.OutputType = GetAppiontCellValue(validRows[FieldRowOrder.OutputType], i);

                // 内容描述
                fDef.AppendDef = GetAppiontCellValue(validRows[FieldRowOrder.AppendDef], i);

                // 检查规则
                fDef.CheckRule = GetAppiontCellValue(validRows[FieldRowOrder.CheckRule], i);
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

                if (titleCell.GetValue() == ConstString.VALID_SHEET_SIGN)
                {
                    signCount++;
                    // 匹配结束
                    if (signCount == 2) break;
                }
                else if (titleCell.GetValue() == ConstString.EXL_FIELD_NAME_TITLE)
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

        public void LoadContent()
        {
            for (int i = 0; i < workBook.NumberOfSheets; i++)
            {
                ISheet sheet = workBook.GetSheetAt(i);
                // 不是有效数据表继续排查
                if (!IsValidSheet(sheet)) continue;

                // table 类型名称
                var tabName = Utils.GetObjectTypeName(filePath);
               

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
                    var firstCell = currentRow.GetCell(0);
                    if (firstCell!= null && firstCell.GetValue() == "#")
                    {
                        continue;
                    }

                    foreach (var title in titles)
                    {
                        var index = title.Key;
                        var fieldName = title.Value;
                        ICell cell = currentRow.GetCell(index);
                        var content = cell == null ? ConstString.STR_EMPTY : cell.GetValue();
                        titleAndContent.Add(fieldName, content);
                    }

                    DataObject data = new DataObject(tabName);
                    data.SetKV(titleAndContent);
                    Lookup.AddData(data);
                }
            }

        }

   
    }
}
