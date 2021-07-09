using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Channel.Define
{
    public class Enum
    {
        /// <summary>
        /// 枚举类型名
        /// </summary>
        public string Name { get; private set; }

        public Enum(string name)
        {
            Name = name;
            Lookup.AddEnumDefine(this);
        }
        /// <summary>
        /// TODO:该枚举数据来源于哪个文件
        /// </summary>
        public string FromFilePath { get; internal set; }
        public class Item
        {
            public Enum Belong { get; internal set; }
            public string Name { get; internal set; }
            public int Value { get; internal set; }
            public string Alias { get; internal set; }
            public OutputType OutputType { get; internal set; }
            public override string ToString()
            {
                if (!string.IsNullOrEmpty(Alias))
                {
                   return string.Format("{0}({1})={2}", Name, Alias, Value);
                }
                else
                {
                    return string.Format("{0}={1}", Name, Value);
                }
                return "";
            }
        }

        Dictionary<string, Item> name2ItemMap = new Dictionary<string, Item>();
        Dictionary<string, Item> alias2ItemMap = new Dictionary<string, Item>();
        Dictionary<int, Item> value2ItemMap = new Dictionary<int, Item>();

        /// <summary>
        /// 通过枚举的字段名获取对应的枚举值,如果枚举中没有指定字段名,将返回-1
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public int GetValueByFieldName(string fieldName)
        {
            var item = GetItemByFieldName(fieldName);
            return item != null ? item.Value : -1;
        }

        /// <summary>
        /// 通过枚举的别名获取枚举对应值,如果枚举没有定义指定别名,将返回-1
        /// </summary>
        /// <param name="aliasName"></param>
        /// <returns></returns>
        public int GetValueByAlias(string aliasName)
        {
            var item = GetItemByAlias(aliasName);
            return item != null ? item.Value : -1;
        }

        /// <summary>
        /// 通过枚举别名获取枚举Item,如果没有指定别名的字段将会返回null
        /// </summary>
        /// <param name="aliasName"></param>
        /// <returns></returns>
        public Item GetItemByAlias(string aliasName)
        {
            Item item = null;
            alias2ItemMap.TryGetValue(aliasName, out item);
            return item;
        }

        /// <summary>
        /// 通过枚举的字段名获取枚举item,如果没有指定的字段名将会返回null
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Item GetItemByFieldName(string fieldName)
        {
            Item item = null;
            name2ItemMap.TryGetValue(fieldName, out item);
            return item;
        }

        /// <summary>
        /// 获取枚举中所有字段名
        /// </summary>
        /// <returns></returns>
        public string[] GetAllItemName()
        {
            return name2ItemMap.Keys.ToArray();
        }

        /// <summary>
        /// 获取枚举中所有的别名
        /// </summary>
        /// <returns></returns>
        public string[] GetAllItemAlias()
        {
            return alias2ItemMap.Keys.ToArray();
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("enum=>{0}:\n", Name);
            foreach (var item in name2ItemMap)
            {
                sb.AppendLine(item.ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// 添加一个枚举Item
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="outputType"></param>
        /// <param name="alias"></param>
        internal void AddEnumItem(string name, int value, OutputType outputType, string alias = "")
        {
            Item item = new Item();
            item.Belong = this;
            item.Name = name;
            item.Value = value;
            item.OutputType = outputType;
            item.Alias = alias;

            if (name2ItemMap.ContainsKey(name))
            {
                CLog.LogError("{0}中定义的枚举{1}名称重复:{2}", FromFilePath, Name, name);
            }
            else
            {
                name2ItemMap.Add(name, item);
            }

            if (value2ItemMap.ContainsKey(value))
            {
                CLog.LogError("{0}中定义的枚举{1}值重复:{2}", FromFilePath, Name, value);
            }
            else
            {
                value2ItemMap.Add(value, item);
            }

            if (!string.IsNullOrEmpty(alias))
            {
                if (alias2ItemMap.ContainsKey(alias))
                {
                    CLog.LogError("{0}中定义的枚举{1}别名重复:{2}", FromFilePath, Name, alias);
                }
                else
                {
                    alias2ItemMap.Add(alias, item);
                }
            }
        }

    }

}
