using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.OutputDefine;

namespace Channel.Data
{
    class ObjectDataCompare : IEqualityComparer<ObjectData>
    {
        public static ObjectDataCompare compare = new ObjectDataCompare();
        public bool Equals(ObjectData x, ObjectData y)
        {
            return !(x.Key1 != y.Key1 || x.Key2 != y.Key2);
        }

        public int GetHashCode(ObjectData obj)
        {
            return 1;
        }
    }

    public class Table :IEnumerable< ObjectData>
    {
        public string Name;

        ObjectDefine define;
        
        internal Table(string name)
        {
            this.Name = name;
            this.define = Lookup.LookObjectDefine(name);
        }

        public List<ObjectData> commonData { get; internal set; } = new List<ObjectData>();


        public IEnumerator<ObjectData> GetEnumerator()
        {
            return commonData.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        internal void AddObjectData(Dictionary<string, string> titleAndContent)
        {
            //创建一个数据对象 映射一条数据
            ObjectData objData = new ObjectData();
            objData.owner = this;
            // 遍历所有的字段,为数据赋值
            foreach (var item in titleAndContent)
            {
                ObjectData.Field field = new ObjectData.Field();
                var fieldDefine = define[item.Key] as FieldDefine;
                // 字段定义
                field.fieldDefine = fieldDefine;
                // 字段原始内容
                field.originalValue = item.Value;

                objData.AddField(field);
            }
            commonData.Add(objData);
        }

        internal void Merge(Table otherTable)
        {
            commonData.Union<ObjectData>(otherTable.commonData, ObjectDataCompare.compare);
        }

 
    }
}
