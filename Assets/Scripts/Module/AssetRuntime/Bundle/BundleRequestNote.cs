using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetRuntime
{
    public partial class Bundle
    {
        class LoadBundleNote : IRequestNote
        {
            int m_currentLoadedCount;
            public bool Valid { get; set; }
            public string mainBundleName;
            public int dependCount;
            public System.Action<LoadedBundle> onBundlesLoaded;

            public string TargetName => mainBundleName;

            public void OnRequestOver(Loaded loaded)
            {
                m_currentLoadedCount++;

                var bundle = loaded as LoadedBundle;
                if (bundle.BundleName != mainBundleName)
                {
                    loaded.AddReferenceCount();
                }

                if (m_currentLoadedCount == dependCount + 1 && onBundlesLoaded != null)
                {
                    var mainBundle = GetLoadedtBundle(mainBundleName);
                    mainBundle.IsMain = true;
                    SafeCall.Call(onBundlesLoaded, mainBundle);
                    m_notePool.Push(this);
                }
            }

            public void OnInterrupt()
            {

            }

            public void Clear()
            {
                mainBundleName = null;
                dependCount = 0;
                m_currentLoadedCount = 0;
                onBundlesLoaded = null;
            }
        }
    }


}
