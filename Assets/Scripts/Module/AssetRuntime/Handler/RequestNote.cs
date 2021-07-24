using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo 将所有的异步操作使用闭包的都改为note类型作为数据传递依据

public abstract class RequestNote
{
    public abstract void RequestOver(Loaded loaded);
}

public class LoadBundleNote : RequestNote
{
    public string mainBundlePath;
    public int dependCount;
    int m_currentLoadedCount;
    public System.Action<LoadedAssetBundle> onBundlesLoaded;

    public override void RequestOver(Loaded loaded)
    {
        m_currentLoadedCount++;
        loaded.AddReferenceCount();

        var bundle = loaded as LoadedAssetBundle;
        if (m_currentLoadedCount == dependCount + 1 && onBundlesLoaded != null)
        {
            SafeCall.Call(onBundlesLoaded, BundleManager.GetLoadedAssetBundle(mainBundlePath));
        }
    }
}

