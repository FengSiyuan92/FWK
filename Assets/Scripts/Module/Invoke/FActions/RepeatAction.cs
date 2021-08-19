using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FAction
{
    // 循环节点
    public class RepeatAction : GroupAction
    {
        public override ActionType ActionType => ActionType.Repeat;
        // 重复次数
        public int repeatCount;

        int overCount;
        public override void Tick()
        {
            var child = childs[0];
            child.Tick();
            if (child.IsFinish)
            {
                overCount++;

                if (repeatCount > 0 && overCount >= repeatCount)
                {
                    this.IsFinish = true;
                }
                else
                {
                    child.Reset();
                }
            }
        }

        public override void Reset()
        {
            foreach (var child in childs)
            {
                child.Reset();
            }
            overCount = 0;
            this.IsFinish = false;
        }

        public override void ClearGroupInfo()
        {
            overCount = 0;
            repeatCount = 0;
        }
    }
}
