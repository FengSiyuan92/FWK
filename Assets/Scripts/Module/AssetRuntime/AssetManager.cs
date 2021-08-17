using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetRuntime;
using System;
using System.IO;


/// <summary>
/// 资源管理器,主要目的是为了对外界屏蔽assetbundle的概念
/// 提供同步加载\异步加载接口
/// 同步加载和异步加载都受 资源加载模拟模式 SIMULATE MODEL 的影响
/// 当勾选模拟加载时,在编辑器下也会强行使用editor下打好的bundle进行加载
///     在勾选模拟加载的前提先,并且勾选了部分模拟,则会使用assetpathmap以及assetdatabase来同时加载
/// 当不勾选时,在编辑器下将直接使用AssetDataBase来加载对应资源
/// </summary>

public class AssetManager : FMonoModule
{
  

    public override void OnInitialize()
    {
        // 使用客户端版本初始化游戏
        AssetRuntime.Version ClientVersion = AssetRuntime.Version.GenClientVersion();
        Bundle.Initialize(ClientVersion);
        Asset.Initialize();
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

    /// <summary>
    /// 获取一个sprite
    /// </summary>
    /// <param name="imgAssetName"></param>
    /// <param name="onLoad"></param>
    public static void GetSprite(string imgAssetName, System.Action<Sprite> onLoad)
    {
        Asset.GetAssetAsync(imgAssetName, (asset) => onLoad(asset as Sprite));
    }

    /// <summary>
    /// 获取一个texture
    /// </summary>
    /// <param name="imgAssetName"></param>
    /// <param name="onLoad"></param>
    public static void GetTexture(string imgAssetName, System.Action<Texture2D> onLoad)
    {
        Asset.GetAssetAsync(imgAssetName, (asset) => onLoad(asset as Texture2D));
    }

    /// <summary>
    /// 获取一个prefab
    /// </summary>
    /// <param name="prefabName"></param>
    /// <param name="onLoad"></param>
    public static void GetPrefab(string prefabName, System.Action<GameObject> onLoad)
    {
        Asset.GetAssetAsync(prefabName, (asset) => onLoad(asset as GameObject));
    }

    /// <summary>
    /// 归还Sprite,通知该资源可以释放
    /// </summary>
    /// <param name="imgAssetName"></param>
    public static void ReturnSprite(string imgAssetName)
    {
        Asset.ReturnAsset(imgAssetName);
    }

    /// <summary>
    /// 归还Texture通知该资源可以释放
    /// </summary>
    /// <param name="imgAssetName"></param>
    public static void ReturnTexture(string imgAssetName)
    {
        Asset.ReturnAsset(imgAssetName);
    }

    /// <summary>
    /// 归还GameObject通知该资源可以释放
    /// </summary>
    /// <param name="imgAssetName"></param>
    public static void ReturnPrefab(string prefabName)
    {
        Asset.ReturnAsset(prefabName);
    }
}
