using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

    }
}
