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

    public override void onRefresh()
    {
        RequestHandleDriver.Drive();
    }

    public static void GetSprite(string imgAssetName, System.Action<Sprite> onLoad)
    {

    }

    public static void GetTexture(string imgAssetName, System.Action<Texture2D> onLoad)
    {

    }

    public static void GetPrefab(string prefabName, System.Action<GameObject> onLoad)
    {

    }
}
