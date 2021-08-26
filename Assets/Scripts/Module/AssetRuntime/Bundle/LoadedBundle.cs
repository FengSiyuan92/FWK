using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AssetRuntime
{
    public partial class Bundle
    {
        public class LoadedBundle : Loaded
        {
            AssetBundle m_AssetBundle;
            HashSet<string> m_AssetNames;
            public string BundleName;

            public AssetBundle Bundle => m_AssetBundle;

            public bool IsMain;

            public void SetBundle(AssetBundle assetbundle)
            {
                m_AssetBundle = assetbundle;
            }

            public void Clear()
            {
                m_AssetBundle = null;
                m_referenceCount = 0;
                BundleName = null;
                IsMain = false;
            }

            protected override void OnUnload()
            {
                m_AssetBundle.Unload(false);
                m_AssetBundle = null;

                if (IsMain)
                {
                    var depends = GetDepends(BundleName);
                    foreach (var item in depends)
                    {
                        GetLoadedtBundle(item).TryUnload();
                    }
                }
            }
        }

    }
}