using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using UnityEngine;

public class DownLoadThread :IDisposable
{
    private Thread thread;
    private Action<NotiData> func;
    private Stopwatch sw;
    private string currDownFile;

     readonly object m_lockObject = new object();
    Queue<ThreadEvent> events;

    delegate void ThreadSyncEvent(NotiData data);
    private ThreadSyncEvent m_SyncEvent;

    public void Initialize()
    {
        m_SyncEvent = OnSyncEvent;
        thread = new Thread(OnUpdate);
        sw = new Stopwatch();
        currDownFile = string.Empty;
        events = new Queue<ThreadEvent>();

        thread.Start();
    }


    public void Dispose()
    {
        thread.Abort();
        m_SyncEvent = null;
        thread = null;
        sw = null;
        events = null;
    }

    /// <summary>
    /// 添加到事件队列
    /// </summary>
    public void AddEvent(ThreadEvent ev, Action<NotiData> func)
    {
        lock (m_lockObject)
        {
            this.func = func;
            events.Enqueue(ev);
        }
    }

    /// <summary>
    /// 通知事件
    /// </summary>
    /// <param name="state"></param>
    private void OnSyncEvent(NotiData data)
    {
        if (this.func != null) func(data);  //回调逻辑层
    }

    void OnUpdate()
    {
        while (true)
        {
            lock (m_lockObject)
            {
                if (events.Count > 0)
                {
                    ThreadEvent e = events.Dequeue();
                    try
                    {
                        OnDownloadFile(e.url, e.file);
                    }
                    catch (System.Exception ex)
                    {
                        UnityEngine.Debug.LogError(ex.Message);
                    }
                }
            }
            Thread.Sleep(1);
        }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    void OnDownloadFile(string url, string file)
    {
        using (WebClient client = new WebClient())
        {
            sw.Start();
            client.DownloadProgressChanged += (ProgressChanged);
            client.DownloadFileAsync(new System.Uri(url), file);
        }
    }

    private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        string value = string.Format("{0} kb/s", (e.BytesReceived / 1024 / sw.Elapsed.TotalSeconds).ToString("0.00"));
        NotiData data = new NotiData(NotiConst.UPDATE_PROGRESS, value);
        if (m_SyncEvent != null) m_SyncEvent(data);

        if (e.ProgressPercentage == 100 && e.BytesReceived == e.TotalBytesToReceive)
        {
            sw.Reset();

            data = new NotiData(NotiConst.UPDATE_DOWNLOAD, currDownFile);
            if (m_SyncEvent != null) m_SyncEvent(data);
        }
    }

    /// <summary>
    /// 调用方法
    /// </summary>
    void OnExtractFile()
    {
        ///------------------通知更新面板解压完成--------------------
        NotiData data = new NotiData(NotiConst.UPDATE_DOWNLOAD, null);
        if (m_SyncEvent != null) m_SyncEvent(data);
    }


}