using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Channel
{
    public enum CheckStage
    {
        CompileOver=0,
        ParseOver
    }

    public class Checker
    {

        internal static void CheckOnOverCompile()
        {
            Utils.Parallel(onCompileOverRule, (v) => v.Value.Check());
        }

        internal static void CheckOnOverParse()
        {
            Utils.Parallel(onParseOverRule, (v) => v.Value.Check());
        }

        static Dictionary<string, Rule> onCompileOverRule;
        static Dictionary<string, Rule> onParseOverRule;

        /// <summary>
        /// 通过该接口添加复合条件的lua 规则
        /// </summary>
        /// <param name="luaRuleDirPath"></param>
        public static void AddLuaRuleDirectory(string luaRuleDirPath)
        {
            HashSet<string> luaPaths = new HashSet<string>();
            FileUtils.FindDirValidFile(luaRuleDirPath, luaPaths, "*.lua");
            foreach (var path in luaPaths)
            {
                LuaCheckManager.AddCheckFile(path);
            }
        }

        /// <summary>
        /// 添加一个自定义规则
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="stage"></param>
        public static void AddRule(Rule rule, CheckStage stage)
        {
            switch (stage)
            {
                case CheckStage.CompileOver:
                    onCompileOverRule.Add(rule.RuleName, rule);
                    break;
                case CheckStage.ParseOver:
                    onParseOverRule.Add(rule.RuleName, rule);
                    break;
                default:
                    break;
            }
        }

        internal static Rule MeetRule(string ruleName)
        {
            Rule result = null;
            if (onCompileOverRule.TryGetValue(ruleName, out result) ||
                onParseOverRule.TryGetValue(ruleName, out result))
            {
                return result;
            }
            return null;
        }

        static Checker()
        {
            onCompileOverRule = new Dictionary<string, Rule>();
            onParseOverRule = new Dictionary<string, Rule>();
            // 内置的检查转换器有效性的代码
            Rule convertValidCheck = new Rule("ConvertValid", InnerCheck_ConvertValid.CheckConverterValid);
            AddRule(convertValidCheck, CheckStage.CompileOver);
            // 内置的检查字段引用的代码
            Rule checkReference = new Rule("ref", InnerCheck_CheckReferenceValid.Check);
            AddRule(checkReference, CheckStage.ParseOver);

            Rule checkResExist = new Rule("res", InnerCheck_ValidField.Check);
            AddRule(checkResExist, CheckStage.ParseOver);
        }

    }
}
