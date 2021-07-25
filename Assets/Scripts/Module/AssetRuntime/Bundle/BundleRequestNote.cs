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

            public string mainBundlePath;
            public int dependCount;
            public System.Action<LoadedBundle> onBundlesLoaded;

            public string TargetName => mainBundlePath;

            public void RequestOver(Loaded loaded)
            {
                m_currentLoadedCount++;
                loaded.AddReferenceCount();

                if (m_currentLoadedCount == dependCount + 1 && onBundlesLoaded != null)
                {
                    SafeCall.Call(onBundlesLoaded, GetLoadedtBundle(mainBundlePath));
                }
                m_notePool.Push(this);
            }

            public void Clear()
            {
                mainBundlePath = null;
                dependCount = 0;
                m_currentLoadedCount = 0;
                onBundlesLoaded = null;
            }
        }
    }


}
