using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AssetRuntime
{
    public class LoadedAsset : Loaded
    {
        LoadedBundle m_belongBundle;
        Object m_asset;
        public Object Asset => m_asset;

        protected override void OnUnload()
        {
            Resources.UnloadAsset(m_asset);
            m_belongBundle.TryUnload();
            m_asset = null;
        }

        public void Clear()
        {
            m_asset = null;
            m_belongBundle = null;
        }

        public void SetInfo(LoadedBundle bundle, UnityEngine.Object asset)
        {
            m_belongBundle = bundle;
            m_asset = asset;
        }
    }
}