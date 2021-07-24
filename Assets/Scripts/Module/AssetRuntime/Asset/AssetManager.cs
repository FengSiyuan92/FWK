using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 资源管理器,主要目的是为了对外界屏蔽assetbundle的概念
/// 提供同步加载\异步加载接口
/// 同步加载和异步加载都受 资源加载模拟模式 SIMULATE MODEL 的影响
/// 当勾选模拟加载时,在编辑器下也会强行使用editor下打好的bundle进行加载
///     在勾选模拟加载的前提先,并且勾选了部分模拟,则会使用assetpathmap以及assetdatabase来同时加载
/// 当不勾选时,在编辑器下将直接使用AssetDataBase来加载对应资源
/// </summary>
public class AssetManager
{
    
    static Dictionary<string, LoadedAsset> m_LoadedAsset;

#if UNITY_EDITOR
    static string[] assetsFolder = new string[] { "AssetBundles" };

    static Object LoadAssetInEditor(string assetName)
    {
        string[] paths = AssetDatabase.FindAssets(assetName, assetsFolder);
        if (paths.Length == 0)
        {
            AssetUtils.LogAssetEmpty(assetName, typeof(Object).ToString());
            return null;
        }
        var result = AssetDatabase.LoadAssetAtPath<Object>(paths[0]);
        return result;
    }
#endif

   

    static void SafeGetAsset(string assetName, System.Action<Object> onAssetLoaded, LoadedAsset asset)
    {
        if (asset == null)
        {
            AssetUtils.LogAssetEmpty(assetName, typeof(Object).ToString());
        }
       
    }

    static LoadedAsset TryGetAsset(string assetName)
    {
        LoadedAsset asset = null;
        m_LoadedAsset.TryGetValue(assetName, out asset);
        return asset;
    }

  
    #region 同步接口
    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="assetName">资源名</param>
    /// <returns></returns>
    public static Object GetAsset(string assetName)
    {
#if UNITY_EDITOR
        if (!AssetSimulate.simulateModel)
        {
            return LoadAssetInEditor(assetName);
        }
#endif

        LoadedAsset loaded = TryGetAsset(assetName);
        if (loaded != null) return loaded.Asset;
   
        string bundleName = AssetNameHelper.LookBundleName(assetName);
        var bundle = BundleManager.GetAssetBundle(bundleName);
        Object asset = bundle == null ? null : bundle.assetBundle.LoadAsset<Object>(assetName);
        CreateLoadedAsset(assetName, asset, bundle);
        return asset;
    }

    static LoadedAsset CreateLoadedAsset(string assetName, Object asset, LoadedAssetBundle bundle)
    {
        if (asset == null)
        {
            return null;
        }
        var loaded = LoadedAssetPool.Get(asset, bundle);
        m_LoadedAsset.Add(assetName, loaded);
        return loaded;
    }

    #endregion

    #region 异步接口

    public class GetAssetNote: RequestNote
    {
        public string targetAssetName;
        public System.Action<Object> onAssetLoaded;

        public override void RequestOver(Loaded loaded)
        {
            var loadedAsset = loaded as LoadedAsset;
            SafeCall.Call(onAssetLoaded, loadedAsset.Asset);
        }
    }

    public class LoadAssetNote : RequestNote
    {
        public string targetAssetName;
        public System.Action<Object> onAssetLoaded;
        public override void RequestOver(Loaded loaded)
        {
            var bundle = loaded as LoadedAssetBundle;
            var note = new GetAssetNote();
            note.targetAssetName = targetAssetName;
            note.onAssetLoaded = onAssetLoaded;
            GetAssetAsyncInternal(bundle, note);
        }
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <typeparam name="T">需要加载的资源类型</typeparam>
    /// <param name="assetName">资源名称</param>
    /// <param name="callback">当资源加载完毕之后的回调</param>
    public static void GetAssetAsync(string assetName, System.Action<Object> callback)
    {
#if UNITY_EDITOR
        if (!AssetSimulate.simulateModel)
        {
            var asset = LoadAssetInEditor(assetName);
            SafeCall.Call(callback, asset);
            return;
        }
#endif
        string bundlePath = AssetNameHelper.LookBundleName(assetName);
        if (bundlePath == null)
        {
            AssetUtils.LogAssetEmpty(assetName, typeof(Object).ToString());
            return;
        }
        
        var loadAssetNote = new LoadAssetNote();
        loadAssetNote.onAssetLoaded = callback;
        loadAssetNote.targetAssetName = assetName;

        BundleManager.LoadBundleAsync(bundlePath, loadAssetNote);
    }

    /// <summary>
    /// 從某個bundle中取出資源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="assetName">资源名</param>
    /// <param name="bundle">bundle</param>
    /// <param name="onAssetLoaded">回调</param>
    static void GetAssetAsyncInternal(LoadedAssetBundle bundle, GetAssetNote note)
    {
        var assetName = note.targetAssetName;
        var callback = note.onAssetLoaded;
        LoadedAsset loaded;
        if (m_LoadedAsset.TryGetValue(assetName, out loaded))
        {
            SafeGetAsset(assetName, callback, loaded);
            return;
        }

        RequestHandler handler = RequestHandleDrive.GetRequestHandler(assetName);

        // 还未申请加载过该资源,则创建一个异步请求
        if (handler == null)
        {
            var request = bundle.assetBundle.LoadAssetAsync(assetName);
            var loadAssetHandler = MAssetRequestPool.Get(request);

            loadAssetHandler.createLoaded += () =>
            {
                loaded = CreateLoadedAsset(assetName, (handler as MAssetRequest).asset, bundle);
                return loaded;
            };

            handler = loadAssetHandler;
            RequestHandleDrive.RegisterRequestHandler(assetName, loadAssetHandler);
        }

        handler.onRequestDone += note.RequestOver;
    }
    #endregion

}
