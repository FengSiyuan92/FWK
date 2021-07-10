using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Channel
{
    internal class InnerCheck_ValidField
    {

        static void CheckValidAsset(string path,DataObject data, Rule.RuleInfo info)
        {
            if (string.IsNullOrEmpty(path) )
            {
                CLog.LogError("配置的资源'{0}'不存在,请检查 => {1}.{2}", path, data.Source(), info.field.FieldName);
            }
            else
            {
                var pre = info.ruleInfo;
                var p = ConstString.STR_EMPTY;
                if (!string.IsNullOrEmpty(pre))
                {
                    p = Path.Combine(pre, path);
                }
                if (!FileUtils.IsValidAssetPath(p))
                {
                    CLog.LogError("配置的资源'{0}'不存在,请检查 => {1}.{2}", path, data.Source(), info.field.FieldName);
                }
            }
    
        }

        public static void Check(List<Rule.RuleInfo> infos)
        {
            foreach (var ruleInfo in infos)
            {
                var field = ruleInfo.field;
                if (field.FieldType != typeof(string)) continue;
  
                var className = field.Belong.Name;
                var getter = Lookup.Datas[className];
                var all = getter.AllDatas();
                foreach (var data in all)
                {
                    var obj = data[field.FieldName];
                    if (obj == null) continue;

                    if (obj is string)
                    {
                        var path = obj as string;
                        CheckValidAsset(path, data, ruleInfo);
                    }
                    else if (obj is DataArray)
                    {
                        var array = obj as DataArray;
                        if (array.ElementType == typeof(string))
                        {
                            for (int i = 0; i < array.Length; i++)
                            {
                                var p = array[i] as string;
                                CheckValidAsset(p, data, ruleInfo);
                            }
                        }

                    }
                }
            }
        }

    }
}
