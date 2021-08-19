using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FAction
{
    public class DelayAction : FAction
    {
        public override ActionType ActionType => ActionType.Delay;
        // 延迟时间,单位秒
        public float delayTime;

        float remain = -1f;

        public override void Tick()
        {
            if (remain < 0)
            {
                remain = delayTime;
            }

            remain = remain - Time.deltaTime;
            if (remain <= 0)
            {
                this.IsFinish = true;
            }
        }

        public override void Clear()
        {
            base.Clear();
            delayTime = 0;
            remain = -1;
        }


        public override void Reset()
        {
            remain = -1;
            this.IsFinish = false;
        }
    }
}

