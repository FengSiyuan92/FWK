using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetRuntime;
using System;
using System.IO;
/// <summary>
/// 
/// </summary>
public class AssetManager : FMonoModule
{
    public override int RelativeOrder => -1;

    public override IEnumerator OnPrepare()
    {
        var filePath = GetAssetMapFilePath();
        AssetMap assetMap = new AssetMap(new FileInfo(filePath));
        Asset.Initialize(assetMap);

        var bundlePath = GetBundleMapFilePath();
        BundleMap bundleMap = new BundleMap(new FileInfo(bundlePath));
        Bundle.Initialize(bundleMap);
        yield break;
    }

    public override void OnRefresh()
    {
        RequestHandleDriver.Drive();
    }

    public static void GetSprite(string imgAssetName, System.Action<Sprite> onLoad)
    {
        Asset.GetAssetAsync(imgAssetName, (asset) => onLoad(asset as Sprite));
    }

    public static void GetTexture(string imgAssetName, System.Action<Texture2D> onLoad)
    {
        Asset.GetAssetAsync(imgAssetName, (asset) => onLoad(asset as Texture2D));
    }

    public static void GetPrefab(string prefabName, System.Action<GameObject> onLoad)
    {
        Asset.GetAssetAsync(prefabName, (asset) => onLoad(asset as GameObject));
    }

    public static void ReturnSprite(string imgAssetName)
    {
        Asset.ReturnAsset(imgAssetName);
    }

    public static void ReturnTexture(string imgAssetName)
    {
        Asset.ReturnAsset(imgAssetName);
    }

    public static void ReturnPrefab(string prefabName)
    {
        Asset.ReturnAsset(prefabName);
    }

    static string GetAssetMapFilePath()
    {

        var perPath = AssetUtils.GetPersistentFilePath("__assetmap");
        if (File.Exists(perPath)) return perPath;
        var strPath = AssetUtils.GetStreamFilePath("__assetmap");
        if (File.Exists(perPath)) return strPath;

        throw new Exception("没有找到资源映射文件");

    }

    static string GetBundleMapFilePath()
    {

        var perPath = AssetUtils.GetPersistentFilePath("__bundleMap");
        if (File.Exists(perPath)) return perPath;
        var strPath = AssetUtils.GetStreamFilePath("__bundleMap");
        if (File.Exists(perPath)) return strPath;

        throw new Exception("没有找到Bundle信息文件");
    }

}
