using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AssetManager
{
    #region 常量， 字段， 容器定义
    // 这个字段可以根据宏定义来确定不同平台下的不同文件夹
    const string PREFILE = "StandaloneWindows";

    static Dictionary<string, LoadedAssetBundle> m_abCaches;
    static Dictionary<string, string> m_abPaths;

    static AssetBundleManifestHelper m_manifest;
    static Dictionary<string, RequestHandler> m_requestHandlers;
    static Dictionary<string, string[]> m_depends;
    static string m_BaseDownloadingURL;
    static string pre;

#if UNITY_EDITOR
    const string simulateKey = "ASSET_BUNDLE_SIMULATE";
    static int m_simulateModel = -1;
    public static bool simulateModel
    {
        get
        {
            if (m_simulateModel == -1)
            {
                m_simulateModel = EditorPrefs.GetBool(simulateKey, true) ? 1 : 0;
            }
            return m_simulateModel == 1;
        }
        set
        {
            EditorPrefs.SetBool(simulateKey, value);
            m_simulateModel = value ? 1 : 0;
        }
    }

#endif

    /// <summary>
    /// manifest暂时使用同步加载方案，如果出现问题，可以改成异步加载
    /// </summary>
    class AssetBundleManifestHelper
    {
        const string manifest = "assetbundlemanifest";

        string m_manifestPath;
        AssetBundle m_manifestBundle;
        AssetBundleManifest m_manifest;

        public AssetBundleManifestHelper(string manifestPath)
        {
            m_manifestPath = manifestPath;
#if UNITY_EDITOR
            if (!File.Exists(m_manifestPath))
            {
                Debug.LogError("bundle资源不存在， 点击Assets -> AssetBundleHelpe r-> Build AssetBundle -> Build For StandaloneWindows后重新启动");
                EditorApplication.isPlaying = false;
                return;
            }
#endif
            m_manifestBundle = AssetBundle.LoadFromFile(m_manifestPath);
            m_manifest = m_manifestBundle.LoadAsset<AssetBundleManifest>(manifest);
        }

        public string[] GetAllDependencies(string assetBundleName)
        {
            return m_manifest.GetAllDependencies(assetBundleName);
        }

        public void InitAssetPath(Dictionary<string, string> vessel)
        {
            pre = Application.streamingAssetsPath + "/" + PREFILE;
            var allABs = m_manifest.GetAllAssetBundles();
            for (int i = 0; i < allABs.Length; i++)
            {
                vessel.Add(allABs[i], Path.Combine(pre, allABs[i]));
            }
        }
    }

    #endregion


    #region 对外接口


    /// <summary>
    /// 初始化方法
    /// </summary>
    public static void Initialize()
    {
        // 容器初始化
        m_abCaches = new Dictionary<string, LoadedAssetBundle>();
        m_abPaths = new Dictionary<string, string>();
        m_requestHandlers = new Dictionary<string, RequestHandler>();
        m_depends = new Dictionary<string, string[]>();

#if UNITY_EDITOR
        if (simulateModel)
        {
            return;
        }
#endif

        m_BaseDownloadingURL = AssetUtils.GetRelativePath();
        // manifest初始化
        m_manifest = new AssetBundleManifestHelper(GetManifestPath());
        m_manifest.InitAssetPath(m_abPaths);

        CallFuncManager.InstallRepeatFunc(CallFuncType.update, LoadAsyncHandler, false);
    }

    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="assetName">资源名，不填写扩展名</param>
    /// <returns></returns>
    public static T LoadAsset<T>(string assetName) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        if (simulateModel)
        {
            return LoadAssetSimulate<T>(assetName);
        }
#endif

        if (!m_abPaths.ContainsKey(assetName))
        {
            LogBundleDtExist(assetName);
            return null;
        }
        var bundle = GetAssetBundle(assetName);
        return bundle.assetBundle.LoadAsset<T>(assetName);
    }


    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <typeparam name="T">需要加载的资源类型</typeparam>
    /// <param name="assetName">资源名称 == bundle名，不包含扩展名</param>
    /// <param name="callback">当资源加载完毕之后的回调</param>
    public static void LoadAssetAsync<T>(string assetName, System.Action<T> callback) where T : UnityEngine.Object
    {

#if UNITY_EDITOR
        if (simulateModel)
        {
            var asset = LoadAssetSimulate<T>(assetName);
            callback(asset);
            return;
        }
#endif

        if (!m_abPaths.ContainsKey(assetName))
        {
            LogBundleDtExist(assetName);
            return;
        }

        LoadBundleAndDepends(assetName, (bundle) =>
        {
            GetAssetAsync<T>(assetName, bundle, (asset) =>
            {
                callback(asset);
            });
        });
    }

    #endregion


    #region 资源加载内部方法
