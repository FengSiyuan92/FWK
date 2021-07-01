using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Channel.RawDefine;

namespace Channel.Reader
{
    public class XMLReader
    {
        // 获取枚举类型名称的arrtibute name
        const string EnumTypeTitle = "type";
        // 获取枚举字段名称的AT_NAME
        const string EnumNameTitle = "name";
        // 获取枚举别名内容的AT_NAME
        const string EnumAliasTitle = "alias";
        // 获取枚举对应值的AT_NAME
        const string EnumValueTitle = "value";

        // 获取Object类型名称的arrtibute name
        const string ObjTypeTitle = "type";
        // 获取Obj字段名称的AT_NAME
        const string ObjNameTitle = "name";
        // 获取Obj字段类型名称的AT_NAME
        const string ObjFieldTypeTitle = "type";
        // 输出类型
        const string ObjFieldOutputTitle = "out";
        // 内容描述
        const string ObjValueDescTitle = "desc";
        // 检查规则
        const string ObjCheckRuleTitle = "check";


        static bool IsValidXMLFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }
            // 参数传递进来的 忽略文件不读取
            if (GlobalArgs.IsIgnoreFile(filePath))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///  加载xml中的Enum定义
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>是否加载成功</returns>
        public static bool LoadEnumDefine(string filePath)
        {
            if (!IsValidXMLFile(filePath)) return false;

            using (FileStream file = new FileStream(filePath, System.IO.FileMode.Open,
               System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                var root = doc.DocumentElement;
                
                foreach (var enumNode in root)
                {
                    //TODO: 多线程和trycatch
                    CreateAndFillEnum(enumNode as XmlNode);
                }
            }

            return true;
        }

        /// <summary>
        /// 根据定义枚举的xml生成枚举定义,并添加到Collect中
        /// 根据定义枚举的xml生成枚举定义,并添加到Collect中
        /// </summary>
        /// <param name="enumNode"></param>
        static void CreateAndFillEnum(XmlNode enumNode)
        {
            // 创建一个新的enum定义
            RawEnumDefine def = new RawEnumDefine();
            def.Name = enumNode.Attributes[EnumTypeTitle].Value;

            // 遍历enum下所有node,用来生成枚举的字段定义
            var nodelist = enumNode.ChildNodes;

            //当前枚举值递增的结果,防止重复
            var defaultValue = 0;
            for (int i = 0; i < nodelist.Count; i++)
            {
                var node = nodelist.Item(i);
                var valueContent = GetNodeAttributeValue(node, EnumValueTitle);
                int enumValue = defaultValue++;
                if (!string.IsNullOrEmpty(valueContent))
                {
                    int res = enumValue;
                    if (int.TryParse(valueContent, out res))
                    {
                        enumValue = res;
                        defaultValue = ++res;
                    }
                    else
                    {
                        throw new Exception("枚举值必须是整数类型");
                    }
                }

                RawEnumFieldDefine edef = new RawEnumFieldDefine();
                edef.FieldName = GetNodeAttributeValue(node, EnumNameTitle);
                edef.alias = GetNodeAttributeValue(node, EnumAliasTitle);
                edef.value = enumValue;
                def.AddFieldDefine(edef);
            }

            // 将枚举定义添加进enumlookup中
            Lookup.AddRawObjDef(def);
        }

        static string GetNodeAttributeValue(XmlNode node, string key)
        {
            // 没有属性返回空串
            var atts = node.Attributes;
            if (atts == null || atts.Count == 0) return string.Empty;

            // 属性中没有指定key值的内容,返回空串
            var attribute = atts[key];
            if (attribute == null) return string.Empty;

            // 返回里面的value值
            var value = attribute.Value;
            return value;
        }

        /// <summary>
        /// 加载XMl文件中的Obj定义
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool LoadObjectDefine(string filePath)
        {
            if (!IsValidXMLFile(filePath)) return false;
            using (FileStream file = new FileStream(filePath, System.IO.FileMode.Open,
              System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                var root = doc.DocumentElement;

                foreach (var enumNode in root)
                {
                    //TODO: 多线程和trycatch
                    CreateObjDefine(enumNode as XmlNode);
                }
            }

            return true;
        }
        /// <summary>
        /// 通过xmlNode创建一个Object类型的定义
        /// </summary>
        /// <param name="enumNode"></param>
        static void CreateObjDefine(XmlNode enumNode)
        {
            RawObjDef def = new RawObjDef();
            def.Name = enumNode.Attributes[ObjTypeTitle].Value;
            var nodelist = enumNode.ChildNodes;

            for (int i = 0; i < nodelist.Count; i++)
            {
                var node = nodelist.Item(i);
                // 为Obj基础字段赋值
                RawObjFieldDef fieldDefine = new RawObjFieldDef();
                fieldDefine.FieldName = GetNodeAttributeValue(node, ObjNameTitle);
                if (string.IsNullOrEmpty(fieldDefine.FieldName))
                {
                    throw new Exception("必须定义字段名");
                }
              
                fieldDefine.fieldType = GetNodeAttributeValue(node, ObjFieldTypeTitle);
                fieldDefine.outType = GetNodeAttributeValue(node, ObjFieldOutputTitle);
                fieldDefine.valueAppend = GetNodeAttributeValue(node, ObjValueDescTitle);
                fieldDefine.checkRule = GetNodeAttributeValue(node, ObjCheckRuleTitle);

                // 将字段定义添加进objdef中
                def.AddFieldDefine(fieldDefine);
            }

            // 将obj定义添加进lookup中
            Lookup.AddRawObjDef(def);
        }
    }
}
