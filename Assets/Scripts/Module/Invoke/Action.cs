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
        Default = 0,
        Sync,
        Sequence,
        Repeat,
        CallFunc,
        Delay,
        WaitFrame,
    }

    [BlackList]
    public class ActionFactory
    {
        // group action pool
        public static ReuseObjectPool<SyncAction> syncActionPool = new ReuseObjectPool<SyncAction>(50);
        public static ReuseObjectPool<RepeatAction> repeatActionPool = new ReuseObjectPool<RepeatAction>(50);
        public static ReuseObjectPool<SequenceAction> sequenceActionPool = new ReuseObjectPool<SequenceAction>(50);

        // signle action pool
        public static ReuseObjectPool<CallFuncAction> callFuncActionPool = new ReuseObjectPool<CallFuncAction>(50);
        public static ReuseObjectPool<DelayAction> delayActionPool = new ReuseObjectPool<DelayAction>(50);
        public static ReuseObjectPool<WaitFrameAction> waitFrameActionPool = new ReuseObjectPool<WaitFrameAction>(50);

        public static void ReturnActionInstance(FAction action)
        {
            switch (action.ActionType)
            {
                case ActionType.Default:
                    break;
                case ActionType.Sync:
                    syncActionPool.Push(action as SyncAction);
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
    public class Action
    {
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
    }
}