#if UNITY_EDITOR
    static T LoadAssetSimulate<T>(string assetName) where T : UnityEngine.Object
    {
        var paths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetName, assetName);
        if (paths.Length == 0)
        {
            LogAssetEmpty(assetName, typeof(T).ToString());
        }
        var result = AssetDatabase.LoadAssetAtPath<T>(paths[0]);
        return result;
    }
#endif

    /// <summary>
    /// 同步加载assetbundle及其依赖，并缓存
    /// </summary>
    /// <param name="assetBundleName">bundle名</param>
    /// <returns></returns>
    static LoadedAssetBundle GetAssetBundle(string assetBundleName)
    {
        LoadedAssetBundle bundle = null;
        if (!m_abCaches.TryGetValue(assetBundleName, out bundle))
        {
            string path = null;
            if (m_abPaths.TryGetValue(assetBundleName, out path))
            {
                LoadDepend(assetBundleName);
                bundle = LoadBundle(assetBundleName);
            }
        }
// todo log
        return bundle;
    }

    /// <summary>
    /// 加载资源依赖
    /// </summary>
    /// <param name="assetBundleName">bundle名</param>
    static void LoadDepend(string assetBundleName)
    {
        string[] depends = null;
        if (!m_depends.TryGetValue(assetBundleName, out depends))
        {
            depends = m_manifest.GetAllDependencies(assetBundleName);
            m_depends.Add(assetBundleName, depends);
        }
        for (int i = 0; i < depends.Length; i++)
        {
            LoadBundle(depends[i]);
        }
    }

    static LoadedAssetBundle LoadBundle(string assetBundleName)
    {
        LoadedAssetBundle loaded = null;
        if (!m_abCaches.TryGetValue(assetBundleName, out loaded))
        {
            string path;
            if (!m_abPaths.TryGetValue(assetBundleName, out path))
            {
#if UNITY_EDITOR
                LogBundleDtExist(assetBundleName);
#endif
                return null;
            }

            var ab = AssetBundle.LoadFromFile(path);
            loaded = LoadedBundlePool.Get(ab);
            m_abCaches.Add(assetBundleName, loaded);
        }
        loaded.AddReferenceCount();
        return loaded;
    }

    /// <summary>
    /// 加载资源及依赖
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="onBundlesLoaded"> 回调</param>
    static void LoadBundleAndDepends(string assetName, System.Action<AssetBundle> onBundlesLoaded)
    {
        string[] depends = null;
        if (!m_depends.TryGetValue(assetName, out depends))
        {
            depends = m_manifest.GetAllDependencies(assetName);
            m_depends.Add(assetName, depends);
        }

        var all = depends.Length + 1;
        var over = 0;

        System.Action<LoadedAssetBundle> onBundleLoaded = (loaded) =>
        {
            over++;
            loaded.AddReferenceCount();
            if (over == all)
            {
                onBundlesLoaded(m_abCaches[assetName].assetBundle);
            }
        };

        LoadAssetBundleAsync(assetName, onBundleLoaded);
        for (int i = 0; i < depends.Length; i++)
        {
            LoadAssetBundleAsync(depends[i], onBundleLoaded);
        }
    }

    /// <summary>
    /// 異步加載單個ab 完成后調用回掉
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <param name="onBundleLoaded"></param>
    static void LoadAssetBundleAsync(string assetBundleName, System.Action<LoadedAssetBundle> onBundleLoaded)
    {
    
        LoadedAssetBundle loaded = null;
        if (m_abCaches.TryGetValue(assetBundleName, out loaded))
        {
            onBundleLoaded(loaded);
        }
        else
        {
            string bundlePath = m_abPaths[assetBundleName];
            RequestHandler handler = null;
            if (!m_requestHandlers.TryGetValue(bundlePath, out handler))
            {
                var request = AssetBundle.LoadFromFileAsync(bundlePath);
                handler = MAssetBundleCreateRequestPool.Get(request);
                handler.onRequestDone += () =>
                {
                    loaded = LoadedBundlePool.Get((handler as MAssetBundleCreateRequest).assetBundle);
                    m_abCaches.Add(assetBundleName, loaded);
                    onBundleLoaded(loaded);
                };
                m_requestHandlers.Add(bundlePath, handler);
            }
            else
            {
                handler.onRequestDone += () =>
                {
                    onBundleLoaded(loaded);
                };
            }
        }
    }

    /// <summary>
    /// 從某個bundle中取出資源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="assetName">资源名</param>
    /// <param name="bundle">bundle</param>
    /// <param name="onAssetLoaded">回调</param>
    static void GetAssetAsync<T>(string assetName, AssetBundle bundle, System.Action<T> onAssetLoaded) where T : UnityEngine.Object
    {
        var request = bundle.LoadAssetAsync<T>(assetName);
        if (request.isDone)
        {
            TryGetAsset(assetName, onAssetLoaded, request.asset);
        }
        else
        {
            RequestHandler handler = null;

            // 已經申請加載，正在加載中
            if (m_requestHandlers.TryGetValue(assetName, out handler))
            {
                handler.onRequestDone += () =>
                {
                    TryGetAsset(assetName, onAssetLoaded, (handler as MAssetBundleRequest).asset);
                };
            }

            // 還未申請加載
            else
            {
                handler = MAssetBundleRequestPool.Get(request);
                handler.onRequestDone += () =>
                {
                    TryGetAsset(assetName, onAssetLoaded, (handler as MAssetBundleRequest).asset);
                };
                m_requestHandlers.Add(assetName, handler);
            }
        }
    }

    static void TryGetAsset<T>(string assetName, System.Action<T> onAssetLoaded, Object asset) where T : UnityEngine.Object
    {

#if UNITY_EDITOR

        if (asset == null || asset as T == null)
        {
            LogAssetEmpty(assetName, typeof(T).ToString());
        }
#endif
        onAssetLoaded(asset as T);
    }

    /// <summary>
    /// 获取不同平台下的manifest路径
    /// </summary>
    /// <returns></returns>
    static string GetManifestPath()
    {
        return string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, PREFILE, PREFILE);
    }


    /// <summary>
    /// 异步加载处理（不使用协程直接加载,为了减少在资源加载峰值时产生的无用GC）
    /// </summary>
    static void LoadAsyncHandler()
    {
        if (m_requestHandlers.Count > 0)
        {
            var keys = ListPool<string>.Get();
            var needDeleteHandlers = ListPool<string>.Get();

            keys.AddRange(m_requestHandlers.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                var handler = m_requestHandlers[keys[i]];
                if (handler.isDone)
                {
                    handler.InvokeCallback();
                    needDeleteHandlers.Add(keys[i]);
                }
            }

            while (needDeleteHandlers.Count > 0)
            {
                var last = needDeleteHandlers.Count - 1;
                var key = needDeleteHandlers[last];
                var handler = m_requestHandlers[key];
                if (handler is MAssetBundleCreateRequest)
                {
                    MAssetBundleCreateRequestPool.Put(handler as MAssetBundleCreateRequest);
                }
                else
                {
                    MAssetBundleRequestPool.Put(handler as MAssetBundleRequest);
                }
                m_requestHandlers.Remove(key);
                needDeleteHandlers.RemoveAt(last);
            }
            ListPool<string>.Put(keys);
            ListPool<string>.Put(needDeleteHandlers);
        }
    }


    #endregion

    #region 容错方法

    static void LogBundleDtExist(string bundleName)
    {
        Debug.LogErrorFormat("AssetBundle what's bundleName = {0} don't exist", bundleName);
    }


    static void LogAssetEmpty(string assetName, string type)
    {
        Debug.LogErrorFormat("nonexistent assetname = {0}  or  unconformable type(T) = {1} ", assetName, type);
    }

    #endregion

}
