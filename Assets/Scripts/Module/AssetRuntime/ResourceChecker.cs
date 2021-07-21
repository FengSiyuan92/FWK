using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceChecker 
{
    const string EXTRACT_RESOURCE_KEY = "HAS_EXTRACT_RESOURCE";
    const string CURRENT_VERSION = "CRT_VERSION";
    private HashSet<string> downloadFiles = new HashSet<string>();

    public string[] messageInfo =
    {
        "正在解包文件->",

        "解包完成",
        
        "正在检查资源更新",
    };

    string m_message = "";
    public string message
    {
        get
        {
            return m_message;
        }
    }

    /// <summary>
    /// 检查是否需要解包
    /// </summary>
    /// <returns></returns>
    public bool CheckExtractResources()
    {
        bool isExists = PlayerPrefs.GetInt(EXTRACT_RESOURCE_KEY, 0) == 1;
        return isExists;
    }

    /// <summary>
    /// 开始解包资源
    /// </summary>
    /// <param name="extracted"></param>
    public void StartExtractResource(System.Action extracted)
    {
        //CoroutineManager.StartCoroutine(ExtractResource(extracted), false);
    }

    /// <summary>
    /// 检查是否需要更新资源
    /// </summary>
    /// <param name="updated"></param>
    public bool CheckUpdateResources()
    {
        var currentVersion = PlayerPrefs.GetString(CURRENT_VERSION,"1.0.0.0");
        // 从服务器获取版本号，如果当前版本号一致，则无需更新，否则进行更新
        bool versionFit = true;
        return !versionFit;
    }

    /// <summary>
    /// 开始更新资源
    /// </summary>
    /// <param name="updated"></param>
    public void StartUpdateResources(System.Action updated)
    {
       // CoroutineManager.StartCoroutine(UpdateResource(updated), false);
    }

    /// <summary>
    /// 提取资源到设备的资源目录 -->persistentDataPath
    /// </summary>
    /// <returns></returns>
    IEnumerator ExtractResource(Action overAction)
    {
        string dataSavePath = AssetUtils.dataSavePath;  //数据目录
        string resPath = AssetUtils.appContentPath; //游戏包资源目录

        if (Directory.Exists(dataSavePath))
            Directory.Delete(dataSavePath, true);
        Directory.CreateDirectory(dataSavePath);

        string infile = resPath + "files.txt";
        string outfile = dataSavePath + "files.txt";
        if (File.Exists(outfile)) File.Delete(outfile);

        m_message = messageInfo[0];

        Debug.Log(infile);
        Debug.Log(outfile);
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW www = new WWW(infile);
            yield return www;

            if (www.isDone)
            {
                File.WriteAllBytes(outfile, www.bytes);
            }
            yield return 0;
        }
        else
            File.Copy(infile, outfile, true);
        yield return Yield.waitForEndFrame;

        //释放所有文件到数据目录
        string[] files = File.ReadAllLines(outfile);
        int totalNum = files.Length;
        foreach (var file in files)
        {
            string[] fs = file.Split('|');
            infile = resPath + fs[0];  //
            outfile = dataSavePath + fs[0];
            m_message = messageInfo[0] + fs[0];
            string dir = Path.GetDirectoryName(outfile);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (Application.platform == RuntimePlatform.Android)
            {
                WWW www = new WWW(infile);
                yield return www;

                if (www.isDone)
                {
                    File.WriteAllBytes(outfile, www.bytes);
                }
                yield return 0;
            }
            else
            {
                if (File.Exists(outfile))
                {
                    File.Delete(outfile);
                }
                File.Copy(infile, outfile, true);
            }
            yield return Yield.waitForEndFrame;
        }
        m_message = messageInfo[1];
        PlayerPrefs.SetInt(EXTRACT_RESOURCE_KEY, 1);
        overAction();
    }

    IEnumerator UpdateResource(Action overAction)
    {
        using (DownLoadThread downLoadThread = new DownLoadThread())
        {
            downLoadThread.Initialize();

            string dataPath = AssetUtils.dataSavePath;  //数据目录
            string url = AppConst.WebUrl;
            string random = DateTime.Now.ToString("yyyymmddhhmmss");
            string listUrl = url + "files.txt?v=" + random;

            WWW www = new WWW(listUrl); yield return www;
            if (www.error != null)
            {
                overAction();
                yield break;
            }
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
            File.WriteAllBytes(dataPath + "files.txt", www.bytes);
            string filesText = www.text;
            string[] files = filesText.Split('\n');

            for (int i = 0; i < files.Length; i++)
            {
                if (string.IsNullOrEmpty(files[i]))
                {
                    continue;
                }
                string[] keyValue = files[i].Split('|');
                string f = keyValue[0];
                string localfile = (dataPath + f).Trim();
                string path = Path.GetDirectoryName(localfile);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileUrl = url + f + "?v=" + random;
                bool needUpdate = !File.Exists(localfile);
                if (!needUpdate)
                {
                    // 比价本地文件和远程文件的md5码是否相同，不相同则删除本地文件
                    string remoteMd5 = keyValue[1].Trim();
                    string localMd5 = AssetUtils.GetMd5(localfile);
                    needUpdate = !remoteMd5.Equals(localMd5);
                    if (needUpdate)
                    {
                        File.Delete(localfile);
                    }
                }
                if (needUpdate)
                {
                    BeginDownload(downLoadThread, fileUrl, localfile);
                    while (!(IsDownOK(localfile)))
                    {
                        yield return Yield.waitForEndFrame;
                    }
                }
            }
            yield return Yield.waitForEndFrame;

            //获取服务器版本号
            string version = "";
            PlayerPrefs.SetString(CURRENT_VERSION, "1.0.0.0");
        }
        overAction();
    }


    /// <summary>
    /// 是否下载完成
    /// </summary>
    bool IsDownOK(string file)
    {
        return downloadFiles.Contains(file);
    }

    /// <summary>
    /// 副线程下载
    /// </summary>
    void BeginDownload(DownLoadThread loader,string url, string file)
    {    
        ThreadEvent ev = new ThreadEvent();
        ev.Key = NotiConst.UPDATE_DOWNLOAD;
        ev.url = url;
        ev.file = file;
        loader.AddEvent(ev, OnThreadCompleted);   //线程下载
    }

    /// <summary>
    /// 线程完成
    /// </summary>
    /// <param name="data"></param>
    void OnThreadCompleted(NotiData data)
    {
        switch (data.evName)
        {
            case NotiConst.UPDATE_DOWNLOAD: //下载一个完成
                downloadFiles.Add(data.evParam.ToString());
                Debug.Log("资源更新完成：" + data.evParam.ToString());
                break;
            case NotiConst.UPDATE_PROGRESS: //下载进度
                break;
        }
    }
}
public class ThreadEvent
{
    public string Key;
    public string url;
    public string file;
}

public class NotiData
{
    public string evName;
    public object evParam;

    public NotiData(string name, object param)
    {
        this.evName = name;
        this.evParam = param;
    }
}
public class NotiConst
{
    public const string UPDATE_DOWNLOAD = "UpdateDownload";         //更新下载
    public const string UPDATE_PROGRESS = "UpdateProgress";         //更新进度
}
