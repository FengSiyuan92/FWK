using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Data;
namespace Channel
{
    public static partial class Lookup
    {

        public class DataObjGetter
        {
   
            internal Dictionary<string, DataObject> datas = new Dictionary<string, DataObject>(); 
            public DataObject this[object id]
            {
                get
                {
                    DataObject data = null;
                    datas.TryGetValue(id.ToString(), out data);
                    return data;
                }
            }

            internal void AddObject(DataObject obj)
            {
                var key = obj.KeyToString;
                if (datas.ContainsKey(key))
                {
                    CLog.LogError("重复向类型:{0} 中添加key为{1}的数据", obj.ClassName, obj.KeyToString);
                }
                datas.Add(key, obj);
            }
        }

        public class DataViewer
        {
            internal Dictionary<string, DataObjGetter> allDatas = new Dictionary<string, DataObjGetter>(); 

            public DataObjGetter this[string className]
            {
                get
                {
                    DataObjGetter getter = null;
                    allDatas.TryGetValue(className, out getter);
                    return getter;
                }
            }

            public string[] AllName()
            {
                return allDatas.Keys.ToArray();
            }
        }


        /// <summary>
        /// 内部通过该接口注册进一个解析过后的数据
        /// </summary>
        /// <param name="enumObj"></param>
        internal static void AddData(DataObject data)
        {
            lock (dataLock)
            {
                DataObjGetter getter = null;
                if (!Datas.allDatas.TryGetValue(data.ClassName, out getter))
                {
                    getter = new DataObjGetter();
                    Datas.allDatas.Add(data.ClassName, getter);
                }
                getter.AddObject(data);
            }
           
        }

        static object dataLock = new object();
        static DataViewer datasInstance= new DataViewer();

        /// <summary>
        /// 枚举查看器
        /// </summary>
        public static DataViewer Datas
        {
            get
            {
                return datasInstance;
            }
        }

    }
}
 