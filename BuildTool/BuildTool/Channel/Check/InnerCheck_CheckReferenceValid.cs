using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define;

namespace Channel
{
    class InnerCheck_CheckReferenceValid
    {

        public static void Check(List<Rule.RuleInfo> infos)
        {
            // 类型名 需要做内容备份的字段名
            Dictionary<string, HashSet<string>> contentIndex = new Dictionary<string, HashSet<string>>();
            // 缓存了所有需要检查的类名和下属字段名
            Dictionary<string, List<Rule.RuleInfo>> allCheckName = new Dictionary<string, List<Rule.RuleInfo>>();

            List<Field> targetFieldName = new List<Field>();
            foreach (var fieldInfo in infos)
            {
                var checkClassName = fieldInfo.field.Belong.Name;
                List<Rule.RuleInfo> checkFields = null;
                if (!allCheckName.TryGetValue(checkClassName, out checkFields))
                {
                    checkFields = new List<Rule.RuleInfo>();
                    allCheckName.Add(checkClassName, checkFields);
                }
                checkFields.Add(fieldInfo);


                var targetInfo = fieldInfo.ruleInfo;
                var candf = targetInfo.Split('.');
                var targetClassName = candf[0];
                HashSet<string> names = null;
                if (!contentIndex.TryGetValue(targetClassName, out names))
                {
                    names = new HashSet<string>();
                    contentIndex.Add(targetClassName, names);
                }
                names.Add(candf[1]);
            }

            // 做引用内容备份 key classname key1 fieldname list objects
            Dictionary<string, Dictionary<string, HashSet<object>>> refContent = new Dictionary<string, Dictionary<string, HashSet<object>>>();
            foreach (var item in contentIndex)
            {
                var className = item.Key;
                var fields = item.Value;

                // fieldname 和所有有效数据
                Dictionary<string, HashSet<object>> c = new Dictionary<string, HashSet<object>>();
                foreach (var fieldname in fields)
                {
                    c.Add(fieldname, new HashSet<object>());
                }

                // 通过类名取到所有数据
                var datas = Lookup.Datas[className].AllDatas();
                // 遍历数据从里面拿所需字段对应的数据,并添加进去
                foreach (var data in datas)
                {
                    foreach (var fieldName in fields)
                    {
                        c[fieldName].Add(data[fieldName]);
                    }
                }

                refContent.Add(className, c);
            }
            Utils.Parallel(allCheckName, (i) => CheckRef(i, refContent));
        }

        const string refErrorTip = "{0}类型中不存在{1}={2}的数据,但是却想在=>{3}.{4}中使用";
        static void CheckRef(KeyValuePair<string, List<Rule.RuleInfo>> item,
            Dictionary<string, Dictionary<string, HashSet<object>>> refContent)
        {
            var checkClassName = item.Key;
            var fieldInfos = item.Value;

            var getter = Lookup.Datas[checkClassName];
            if (getter == null)
            {
                return;
            }
            var allData = getter.AllDatas();

            foreach (var info in fieldInfos)
            {
                var refPos = info.ruleInfo;
                var splitInfo = refPos.Split('.');
                var targetClassName = splitInfo[0];
                var targetFieldName = splitInfo[1];
                var content = refContent[targetClassName][targetFieldName];

                foreach (var data in allData)
                {
                    // obj自己的数据
                    var objData = data[info.field.FieldName];
                    if (!Utils.EqualsContains(content, objData))
                    {
                        CLog.LogError(refErrorTip,
                            targetClassName, targetFieldName, objData.ToString(),
                            data.Source(), info.field.FieldName);
                    }
                }
            }
        }
    }
}
