using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FAction
{
    // 并发执行组合节点
    public class SyncAction : GroupAction
    {
        public override ActionType ActionType => ActionType.Sync;
        public override void ClearGroupInfo() { }


        public override void Reset()
        {
            foreach (var child in childs)
            {
                child.Reset();
            }
            this.IsFinish = false;
        }

        public override void Tick()
        {
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

            if (finishCount == ChildCount)
            {
                this.IsFinish = true;
            }
        }
    }
}