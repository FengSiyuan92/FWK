using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BundleManager
{
    #region 常量， 字段， 容器定义
    static Dictionary<string, LoadedAssetBundle> m_abCaches;
    static Dictionary<string, string> m_abPaths;
    static ManifestHelper m_manifest;
    static Dictionary<string, RequestHandler> m_requestHandlers;
    static Dictionary<string, string[]> m_depends;


    #endregion

    #region 对外接口

    /// <summary>
    /// 初始化方法
    /// </summary>
    public static void Initialize()
    {
        // 容器初始化
        m_abCaches = new Dictionary<string, LoadedAssetBundle>(100);
        m_abPaths = new Dictionary<string, string>();
        m_requestHandlers = new Dictionary<string, RequestHandler>(5);
        m_depends = new Dictionary<string, string[]>();

        // manifest初始化
        m_manifest = new ManifestHelper();
        m_manifest.InitAssetPath(m_abPaths);

       
    }

    #endregion


    #region 同步接口
    /// <summary>
    /// 同步加载assetbundle及其依赖，并缓存
    /// </summary>
    /// <param name="assetBundleName">bundle名</param>
    /// <returns></returns>
    public static LoadedAssetBundle GetAssetBundle(string assetBundlePath)
    {
        LoadedAssetBundle bundle = null;
        if (!m_abCaches.TryGetValue(assetBundlePath, out bundle))
        {
            string path = null;
            if (m_abPaths.TryGetValue(assetBundlePath, out path))
            {
                LoadDepend(assetBundlePath);
                bundle = LoadBundle(assetBundlePath);
            }
            else
            {
                AssetUtils.LogBundleDtExist(assetBundlePath);
            }
        }
        // todo log
        return bundle;
    }

    public static LoadedAssetBundle GetLoadedAssetBundle(string assetBundlePath)
    {
        LoadedAssetBundle bundle = null;
        m_abCaches.TryGetValue(assetBundlePath, out bundle);
        return bundle;
    }

    #endregion


    #region 资源加载内部方法

    /// <summary>
    /// 加载资源依赖
    /// </summary>
    /// <param name="assetBundleName">bundle名</param>
    static void LoadDepend(string bundlePath)
    {
        string[] depends = null;
        if (!m_depends.TryGetValue(bundlePath, out depends))
        {
            depends = m_manifest.GetAllDependencies(bundlePath);
            m_depends.Add(bundlePath, depends);
        }
        for (int i = 0; i < depends.Length; i++)
        {
            LoadBundle(depends[i]);
        }
    }
    // 同步加载一个bundle
    static LoadedAssetBundle LoadBundle(string bundlePath)
    {
        LoadedAssetBundle loaded = null;
        if (!m_abCaches.TryGetValue(bundlePath, out loaded))
        {
            string path;
            if (!m_abPaths.TryGetValue(bundlePath, out path))
            {
                AssetUtils.LogBundleDtExist(bundlePath);
                return null;
            }
            var ab = AssetBundle.LoadFromFile(path);
            loaded = LoadedBundlePool.Get(ab);
            m_abCaches.Add(bundlePath, loaded);
        }
        loaded.AddReferenceCount();
        return loaded;
    }

    /// <summary>
    /// 异步加载bundle及其依赖的bundle,并返回指定bundle
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="onBundlesLoaded"> 回调</param>
    internal static void LoadBundleAsync(string bundlePath, RequestNote note)
    {
        string[] depends = null;
        if (!m_depends.TryGetValue(bundlePath, out depends))
        {
            depends = m_manifest.GetAllDependencies(bundlePath);
            m_depends.Add(bundlePath, depends);
        }

        var bundleNote = new LoadBundleNote();
        bundleNote.mainBundlePath = bundlePath;
        bundleNote.dependCount = depends.Length;
        bundleNote.onBundlesLoaded = note.RequestOver;

        for (int i = 0; i < depends.Length; i++)
        {
            LoadAssetBundleAsyncInternal(depends[i], note);
        }
        LoadAssetBundleAsyncInternal(bundlePath, note);
    }

    /// <summary>
    /// 异步加载一个bundle及其依赖的bundle
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <param name="onBundleLoaded"></param>
    static void LoadAssetBundleAsyncInternal(string bundlePath, RequestNote note)
    {
        LoadedAssetBundle loaded = null;
        // 如果bundle已经加载过,并且缓存起来了,直接取出对应缓存,调用回调即可
        if (m_abCaches.TryGetValue(bundlePath, out loaded))
        {
            note.RequestOver(loaded);
            return;
        }

        string fullBundlePath = m_abPaths[bundlePath];
        RequestHandler handler = RequestHandleDrive.GetRequestHandler(bundlePath);

        // 如果当前bundle没有加载中,则开始加载bundle,并且关联对应回调
        if (handler == null)
        {
            var request = AssetBundle.LoadFromFileAsync(bundlePath);
            handler = MAssetBundleCreateRequestPool.Get(request);
            var loadBundleHandler = MAssetBundleCreateRequestPool.Get(request);

            loadBundleHandler.createLoaded = () =>
            {
                loaded = LoadedBundlePool.Get(loadBundleHandler.assetBundle);
                m_abCaches.Add(bundlePath, loaded);
                return loaded;
            };

            handler = loadBundleHandler;
            m_requestHandlers.Add(bundlePath, handler);
        }

        handler.onRequestDone += note.RequestOver;
    }


    #endregion
}
