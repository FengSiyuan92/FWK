using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FAction
{
    // 顺序执行组合节点
    public class SequenceAction : GroupAction
    {
        public override ActionType ActionType => ActionType.Sequence;

        int currentIndex;

        public override void ClearGroupInfo()
        {
            currentIndex = 0;
        }

        public override void Reset()
        {
            foreach (var child in childs)
            {
                child.Reset();
            }
            currentIndex = 0;
            this.IsFinish = false;
        }

        public override void Tick()
        {
            var targetAction = childs[currentIndex];
            if (!targetAction.IsFinish)
            {
                targetAction.Tick();
            }

            if (targetAction.IsFinish)
            {
                currentIndex++;
            }
   
            if (currentIndex >= ChildCount)
            {
                this.IsFinish = true;
            }
        }
    }
}