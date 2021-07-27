using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum F_MODULE_STATE
{
    /// <summary>
    /// 准备运行
    /// </summary>
    PREPARE,
    /// <summary>
    /// 运行中
    /// </summary>
    RUNNING,
    /// <summary>
    /// 暂停运行中
    /// </summary>
    PAUSE,
    /// <summary>
    /// 运行结束
    /// </summary>
    ENDED
}
public interface FModuleInterface
{
    F_MODULE_STATE STATE { get; set; }

    IEnumerator OnPrepare();

    int RelativeOrder { get; }

    void OnInitialize ();

    void OnRefresh();

    void OnPause();

    void OnResume();

    void Restart();
}
