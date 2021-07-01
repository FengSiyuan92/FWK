using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Channel.Agent.XML
{
    static class Extend
    {
        public static string GetNodeAttributeValue(this XmlNode node, string key)
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
    }
}
