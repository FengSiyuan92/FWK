using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FAction
{
    public class CallFuncAction : FAction
    {
        public override ActionType ActionType => ActionType.CallFunc;

        // 需要调用的函数
        public System.Action action;

        public override void Tick()
        {
            base.Tick();
            SafeCall.Call(action);
            this.IsFinish = true;
        }

        public override void Clear()
        {
            base.Clear();
            action = null;
        }
    }
}