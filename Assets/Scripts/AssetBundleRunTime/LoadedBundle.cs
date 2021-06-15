using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedAssetBundle
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

    public void AddReferenceCount()
    {
        m_referenceCount++;
    }

    public bool UnloadAssetBundle()
    {
        m_referenceCount--;
        if (m_referenceCount == 0)
        {
            m_assetBundle.Unload(false);
            m_assetBundle = null;
            return true;
        }
        return false;
    }
}

