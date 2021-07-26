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

namespace AssetRuntime
{
    public partial class Asset : AsyncTemplate<LoadedAsset>
    {
        static Dictionary<string, LoadedAsset> m_LoadedAsset;
        static ReuseObjectPool<MLoadAssetRequest> m_RequestPool = new ReuseObjectPool<MLoadAssetRequest>();
        static ReuseObjectPool<LoadAssetNote> m_LoadAssetNotePool = new ReuseObjectPool<LoadAssetNote>();
        static ReuseObjectPool<GetAssetNote> m_GetAssetNotePool = new ReuseObjectPool<GetAssetNote>();

        static AssetMap m_AssetMap;

        public static void Initialize()
        {
            m_AssetMap = new AssetMap(null);
        }
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
            string bundlePath = m_AssetMap.GetAssetBundleName(assetName);
            if (bundlePath == null)
            {
                AssetUtils.LogAssetEmpty(assetName, typeof(Object).ToString());
                return;
            }

            var loadAssetNote = new LoadAssetNote();
            loadAssetNote.onAssetLoaded = callback;
            loadAssetNote.targetAssetName = assetName;

            Bundle.LoadBundleAsync(bundlePath, loadAssetNote);
        }

        public static void ReturnAsset(string assetName)
        {
            LoadedAsset loaded = null;
            if (m_LoadedAsset.TryGetValue(assetName, out loaded))
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
                m_LoadedAsset.Add(targetName, loaded);
                return loaded;
            };

            loadAssetHandler.SetLoadedCreater(creater);
            return loadAssetHandler;
        }

    }
}