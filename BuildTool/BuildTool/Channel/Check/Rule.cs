using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Define;

namespace Channel
{
    public class Rule
    {
        public string RuleName { get; private set; }
        Action<List<RuleInfo>> action;
        public Rule(string ruleName, Action<List<RuleInfo>> checkFunc)
        {
            RuleName = ruleName;
            action = checkFunc;
        }

        public class RuleInfo
        {
            public Field field { get; internal set; }
            public string ruleInfo { get; internal set; }
            internal RuleInfo(string info, Field field)
            {
                this.field = field;
                this.ruleInfo = info;
            }
        }

        List<RuleInfo> careInfo = new List<RuleInfo>();

        internal void AddCareforField(Field field, string ruleInfo)
        {
            lock (careInfo)
            {
                careInfo.Add(new RuleInfo(ruleInfo, field));
            }
        }

        internal void Check()
        {
            if (action != null)
            {
                action(careInfo);
            }
        }

    }
}
