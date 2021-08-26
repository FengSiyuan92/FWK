using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace AssetRuntime
{
    /// <summary>
    /// 支持加载资源的4个时间点:(应该是任何时间节点了)
    /// 1. 完全没有申请过加载时,第一次申请加载
    /// 2. 第一申请异步,进行到加载bundle中,再次申请加载资源
    /// 3. 已经加载完bundle,但是正在异步解压资源时,再次申请加载资源
    /// 4. 已经解压好资源,再次申请
    /// </summary>
    public partial class Asset : AsyncTemplate<LoadedAsset>
    {

        static ReuseObjectPool<MLoadAssetRequest> m_RequestPool;
        static ReuseObjectPool<LoadBundleRequestNote> m_RequestBundleNotePool;
        static ReuseObjectPool<GetAssetNote> m_GetAssetNotePool;
        static AssetMap m_AssetMap;

        public static void Initialize()
        {
            m_AssetMap = new AssetMap();
            m_loadedCache = new Dictionary<string, LoadedAsset>(300);
            m_RequestPool = new ReuseObjectPool<MLoadAssetRequest>(100);
            m_RequestBundleNotePool= new ReuseObjectPool<LoadBundleRequestNote>();
            m_GetAssetNotePool = new ReuseObjectPool<GetAssetNote>();
        }

        public static void ReStart()
        {
            m_AssetMap = new AssetMap();

        }

#if UNITY_EDITOR
        static string[] assetsFolder = new string[] { "AssetBundles" };

        static Object LoadAssetInEditor(string assetName)
        {
            string[] paths = AssetDatabase.FindAssets(assetName, assetsFolder);
            if (paths.Length == 0)
            {
                throw new System.Exception("没有找到对应资源");
     
            }
            var result = AssetDatabase.LoadAssetAtPath<Object>(paths[0]);
            return result;
        }
#endif

        public void ReduceAssetReferenct()
        {
          
        }

        static LoadedAsset GetLoadedAsset(string assetName)
        {
            LoadedAsset loaded;
            m_loadedCache.TryGetValue(assetName, out loaded);
            return loaded;
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T">需要加载的资源类型</typeparam>
        /// <param name="assetName">资源名称</param>
        /// <param name="callback">当资源加载完毕之后的回调</param>
        public static InterruptNote GetAssetAsync(string assetName, System.Action<Object> callback)
        {
#if UNITY_EDITOR
            if (!AssetSimulate.USE_ASSET_BUNDLE)
            {
                var asset = LoadAssetInEditor(assetName);
                SafeCall.Call(callback, asset);
                return null;
            }
#endif
            // 获取一个已经加载好的资源
            var loaded = GetLoadedAsset(assetName);
            if (loaded != null)
            {
                loaded.AddReferenceCount();
                SafeCall.Call(callback, loaded.Asset);
                return null;
            }
            
            //  开始异步加载asset
            string bundlePath = m_AssetMap.GetAssetBundleName(assetName);
            if (bundlePath == null)
            {
                throw new System.Exception("资源映射中没有找到所在bundle,检查是否生成过资源映射");
            }

            var loadBundleRequestNote = m_RequestBundleNotePool.Get();
            loadBundleRequestNote.onAssetLoaded = callback;
            loadBundleRequestNote.targetAssetName = assetName;

            Bundle.LoadBundleAsync(bundlePath, loadBundleRequestNote);
            return loadBundleRequestNote;
        }

        public static void ReturnAsset(string assetName)
        {
            var loaded = GetLoadedAsset(assetName);
            if (loaded != null)
            {
                loaded.TryUnload();
            }
        }

        static AsyncRequest CreateLoadAssetHandler(string targetName, IRequestNote note)
        {
            var getAssetNote = note as GetAssetNote;
            var operation = getAssetNote.loadedBundle.Bundle.LoadAssetAsync(targetName);

            var loadAssetHandler = m_RequestPool.Get();
            loadAssetHandler.SetAsynOperation(operation);

            LoadedCreater creater = () =>
            {
                var loaded = new LoadedAsset();
                loaded.SetInfo(getAssetNote.loadedBundle, loadAssetHandler.Asset);
                m_loadedCache.Add(targetName, loaded);
                return loaded;
            };

            loadAssetHandler.SetLoadedCreater(creater);
            return loadAssetHandler;
        }

        static string GetHandlerName(string targetName)
        {
            return targetName;
        }

    }
}