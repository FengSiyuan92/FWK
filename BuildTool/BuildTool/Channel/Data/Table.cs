using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.RawDefine;

namespace Channel.Data
{
    public class Table  :ITable
    {
        public string Name { get; private set; }
        public string Path { get; private set; }
        public ITable MainTable { get; private set; }
        public List<ITable> SubTable { get; private set; }
        RawObjDef define;

        internal Table(string name, string path)
        {
            this.MainTable = this;
            this.Name = name;
            this.define = Lookup.LookupRawObjDef(name) as RawObjDef;
            this.Path = path;
        }

        Dictionary<string, ObjectData> datas = new Dictionary<string, ObjectData>();

        List<Table> subTable;

        /// <summary>
        /// 向table中添加一条数据
        /// </summary>
        /// <param name="titleAndContent"></param>
        internal void AddObjectData(Dictionary<string, string> titleAndContent)
        {
            //创建一个数据对象 映射一条数据
            ObjectData objData = new ObjectData();
            objData.owner = this;
            // 遍历所有的字段,为数据赋值
            foreach (var item in titleAndContent)
            {
                ObjectData.Field field = new ObjectData.Field();
                var fieldDefine = define[item.Key] as RawObjFieldDef;
                // 字段定义
                field.fieldDefine = fieldDefine;
                // 字段原始内容
                field.originalValue = item.Value;

                objData.AddField(field);
            }

            if (datas.ContainsKey(objData.Key))
            {
                CLog.LogError("{0}表格中存在着两个key(id?)相同的两条配置 key = {1}.", Path, objData.Key);
                return;
            }

            datas.Add(objData.Key, objData);
        }

        internal void Merge(Table otherTable)
        {
            otherTable.MainTable = this;
            if (subTable == null)
            {
                SubTable = new List<ITable>();
            }
            SubTable.Add(otherTable);

            // 检查主表和子表中是否有相同的key值(id)
            var inter = datas.Keys.Intersect(otherTable.datas.Keys);
            foreach (var item in inter)
            {
                CLog.LogError("表{0}和表{1},存在着相同key(={2})的配置项,需要删除一条,否则可能会被覆盖",
                    Path, otherTable.Path, item);
            }
        }

 
    }
}
