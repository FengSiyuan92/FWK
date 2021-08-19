using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FAction
{
    public class WaitFrameAction : FAction
    {
        public override ActionType ActionType => ActionType.WaitFrame;
        // 延迟多少帧
        public int waitCount;

        int remain = -1;

        public override void Tick()
        {
            if (remain < 0)
            {
                remain = waitCount;
            }

            remain--;

            if (remain < 0)
            {
                this.IsFinish = true;
            }
        }

        public override void Clear()
        {
            base.Clear();
            waitCount = 0;
        }


        public override void Reset()
        {
            remain = -1;
            this.IsFinish = false;
        }
    }
}
