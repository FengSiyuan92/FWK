using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FAction {
    public class GraphicToAction : FAction
    {
        public enum GraphicControlType
        {
            /// <summary>
            /// 仅alpha
            /// </summary>
            OnlyAlpha = 0,
            /// <summary>
            /// 除了alpha
            /// </summary>
            ExcludeAlpha = 1,
            /// <summary>
            /// rgba
            /// </summary>
            All = 2,
        }

        public override ActionType ActionType => ActionType.GraphicTo;
        public GraphicControlType ControlType { get; private set; }
        Graphic graphic;

        Color target;
        Color start;
        float time = 0;

        float currentTime = 0;

        public void InitGraphicInfo( Graphic graphic, GraphicControlType type, Color target, float duration)
        {
            this.graphic = graphic;
            ControlType = type;

            this.target = target;
            this.time = duration;
        }

        public override void Replay()
        {
            base.Replay();
            currentTime = 0;
        }
        public override void OnStartTick()
        {
            start = graphic.color;
        }

        public override void Tick()
        {
            base.Tick();
            if (time <= 0)
            {
                SetGraphic(target);
                this.IsFinish = true;
                return;
            }


            currentTime += Time.deltaTime;
            float percent = currentTime / time;
            if (percent >= 1)
            {
                SetGraphic(target);
                this.IsFinish = true;
                return;
            }
            var lerp = Color.Lerp(start, target, percent);
            SetGraphic(lerp);
    
        }

        public override void Clear()
        {
            base.Clear();
            graphic = null;
            currentTime = 0;
            time = 0;
        }

        void SetGraphic(Color target)
        {
            var current = graphic.color;
            switch (ControlType)
            {
                case GraphicControlType.OnlyAlpha:
                    graphic.color = new Color(current.r, current.g, current.b, target.a);
                    break;
                case GraphicControlType.ExcludeAlpha:
                    graphic.color = new Color(target.r, target.g, target.b, current.a);
                    break;
                case GraphicControlType.All:
                    graphic.color = target;
                    break;
                default:
                    break;
            }
        }
    }
}
