using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedAsset : Loaded
{
    LoadedAssetBundle m_belongBundle;
    Object m_asset;
    public Object Asset => m_asset;
    protected override void OnUnload()
    {
        Resources.UnloadAsset(m_asset);
        m_asset = null;
    }

    public void Clear()
    {
        m_asset = null;
        m_belongBundle = null;
    }

    public void SetInfo(LoadedAssetBundle bundle, UnityEngine.Object asset)
    {
        m_belongBundle = bundle;
        m_asset = asset;
    }
}
