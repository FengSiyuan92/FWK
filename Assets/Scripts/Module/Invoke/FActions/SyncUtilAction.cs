using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FAction
{
    // 并发执行组合节点
    public class SyncWaitFastAction : GroupAction
    {
        public override ActionType ActionType => ActionType.WaitFast;
        public override void ClearGroupInfo() { }

        public override void Replay()
        {
            base.Replay();
            foreach (var child in childs)
            {
                child.Replay();
            }
        }

        public override void Tick()
        {
            base.Tick();
            var finishCount = 0;
            for (int i = 0; i < ChildCount; i++)
            {
                var child = childs[i];
                if (!child.IsFinish)
                {
                    child.Tick();
                }

                if (child.IsFinish)
                {
                    finishCount++;
                }
            }

            if (finishCount > 0)
            {
                this.IsFinish = true;
            }
        }
    }
}