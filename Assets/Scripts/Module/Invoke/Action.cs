using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace FAction {
    /// <summary>
    /// 行为类型
    /// </summary>
    public enum ActionType
    {
        /*   复合行为     */
        Default = 0,
        Sync, // 同时执行某些行为,当所有行为都执行结束后,该action结束
        WaitFast, // 同时执行某些行为,当其中任何一个行为执行结束后,该action结束
        Sequence, // 顺序执行某些行为,从头到尾全部执行结束后,该action结束
        Repeat, // 循环N次

        /*   单一行为    */
        CallFunc,
        Delay,
        WaitFrame,

        /*    Transform用行为     */
        TransformTo,

        /*    UnityGraphic用行为     */
        GraphicTo,
    }

    [BlackList]
    public class ActionFactory
    {
        // group action pool
        public static ReuseObjectPool<SyncAction> syncActionPool = new ReuseObjectPool<SyncAction>(50);
        public static ReuseObjectPool<RepeatAction> repeatActionPool = new ReuseObjectPool<RepeatAction>(50);
        public static ReuseObjectPool<SequenceAction> sequenceActionPool = new ReuseObjectPool<SequenceAction>(50);
        public static ReuseObjectPool<SyncWaitFastAction> waitFastActionPool = new ReuseObjectPool<SyncWaitFastAction>();

        // signle action pool
        public static ReuseObjectPool<CallFuncAction> callFuncActionPool = new ReuseObjectPool<CallFuncAction>(50);
        public static ReuseObjectPool<DelayAction> delayActionPool = new ReuseObjectPool<DelayAction>(50);
        public static ReuseObjectPool<WaitFrameAction> waitFrameActionPool = new ReuseObjectPool<WaitFrameAction>(10);
        public static ReuseObjectPool<TransformToAction> transformToActionPool = new ReuseObjectPool<TransformToAction>(10);
        public static ReuseObjectPool<GraphicToAction> graphicToAction = new ReuseObjectPool<GraphicToAction>(10);
      

        public static void ReturnActionInstance(FAction action)
        {
            switch (action.ActionType)
            {
                case ActionType.Default:
                    break;
                case ActionType.Sync:
                    syncActionPool.Push(action as SyncAction);
                    break;
                case ActionType.WaitFast:
                    waitFastActionPool.Push(action as SyncWaitFastAction);
                    break;
                case ActionType.Sequence:
                    sequenceActionPool.Push(action as SequenceAction);
                    break;
                case ActionType.Repeat:
                    repeatActionPool.Push(action as RepeatAction);
                    break;
                case ActionType.CallFunc:
                    callFuncActionPool.Push(action as CallFuncAction);
                    break;
                case ActionType.Delay:
                    delayActionPool.Push(action as DelayAction);
                    break;
                case ActionType.WaitFrame:
                    waitFrameActionPool.Push(action as WaitFrameAction);
                    break;
                case ActionType.TransformTo:
                    transformToActionPool.Push(action as TransformToAction);
                    break;
                case ActionType.GraphicTo:
                    graphicToAction.Push(action as GraphicToAction);
                    break;
                default:
                    break;
            }
        }
        public static void Preload() { }

    }

    /// <summary>
    /// 行为对象生成器
    /// </summary>
    [LuaCallCSharp]
    public static class Action
    {

 //////////////////////////////////////////////// 基础逻辑行为创建接口 /////////////////////////////////////////////////
        /// <summary>
        /// 按顺序逐个执行一些行为
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static SequenceAction Sequence(params FAction[] p)
        {
            var seq = ActionFactory.sequenceActionPool.Get();
            foreach (var action in p)
            {
                seq.AddAction(action);
            }
            return seq;
        }

        /// <summary>
        /// 同时执行一些行为
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static SyncAction Sync(params FAction[] p)
        {
            var sync = ActionFactory.syncActionPool.Get();
            foreach (var action in p)
            {
                sync.AddAction(action);
            }
            return sync;
        }

        /// <summary>
        /// 同时并发执行多个action,当任何一个行为结束时,即为结束
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static SyncWaitFastAction WaitFast(params FAction[] p)
        {
            var sync = ActionFactory.waitFastActionPool.Get();
            foreach (var action in p)
            {
                sync.AddAction(action);
            }
            return sync;
        }

        /// <summary>
        /// 重复对某些行为执行repeatCount次, 如果repeatcount小于0,则等价于Loop函数
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static RepeatAction Repeat(int repeatCount, params FAction[] p)
        {
            var repeat = ActionFactory.repeatActionPool.Get();
            if (p.Length == 1)
            {
                repeat.AddAction(p[0]);
            }
            else
            {
                repeat.AddAction(Sequence(p));
            }
            repeat.repeatCount = repeatCount;
            return repeat;
        }

        // 一直重复做某些行为
        public static RepeatAction Loop(params FAction[] p)
        {
            var repeat = ActionFactory.repeatActionPool.Get();
            if (p.Length == 1)
            {
                repeat.AddAction(p[0]);
            }
            else
            {
                repeat.AddAction(Sequence(p));
            }
            repeat.repeatCount = -1;
            return repeat;
        }

        /// <summary>
        /// 调用一个闭包
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static CallFuncAction Call(System.Action action)
        {
            var call = ActionFactory.callFuncActionPool.Get();
            call.action = action;
            return call;
        }

        /// <summary>
        /// 等待多少秒
        /// </summary>
        /// <param name="delayTime"></param>
        /// <returns></returns>
        public static DelayAction Delay(float delayTime)
        {
            var delay = ActionFactory.delayActionPool.Get();
            delay.delayTime = delayTime;
            return delay;
        }

        /// <summary>
        /// 等待多少帧
        /// </summary>
        /// <param name="frameCount"></param>
        /// <returns></returns>
        public static WaitFrameAction WaitFrame(int frameCount)
        {
            var waitFrame = ActionFactory.waitFrameActionPool.Get();
            waitFrame.waitCount = frameCount;
            return waitFrame;
        }
//////////////////////////////////////////////// Transform行为创建接口 /////////////////////////////////////////////////
        /// <summary>
        /// 移动到某个位置
        /// </summary>
        /// <param name="targetTransform">要移动的transform</param>
        /// <param name="x"> x坐标</param>
        /// <param name="y">y 坐标 </param>
        /// <param name="duration">多少s内挪动到,传入小于等于0的数值,则瞬间移动并完成</param>
        /// <returns></returns>
        public static TransformToAction MoveLocalPos(this Transform targetTransform, float x, float y, float duration)
        {
            return MoveLocalPos(targetTransform, x, y, targetTransform.localPosition.z, duration);
        }

        /// <summary>
        /// 移动到某个位置
        /// </summary>
        /// <param name="targetTransform">要移动的transform</param>
        /// <param name="x"> x坐标</param>
        /// <param name="y">y 坐标 </param>
        /// <param name="duration">多少s内挪动到,传入小于等于0的数值,则瞬间移动并完成</param>
        /// <returns></returns>
        public static TransformToAction MoveLocalPos(this Transform targetTransform, float x, float y, float z, float duration)
        {
            var action = ActionFactory.transformToActionPool.Get();
            action.InitTransform(targetTransform, TransformToAction.TransformControlType.LocalPosition, new Vector3(x, y, z), duration);
            return action;
        }
        /// <summary>
        /// 移动到某个位置
        /// </summary>
        /// <param name="targetTransform">要移动的transform</param>
        /// <param name="x"> x坐标</param>
        /// <param name="y">y 坐标 </param>
        /// <param name="duration">多少s内挪动到,传入小于等于0的数值,则瞬间移动并完成</param>
        /// <returns></returns>
        public static TransformToAction MovePos(this Transform targetTransform, float x, float y, float duration)
        {
            return MovePos(targetTransform, x, y, targetTransform.position.z, duration);
        }

        /// <summary>
        /// 移动到某个位置
        /// </summary>
        /// <param name="targetTransform">要移动的transform</param>
        /// <param name="x"> x坐标</param>
        /// <param name="y">y 坐标 </param>
        /// <param name="duration">多少s内挪动到,传入小于等于0的数值,则瞬间移动并完成</param>
        /// <returns></returns>
        public static TransformToAction MovePos(this Transform targetTransform, float x, float y, float z, float duration)
        {
            var action = ActionFactory.transformToActionPool.Get();
            action.InitTransform(targetTransform, TransformToAction.TransformControlType.Position, new Vector3(x, y, z), duration);
            return action;
        }

        /// <summary>
        /// 缩放到某个比例
        /// </summary>
        /// <param name="targetTransform"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static TransformToAction ScaleTo(this Transform targetTransform, float x, float y, float duration)
        {
            return ScaleTo(targetTransform, x, y, targetTransform.localScale.z, duration);
        }

        public static TransformToAction ScaleTo (this Transform targetTransform, float x, float y, float z, float duration)
        {
            var action = ActionFactory.transformToActionPool.Get();
            action.InitTransform(targetTransform, TransformToAction.TransformControlType.LocalScale, new Vector3(x, y, z), duration);
            return action;
        }

        public static TransformToAction RotateToLocal(this Transform targetTransform, float x, float y, float duration)
        {
            return RotateToLocal(targetTransform, x, y, targetTransform.localEulerAngles.z, duration);
        }

        public static TransformToAction RotateToLocal(this Transform targetTransform, float x, float y, float z, float duration)
        {
            var action = ActionFactory.transformToActionPool.Get();
            action.InitTransform(targetTransform, TransformToAction.TransformControlType.LocalRotation, new Vector3(x, y, z), duration);
            return action;
        }

        public static TransformToAction RotateTo(this Transform targetTransform, float x, float y, float duration)
        {
            return RotateTo(targetTransform, x, y, targetTransform.eulerAngles.z, duration);
        }

        public static TransformToAction RotateTo(this Transform targetTransform, float x, float y, float z, float duration)
        {
            var action = ActionFactory.transformToActionPool.Get();
            action.InitTransform(targetTransform, TransformToAction.TransformControlType.Rotation, new Vector3(x, y, z), duration);
            return action;
        }

 //////////////////////////////////////////////// Graphic行为创建接口 /////////////////////////////////////////////////
        public static GraphicToAction ColorTo(this UnityEngine.UI.Graphic graphic, float r, float g, float b, float duration)
        {
            var action = ActionFactory.graphicToAction.Get();
            action.InitGraphicInfo(graphic,  GraphicToAction.GraphicControlType.ExcludeAlpha, new Color(r,g,b,0), duration);
            return action;
        }

        public static GraphicToAction AlphaTo(this UnityEngine.UI.Graphic graphic, float a, float duration)
        {
            var action = ActionFactory.graphicToAction.Get();
            action.InitGraphicInfo(graphic, GraphicToAction.GraphicControlType.OnlyAlpha, new Color(0,0,0, a), duration);
            return action;
        }

        public static GraphicToAction ColorTo(this UnityEngine.UI.Graphic graphic, float r, float g, float b, float a, float duration)
        {
            var action = ActionFactory.graphicToAction.Get();
            action.InitGraphicInfo(graphic, GraphicToAction.GraphicControlType.All, new Color(r, g, b, a), duration);
            return action;
        }
    }
}
