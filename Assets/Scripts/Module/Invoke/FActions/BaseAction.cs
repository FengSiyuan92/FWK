using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FAction
{
    public abstract class FAction : IReuseObject
    {
        public bool IsPause { get; private set; }

        bool runing = false;

        // 更新action行为
        public abstract void Tick();

        // action是否执行结束
        public virtual bool IsFinish { get; protected set; } = false;

        // 行为复位,恢复参数为初始状态
        public abstract void Reset();

        // 入池后清理残留数据的接口,不需要使用者调用
        public virtual void Clear()
        {
            this.IsFinish = false;
        }

        // 销毁一个Action对象,在使用侧用完之后,应该调用Destroy
        public virtual void Destroy()
        {
            ActionManager.RemoveAction(this);
            runing = false;
            ActionFactory.ReturnActionInstance(this);
        }

        public virtual ActionType ActionType => ActionType.Default;

        public void Run()
        {
            if (runing)
            {
                return;
            }
            runing = true;
            this.IsPause = false;
            ActionManager.AppendAction(this);
        }

        public void Stop()
        {
            ActionManager.RemoveAction(this);
            runing = false;
            Reset();
        }

        public void Pause()
        {
            this.IsPause = true;
        }

        public void Resume()
        {
            this.IsPause = false;
        }
    }

    public abstract class GroupAction : FAction
    {
        protected List<FAction> childs = new List<FAction>();

        public int ChildCount => childs.Count;

        public void AddAction(FAction action)
        {
            childs.Add(action);
        }

        public abstract void ClearGroupInfo();

        public override void Clear()
        {
            childs.Clear();
            base.Clear();
            ClearGroupInfo();
        }

        public override void Destroy()
        {
            foreach (var child in childs)
            {
                child.Destroy();
            }
            base.Destroy();
        }
    }
}