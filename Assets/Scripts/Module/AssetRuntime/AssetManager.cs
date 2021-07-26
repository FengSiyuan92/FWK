using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssetRuntime;
using System;
/// <summary>
/// 
/// </summary>
public class AssetManager : FMonoModule
{
    public override IEnumerator OnPrepare()
    {
        Asset.Initialize();
        yield break;
    }


    public override void OnInitialize()
    {
       
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
}
