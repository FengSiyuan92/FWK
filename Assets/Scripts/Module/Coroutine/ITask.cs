using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TASK_STATE
{
    /// <summary>
    /// 等待开始中
    /// </summary>
    WAIT,
    /// <summary>
    /// 正在运行中
    /// </summary>
    RUNNING,
    /// <summary>
    /// 暂停中
    /// </summary>
    PAUSE,
    /// <summary>
    /// 正常运行结束
    /// </summary>
    ENDED,
    /// <summary>
    /// 被提前终止
    /// </summary>
    BREAK
}

/// <summary>
/// 任务接口，可以对任务进行开始、停止、暂停、恢复、重新启动等操作
/// </summary>
public interface ITask 
{
    /// <summary>
    /// 获取当前任务状态
    /// </summary>
    TASK_STATE State { get; }

    /// <summary>
    /// 开始任务
    /// </summary>
    void Start();

    /// <summary>
    /// 暂停任务
    /// </summary>
    void Pause();

    /// <summary>
    /// 恢复任务，继续执行
    /// </summary>
    void Resume();

    /// <summary>
    /// 停止任务
    /// </summary>
    void Stop();

    /// <summary>
    /// 重新开始整个任务
    /// </summary>
    void Restart();


}
