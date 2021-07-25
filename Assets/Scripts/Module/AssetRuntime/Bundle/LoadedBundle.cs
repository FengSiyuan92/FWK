using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AssetRuntime
{
    public class LoadedBundle : Loaded
    {
        uint m_referenceCount;
        AssetBundle m_AssetBundle;
        HashSet<string> m_AssetNames;

        public AssetBundle Bundle => m_AssetBundle;

        public void SetBundle(AssetBundle assetbundle)
        {
            m_AssetBundle = assetbundle;
        }

        public void Clear()
        {
            m_AssetBundle = null;
            m_referenceCount = 0;
        }

        protected override void OnUnload()
        {
            m_AssetBundle.Unload(false);
            m_AssetBundle = null;
        }
    }

}