using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FAction
{

    public class FAction : IReuseObject
    {
        public bool IsPause { get; private set; }

        bool runing = false;
        bool ticking = false;

        // 执行一次后自动复用
        public bool AutoReuse { get; set; } = false;
        public bool IsFinish { get; protected set; } = false;

        public virtual ActionType ActionType => ActionType.Default;
        // action是否执行结束
     

        // 更新action行为
        public virtual void Tick()
        {
            if (!ticking)
            {
                OnStartTick();
                ticking = true;
            }
        }

        public virtual void OnStartTick()
        {

        }

        // 行为复位,恢复运行参数为初始状态,但
        public virtual void Replay()
        {
            this.IsFinish = false;
            this.ticking = false;

        }

        // 入池后清理残留数据的接口,不需要使用者调用
        public virtual void Clear()
        {

        }

        // 销毁一个Action对象,在使用侧用完之后,应该调用Destroy
        public virtual void Destroy()
        {
            ActionManager.RemoveAction(this);
            runing = false;
            AutoReuse = false;
            ActionFactory.ReturnActionInstance(this);
        }

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
            Replay();
            if (AutoReuse)
            {
                Destroy();
            }
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