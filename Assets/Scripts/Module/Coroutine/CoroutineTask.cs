using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public sealed partial class CoroutineManager :FMonoModule
{
    /// <summary>
    /// 协程任务具体实现类，通过内部类的防止屏蔽外部访问权限，外界的所有操作通过ITask接口调用
    /// </summary>
    class Task : IEnumerator, ITask
    {
        // unity启动起来的协程对象
        Coroutine _coroutine;
        // 外界的迭代器函数
        IEnumerator _enumerator;
        // 协程挂靠的载体
        MonoBehaviour _carrier;
        // 外部委托函数
        Delegate _action;
        // 委托函数调用参数
        object[] _args;

        /// <summary>
        /// 任务状态
        /// </summary>
        public TASK_STATE State
        {
            get;
            private set;
        }

        public object Current
        {
            get
            {
                if (_enumerator != null)
                    return _enumerator.Current;
                return null;
            }
        }

        /// <summary>
        /// 开始协程任务
        /// </summary>
        public void Start()
        {
            State = TASK_STATE.RUNNING;
            if (_coroutine == null)
            {
                _enumerator = _action.Method.Invoke(_action.Target, _args) as IEnumerator;
                _coroutine = _carrier.StartCoroutine(this);
            }
        }

        /// <summary>
        /// 停止协程任务
        /// </summary>
        public void Stop()
        {
            var interupt = State == TASK_STATE.RUNNING || State == TASK_STATE.PAUSE;
            State = interupt ? TASK_STATE.BREAK : TASK_STATE.ENDED;

            if (_coroutine != null)
            {
                _carrier.StopCoroutine(_coroutine);
                _coroutine = null;
                _enumerator = null;
            }
        }

        /// <summary>
        /// 重新开始协程任务
        /// </summary>
        public void Restart()
        {
            if (_coroutine != null)
            {
                _carrier.StopCoroutine(_coroutine);
            }
            State = TASK_STATE.RUNNING;
            _enumerator = _action.Method.Invoke(_action.Target, _args) as IEnumerator;
            _coroutine = _carrier.StartCoroutine(this);
        }

        /// <summary>
        /// 暂停当前协程任务
        /// </summary>
        public void Pause()
        {
            if (State == TASK_STATE.RUNNING)
                State = TASK_STATE.PAUSE;
        }

        /// <summary>
        /// 恢复暂停的协程任务
        /// </summary>
        public void Resume()
        {
            if (State == TASK_STATE.PAUSE)
                State = TASK_STATE.RUNNING;
        }

        /// <summary>
        /// 填充参数，复用时可以调用
        /// </summary>
        /// <param name="action"></param>
        /// <param name="args"></param>
        public void ResetParam(Delegate action, params object[] args)
        {
            _action = action;
            _args = args;
        }

        public bool MoveNext()
        {
            switch (State)
            {
                case TASK_STATE.WAIT:
                case TASK_STATE.PAUSE:
                    return true;
                case TASK_STATE.RUNNING:
                    var result = _enumerator.MoveNext();
                    if (!result)
                        this.State = TASK_STATE.ENDED;
                    return result;
                case TASK_STATE.ENDED:
                case TASK_STATE.BREAK:
                    return false;
            }
            return false;
        }

        public void Reset()
        {
            _coroutine = null;
            _enumerator = null;
            _carrier = null;
            _args = null;
        }

        public Task(MonoBehaviour carrier)
        {
            _carrier = carrier;
            State = TASK_STATE.WAIT;
        }

    }
}
