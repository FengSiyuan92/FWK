using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace AssetRuntime
{
    public partial class Bundle : AsyncTemplate<LoadedBundle>
    {
        static Dictionary<string, LoadedBundle> m_abCaches;
        static Dictionary<string, string> m_abPaths;
        static ManifestHelper m_manifest;
        static Dictionary<string, string[]> m_depends;
        static ReuseObjectPool<MLoadBundleRequest> m_requestPool = new ReuseObjectPool<MLoadBundleRequest>();
        static ReuseObjectPool<LoadBundleNote> m_notePool = new ReuseObjectPool<LoadBundleNote>();
        static BundleMap m_BundleMap;

        /// <summary>
        /// 初始化方法
        /// </summary>
        public static void Initialize(BundleMap bundleMap)
        {
            // 容器初始化
            m_abCaches = new Dictionary<string, LoadedBundle>(500);
            m_abPaths = new Dictionary<string, string>(500);

            m_depends = new Dictionary<string, string[]>(100);
            m_BundleMap = bundleMap;

            // manifest初始化
            m_manifest = new ManifestHelper();
            m_manifest.InitAssetPath(m_abPaths);
        }

        /// <summary>
        /// 异步加载bundle及其依赖的bundle,并返回指定bundle
        /// </summary>
        /// <param name="assetName">资源名</param>
        /// <param name="onBundlesLoaded"> 回调</param>
        internal static void LoadBundleAsync(string bundleName, IRequestNote note)
        {
            string[] depends = GetDepends(bundleName);
            var depCount = depends == null ? 0 : depends.Length;

            var bundleNote = m_notePool.Get();
            bundleNote.mainBundleName = bundleName;
            bundleNote.dependCount = depCount;
            bundleNote.onBundlesLoaded = note.RequestOver;

            for (int i = 0; i < depCount; i++)
            {
                GetOrRequestLoaded(depends[i], note, CreateLoadBundleRequest);
            }
            GetOrRequestLoaded(bundleName, note, CreateLoadBundleRequest);
        }

        static string[] GetDepends(string mainBundleName)
        {
            string[] depends = null;
            if (!m_depends.TryGetValue(mainBundleName, out depends))
            {
                depends = m_manifest.GetAllDependencies(mainBundleName);
                m_depends.Add(mainBundleName, depends);
            }
            return depends;
        }

        static LoadedBundle GetLoadedtBundle(string bundleName)
        {
            LoadedBundle bundle = null;
            m_abCaches.TryGetValue(bundleName, out bundle);
            return bundle;
        }

        static AsyncRequest CreateLoadBundleRequest(string targetName, IRequestNote note)
        {
            var operation = AssetBundle.LoadFromFileAsync(m_abPaths[targetName]);

            var loadBundleRequest = m_requestPool.Get();
            loadBundleRequest.SetAsynOperation(operation);

            LoadedCreater loadedCreater = () =>
            {
                var loaded = new LoadedBundle();
                loaded.SetBundle(loadBundleRequest.assetBundle);
                m_abCaches.Add(targetName, loaded);
                return loaded;
            };

            loadBundleRequest.SetLoadedCreater(loadedCreater);
            return loadBundleRequest;
        }

    }
}