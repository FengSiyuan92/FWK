using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetRuntime;
using System;
using System.IO;
using XLua;

/// <summary>
/// 资源管理器,主要目的是为了对外界屏蔽assetbundle的概念
/// 提供同步加载\异步加载接口
/// 同步加载和异步加载都受 资源加载模拟模式 SIMULATE MODEL 的影响
/// 当勾选模拟加载时,在编辑器下也会强行使用editor下打好的bundle进行加载
///     在勾选模拟加载的前提先,并且勾选了部分模拟,则会使用assetpathmap以及assetdatabase来同时加载
/// 当不勾选时,在编辑器下将直接使用AssetDataBase来加载对应资源
/// </summary>
[LuaCallCSharp]
public class AssetManager : FMonoModule
{
    static Dictionary<string, string> spriteMap = new Dictionary<string, string>(1000);
    static AssetRuntime.Version ClientVersion;

    public override IEnumerator OnPrepare()
    {

        ClientVersion = AssetRuntime.Version.GenClientVersion();
        Bundle.Initialize(ClientVersion);
        Asset.Initialize();
        yield return null;

        FillSpriteMap();
        yield return null;
    }

    public override void OnInitialize()
    {
        // 使用客户端版本初始化游戏
    }

    public override void Restart()
    {
        var version = AssetRuntime.Version.GenClientVersion();
        Bundle.ReStart(version);
        Asset.ReStart();
        Resources.UnloadUnusedAssets();
    }

    public override void OnRefresh()
    {
        RequestHandleDriver.Drive();
    }

    /// <summary>
    /// 尚且有资源或者bundle正在加载中,此时某些逻辑不应该触发
    /// </summary>
    /// <returns></returns>
    public static bool HasUnDoneTask()
    {
        return RequestHandleDriver.Runing;
    }


    public static string GetAtlasName(string spriteName)
    {
        string atlas = "";
        spriteMap.TryGetValue(spriteName, out atlas);
        return atlas;

    }

    static void FillSpriteMap()
    {
        var filePath = ClientVersion.GetFilePath(AssetUtils.SpriteMap);
        if (!File.Exists(filePath))
        {
            Debug.LogError("没有找到sprite映射文件,路径" + filePath);
            return;
        }

        spriteMap.Clear();
        var read = File.OpenText(filePath);
        string line = read.ReadLine();
        do
        {
            var splites = line.Split('|');
            spriteMap.Add(splites[0], splites[1]);

            line = read.ReadLine();
        } while (!string.IsNullOrEmpty(line));
    }
}
