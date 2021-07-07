using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Channel.RawDefine;

namespace Channel.Agent.XML
{
    class XMLAgent :IFileAgent,IDisposable
    {

       

        public bool Valid { get; private set; }
        XmlDocument doc;
        FileStream file;
        string filePath;
    
        public void Compile()
        {
            throw new NotImplementedException();
        }

  

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

        public XMLAgent(string filePath)
        {
            if (!IsValidXMLFile(filePath))
            {
                Valid = false;
                return;
            }
            this.filePath = filePath;
            file = new FileStream(filePath, System.IO.FileMode.Open,
            System.IO.FileAccess.Read, FileShare.ReadWrite);
            doc = new XmlDocument();
            doc.Load(file);
            Valid = true;
        }

        public void LoadDefine()
        {
            var root = doc.DocumentElement;

            foreach (var defineNode in root)
            {
                var node = defineNode as XmlNode;
                switch (node.Name)
                {
                    case ConstString.XML_ENUM_DEF_TYPE:
                        LoadEnumDef(node);
                        break;

                    case ConstString.XML_OBJ_DEF_TYPE:
                        LoadObjDef(node);
                        break;
                    default:
                        CLog.LogError("XML定义目前只支持 enum和obj两种类型");
                        break;
                }
            }
        }

        public void LoadContent()
        {
            //CLog.LogError("XML文件中不支持定义Content");
        }

        /// <summary>
        /// 根据定义枚举的xml生成枚举定义,并添加到Collect中
        /// 根据定义枚举的xml生成枚举定义,并添加到Collect中
        /// </summary>
        /// <param name="enumNode"></param>
        void LoadEnumDef(XmlNode enumNode)
        {
            // 创建一个新的enum定义
            RawObjDef obj = new RawObjDef(enumNode.Attributes[ConstString.XML_TYPE_TITLE].Value, RawObjType.ENUM);

            // 遍历enum下所有node,用来生成枚举的字段定义
            var nodelist = enumNode.ChildNodes;

            //当前枚举值递增的结果,防止重复
            var defaultValue = 0;
            for (int i = 0; i < nodelist.Count; i++)
            {
                var node = nodelist.Item(i);
                var valueContent = node.GetNodeAttributeValue(ConstString.XML_VALUE_TITLE);
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
                        CLog.LogError("{0}下的枚举定义{1},值必须为整数类型", filePath, obj.Name);
                        return;
                    }
                }

                RawFieldDef fieldDef = new RawFieldDef();
                fieldDef.FieldName = node.GetNodeAttributeValue(ConstString.XML_NAME_TITLE);
                fieldDef.SourceInfo = string.Format("{0}:{1}.{2}", filePath, obj.Name, fieldDef.FieldName);

                fieldDef.FieldType = RawFieldType.Int;
                fieldDef.OutputType = node.GetNodeAttributeValue(ConstString.XML_OUTPUT_TYPE_TITLE);
                fieldDef.CheckRule = node.GetNodeAttributeValue(ConstString.XML_CHECK_RULE_TITLE);
                fieldDef.DefaultValue = enumValue.ToString();
                fieldDef.AppendDef +=  "alias=" + node.GetNodeAttributeValue(ConstString.XML_ALIAS_TITLE);

                obj.AddFieldDefine(fieldDef);
            }

            // 将枚举定义添加进enumlookup中
            Lookup.AddRawDef(obj);
        }

        /// <summary>
        /// 通过xmlNode创建一个Object类型的定义
        /// </summary>
        /// <param name="enumNode"></param>
        void LoadObjDef(XmlNode enumNode)
        {
            RawObjDef objDef = new RawObjDef(enumNode.Attributes[ConstString.XML_TYPE_TITLE].Value, RawObjType.OBJECT);
    
            var nodelist = enumNode.ChildNodes;

            for (int i = 0; i < nodelist.Count; i++)
            {
                var node = nodelist.Item(i);
                // 为Obj基础字段赋值
                RawFieldDef fieldDefine = new RawFieldDef();
                fieldDefine.FieldName = node.GetNodeAttributeValue(ConstString.XML_NAME_TITLE);
                fieldDefine.SourceInfo = string.Format("{0}:{1}.{2}", filePath, objDef.Name, fieldDefine.FieldName);
                if (string.IsNullOrEmpty(fieldDefine.FieldName))
                {
                    CLog.LogError("{0}:{1}的Object定义必须使用 name=\"??\"格式来定义字段名", filePath, objDef.Name);
                    return;
                }
                fieldDefine.FieldType = node.GetNodeAttributeValue(ConstString.XML_TYPE_TITLE);
                fieldDefine.OutputType = node.GetNodeAttributeValue(ConstString.XML_OUTPUT_TYPE_TITLE);

                var refPos = node.GetNodeAttributeValue(ConstString.XML_REF_TITLE);
                refPos = string.IsNullOrEmpty(refPos) ? refPos : "ref=" + refPos;

                var defaultValue = node.GetNodeAttributeValue(ConstString.XML_DEFAULT_TITLE);
                defaultValue = string.IsNullOrEmpty(defaultValue) ? defaultValue : "default=" + refPos;

                string[] append = new string[] {
                    refPos,defaultValue
                };

                fieldDefine.AppendDef = string.Join("&", append);
                fieldDefine.CheckRule = node.GetNodeAttributeValue(ConstString.XML_CHECK_RULE_TITLE);
                var index = node.GetNodeAttributeValue(ConstString.XML_FIELD_INDEX);
                int defIndex = 0;
                if (!string.IsNullOrEmpty(index))
                {
                    int.TryParse(index, out defIndex);
                }
                fieldDefine.DefIndex = defIndex;
                // 将字段定义添加进objdef中
                objDef.AddFieldDefine(fieldDefine);
            }
            // 将obj定义添加进lookup中
            Lookup.AddRawDef(objDef);
        }

        public void Dispose()
        {
            file.Close();
        }
    }
}
