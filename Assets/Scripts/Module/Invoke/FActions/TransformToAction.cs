using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FAction
{
    public class TransformToAction : FAction
    {
        public enum TransformControlType
        {
            LocalPosition = 0,
            Position,
            LocalScale,
            LocalRotation,
            Rotation,
        }

        public override ActionType ActionType =>  ActionType.TransformTo;


        public TransformControlType ControlType { get; private set; }

        Transform transform;
        Vector3 start;
        Vector3 target;
        public float time;
       
        float currentTime = 0;

        public void InitTransform(Transform transform, TransformControlType type, Vector3 target, float time = 1)
        {
            this.transform = transform;
            ControlType = type;
            this.target = target;
            this.time = time;
        }

        public override void OnStartTick()
        {
            start = GetTransform();
        }

        public override void Replay()
        {
            base.Replay();
            currentTime = 0;
        }

        public override void Clear()
        {
            base.Clear();
            transform = null;
            start = Vector3.zero;
            time = 0;
        }


        void SetTransform(Vector3 current)
        {
            switch (ControlType)
            {
                case TransformControlType.LocalPosition:
                    transform.localPosition = current;
                    break;
                case TransformControlType.Position:
                    transform.position = current;
                    break;
                case TransformControlType.LocalScale:
                    transform.localScale = current;
                    break;
                case TransformControlType.LocalRotation:
                    transform.localEulerAngles = current;
                    break;
                case TransformControlType.Rotation:
                    transform.eulerAngles = current;
                    break;
                default:
                    break;
            }
        }

        Vector3 GetTransform()
        {
            switch (ControlType)
            {
                case TransformControlType.LocalPosition:
                    return transform.localPosition;
                case TransformControlType.Position:
                    return transform.position;
                case TransformControlType.LocalScale:
                    return transform.localScale;
                case TransformControlType.LocalRotation:
                    return transform.localEulerAngles;
                case TransformControlType.Rotation:
                    return transform.eulerAngles;
                default:
                    break;
            }
            return Vector3.zero;
        }

        public override void Tick()
        {
            base.Tick();
            if (time <= 0)
            {
                SetTransform(target);
                this.IsFinish = true;
                return;
            }
            currentTime += Time.deltaTime;

            float percent = currentTime / time;
            if (percent > 1)
            {
                SetTransform(target);
                this.IsFinish = true;
                return;
            }
          
            var current = GetTransform();
            var lerp = Vector3.Slerp(start, target, percent);
            SetTransform(lerp);

        }
    }
}