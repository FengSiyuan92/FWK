using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//////////////////////////////////////////////////////////////////////////////////////////
/// 协程管理类 通过这个类可以：
/// 开启一个新的全局协程 StartGlobal,场景切换时不会停止该协程
/// 开启一个新的局部协程 StartLocal, 在场景切换时，该协程也会伴随停止
/// TODO: 目前已经支持先创建协程但不开始运行（不调用Start），后续有需求可以实现
/// 
//////////////////////////////////////////////////////////////////////////////////////////
public sealed partial class CoroutineManager : FMonoModule
{
    /// <summary>
    /// 全局的协程执行载体
    /// </summary>
    static MonoBehaviour _carrier;

    /// <summary>
    /// 和场景关联的协程驱动代理
    /// </summary>
    static LocalCoroutineAgent _agent;

    /// <summary>
    /// 模块初始化
    /// </summary>
    public override void onInitialize()
    {
        _carrier = this;
    }

    /// <summary>
    /// 启动一个全局的协程
    /// </summary>
    /// <param name="action">函数委托</param>
    /// <returns>任务接口对象</returns>
    public static ITask StartGlobal(CoroutineAction action)
    {
        Task task = new Task(_carrier);
        task.ResetParam(action);
        task.Start();
        return task;
    }

    /// <summary>
    /// 启动一个全局的协程
    /// </summary>
    /// <typeparam name="T">不需要传入</typeparam>
    /// <param name="action">协程函数</param>
    /// <param name="p">协程函数所需要的参数</param>
    /// <returns>任务接口对象</returns>
    public static ITask StartGlobal<T>(CoroutineAction<T> action, T p)
    {
        Task task = new Task(_carrier);
        task.ResetParam(action, p);
        task.Start();
        return task;
    }

    /// <summary>
    /// 启动一个全局的协程
    /// </summary>
    /// <param name="action">协程函数</param>
    /// <param name="p1">协程函数所需要的参数</param>
    /// <param name="p2">协程函数所需要的参数</param>
    /// <returns>任务接口对象</returns>
    public static ITask StartGlobal<T1, T2>(CoroutineAction<T1, T2> action, T1 p1, T2 p2)
    {
        Task task = new Task(_carrier);
        task.ResetParam(action, p1, p2);
        task.Start();
        return task;
    }

    /// <summary>
    /// 启动一个全局的协程
    /// </summary>
    /// <param name="action">协程函数</param>
    /// <param name="p1">协程函数所需要的参数</param>
    /// <param name="p2">协程函数所需要的参数</param>
    /// <param name="p3">协程函数所需要的参数</param>
    /// <returns>任务接口对象</returns>
    public static ITask StartGlobal<T1, T2, T3>(CoroutineAction<T1, T2, T3> action, T1 p1, T2 p2, T3 p3)
    {
        Task task = new Task(_carrier);
        task.ResetParam(action, p1, p2, p3);
        task.Start();
        return task;
    }

    /// <summary>
    /// 启动一个全局的协程
    /// </summary>
    /// <param name="action">协程函数</param>
    /// <param name="p1">协程函数所需要的参数</param>
    /// <param name="p2">协程函数所需要的参数</param>
    /// <param name="p3">协程函数所需要的参数</param>
    /// <param name="p4">协程函数所需要的参数</param>
    /// <returns>任务接口对象</returns>
    public static ITask StartGlobal<T1, T2, T3, T4>(CoroutineAction<T1, T2, T3, T4> action, T1 p1, T2 p2, T3 p3, T4 p4)
    {
        Task task = new Task(_carrier);
        task.ResetParam(action, p1, p2, p3, p4);
        task.Start();
        return task;
    }

    /// <summary>
    /// 在当前场景内启动一个协程
    /// </summary>
    /// <param name="action">协程函数</param>
    /// <returns></returns>
    public static ITask StartLocal(CoroutineAction action)
    {
        Task task = new Task(GetAgent());
        task.ResetParam(action);
        task.Start();
        return task;
    }

    /// <summary>
    /// 在当前场景内启动一个协程
    /// </summary>
    /// <param name="action">协程函数</param>
    /// <param name="p">协程函数所需要的参数</param>
    /// <returns>任务接口对象</returns>
    public static ITask StartLocal<T>(CoroutineAction<T> action, T p)
    {
        Task task = new Task(GetAgent());
        task.ResetParam(action, p);
        task.Start();
        return task;
    }

    /// <summary>
    /// 在当前场景内启动一个协程
    /// </summary>
    /// <param name="action">协程函数</param>
    /// <param name="p1">协程函数所需要的参数</param>
    /// <param name="p2">协程函数所需要的参数</param>
    /// <returns>任务接口对象</returns>
    public static ITask StartLocal<T1, T2>(CoroutineAction<T1, T2> action, T1 p1, T2 p2)
    {
        Task task = new Task(GetAgent());
        task.ResetParam(action, p1, p2);
        task.Start();
        return task;
    }

    /// <summary>
    /// 在当前场景内启动一个协程
    /// </summary>
    /// <param name="action">协程函数</param>
    /// <param name="p1">协程函数所需要的参数</param>
    /// <param name="p2">协程函数所需要的参数</param>
    /// <param name="p3">协程函数所需要的参数</param>
    /// <returns>任务接口对象</returns>
    public static ITask StartLocal<T1, T2, T3>(CoroutineAction<T1, T2, T3> action, T1 p1, T2 p2, T3 p3)
    {
        Task task = new Task(GetAgent());
        task.ResetParam(action, p1, p2, p3);
        task.Start();
        return task;
    }

    /// <summary>
    /// 在当前场景内启动一个协程
    /// </summary>
    /// <param name="action">协程函数</param>
    /// <param name="p1">协程函数所需要的参数</param>
    /// <param name="p2">协程函数所需要的参数</param>
    /// <param name="p3">协程函数所需要的参数</param>
    /// <param name="p4">协程函数所需要的参数</param>
    /// <returns>任务接口对象</returns>
    public static ITask StartLocal<T1, T2, T3, T4>(CoroutineAction<T1, T2, T3, T4> action, T1 p1, T2 p2, T3 p3, T4 p4)
    {
        Task task = new Task(GetAgent());
        task.ResetParam(action, p1, p2, p3, p4);
        task.Start();
        return task;
    }

    static LocalCoroutineAgent GetAgent()
    {
        if (_agent == null)
        {
            var go = new GameObject("CoroutineAgent");
            _agent = go.AddComponent<LocalCoroutineAgent>();
        }

        return _agent; 
    }
}

public delegate IEnumerator CoroutineAction();
public delegate IEnumerator CoroutineAction<in T>(T param);
public delegate IEnumerator CoroutineAction<in T1, in T2>(T1 p1, T2 p2);
public delegate IEnumerator CoroutineAction<in T1, in T2, in T3>(T1 p1, T2 p2, T3 p3);
public delegate IEnumerator CoroutineAction<in T1, in T2, in T3, in T4>(T1 p1, T2 p2, T3 p3, T4 p4);


