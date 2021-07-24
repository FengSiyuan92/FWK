using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Loaded
{
    uint m_referenceCount;
    public void AddReferenceCount()
    {
        m_referenceCount++;
    }
    public void TryUnload()
    {
        m_referenceCount--;
        if (m_referenceCount == 0)
        {
            OnUnload();
        }
    }
    protected abstract void OnUnload();
}

public class LoadedAssetBundle : Loaded
{
    uint m_referenceCount;
    AssetBundle m_assetBundle;

    public void SetBundle(AssetBundle assetbundle)
    {
        m_assetBundle = assetbundle;
    }
    public void Clear()
    {
        m_assetBundle = null;
        m_referenceCount = 0;
    }

    public AssetBundle assetBundle
    {
        get { return m_assetBundle; }
    }

    protected override void OnUnload()
    {
        m_assetBundle.Unload(false);
        m_assetBundle = null;
    }
}

