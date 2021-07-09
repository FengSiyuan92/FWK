using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Channel
{
    namespace Channel.Viewer
    {
        public class DataObjGetter
        {
            internal Dictionary<string, DataObject> datas = new Dictionary<string, DataObject>();
            internal List<DataObject> datasWithoutKey = new List<DataObject>();

            /// <summary>
            /// 通过key值获取具体数据对象
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public DataObject this[object id] =>
                KeyTable ? GetDataByKey(id) : GetDataByIndex(int.Parse(id.ToString()));

            /// <summary>
            /// 通过key值(数据的唯一标志)来获取对应的数据实例,该接口可以直接使用索引器代替
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public DataObject GetDataByKey(object key)
            {
                DataObject data = null;
                datas.TryGetValue(key.ToString(), out data);
                return data;
            }

            /// <summary>
            /// 通过索引来获取对应的数据实例,只适用于没有key值的数据,否则会返回null
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public DataObject GetDataByIndex(int index)
            {
                if (index >= datasWithoutKey.Count)
                {
                    return null;
                }
                return datasWithoutKey[index];
            }

            internal void AddObject(DataObject obj)
            {
                var key = obj.KeyToString;
                if (string.IsNullOrEmpty(key))
                {
                    datasWithoutKey.Add(obj);
                }
                else
                {
                    if (datas.ContainsKey(key))
                    {
                        CLog.LogError("重复向类型:{0}中添加key为{1}的数据,数据来源分别是=>\n\t{2}\n\t{3}",
                            obj.ClassName, obj.KeyToString,
                            datas[key].Source(), obj.Source());
                        return;
                    }
                    datas.Add(key, obj);
                }
            }

            /// <summary>
            /// 该类型数据实例对象是否有一个唯一key值,如果该值返回true,
            /// 则可以通过key值来索引对应数据对象
            /// </summary>
            public bool KeyTable
            {
                get
                {
                    return datas.Count != 0;
                }
            }

            /// <summary>
            /// 获取所有该类型的实例数据对象
            /// </summary>
            /// <returns></returns>
            public DataObject[] AllDatas()
            {
                return KeyTable ? datas.Values.ToArray() : datasWithoutKey.ToArray();
            }
        }
        /// <summary>
        /// 通过k-v形式缓存了所有已经生成的数据对象, 其中k是对应数据对象实例的类型名称,v是对应类型的数据对象实例查看器
        /// </summary>
        public class DataViewer
        {
            internal Dictionary<string, DataObjGetter> allDatas = new Dictionary<string, DataObjGetter>();

            public DataObjGetter this[string className] => GetDataGetterByClassName(className);

            /// <summary>
            /// 通过数据的类型名字获取对应数据实例的访问器,该接口可以直接使用
            /// </summary>
            /// <param name="className"></param>
            /// <returns></returns>
            public DataObjGetter GetDataGetterByClassName(string className)
            {
                DataObjGetter getter = null;
                allDatas.TryGetValue(className, out getter);
                return getter;
            }

            /// <summary>
            /// 获取所有数据对象实例的类型名称一共有哪些
            /// </summary>
            /// <returns></returns>
            public string[] AllName()
            {
                return allDatas.Keys.ToArray();
            }
        }
    }

    public static partial class Lookup
    {
        /// <summary>
        /// 内部通过该接口注册进一个解析过后的数据
        /// </summary>
        /// <param name="enumObj"></param>
        internal static void AddData(DataObject data)
        {
            lock (dataLock)
            {
                Channel.Viewer.DataObjGetter getter = null;
                if (!Datas.allDatas.TryGetValue(data.ClassName, out getter))
                {
                    getter = new Channel.Viewer.DataObjGetter();
                    Datas.allDatas.Add(data.ClassName, getter);
                }
                getter.AddObject(data);
            }
        }

        static object dataLock = new object();
        static Channel.Viewer.DataViewer datasInstance= new Channel.Viewer.DataViewer();

        /// <summary>
        /// 对象数据查看器
        /// </summary>
        public static Channel.Viewer.DataViewer Datas => datasInstance; 
 

    }
}
 