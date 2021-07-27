using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace AssetRuntime
{
    public partial class Asset : AsyncTemplate<LoadedAsset>
    {

        static ReuseObjectPool<MLoadAssetRequest> m_RequestPool;
        static ReuseObjectPool<LoadAssetNote> m_LoadAssetNotePool;
        static ReuseObjectPool<GetAssetNote> m_GetAssetNotePool;

        static AssetMap m_AssetMap;

        public static void Initialize()
        {
            m_AssetMap = new AssetMap();
            m_loadedCache = new Dictionary<string, LoadedAsset>(300);
            m_RequestPool = new ReuseObjectPool<MLoadAssetRequest>(100);
            m_LoadAssetNotePool= new ReuseObjectPool<LoadAssetNote>();
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
        public static void GetAssetAsync(string assetName, System.Action<Object> callback)
        {
          
#if UNITY_EDITOR
            if (!AssetSimulate.USE_ASSET_BUNDLE)
            {
                var asset = LoadAssetInEditor(assetName);
                SafeCall.Call(callback, asset);
                return;
            }
#endif
            // 获取一个已经加载好的资源
            var loaded = GetLoadedAsset(assetName);
            if (loaded != null)
            {
                loaded.AddReferenceCount();
                SafeCall.Call(callback, loaded.Asset);
                return;
            }
            
            //  开始异步加载asset
            string bundlePath = m_AssetMap.GetAssetBundleName(assetName);
            if (bundlePath == null)
            {
                throw new System.Exception("资源映射中没有找到所在bundle,检查是否生成过资源映射");
            }

            var loadAssetNote = m_LoadAssetNotePool.Get();
            loadAssetNote.onAssetLoaded = callback;
            loadAssetNote.targetAssetName = assetName;

            Bundle.LoadBundleAsync(bundlePath, loadAssetNote);
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