using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace AssetRuntime
{
    /// <summary>
    /// manifest暂时使用同步加载方案，如果出现问题，可以改成异步加载
    /// </summary>
    public class ManifestHelper
    {
        string m_manifestPath;
        AssetBundle m_manifestBundle;
        AssetBundleManifest m_manifest;


#if UNITY_ANDROID
        string manifistName = "android";
#elif UNITY_IOS
        string  manifistName = "ios"
#else
        string manifistName = "android";
#endif

        public ManifestHelper()
        {
            m_manifestPath = AssetUtils.GetValidFilePath(manifistName);
            m_manifestBundle = AssetBundle.LoadFromFile(m_manifestPath);
            m_manifest = m_manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        public void ReStart(Version version)
        {
            m_manifestBundle.Unload(true);
            m_manifestPath = version.GetFilePath(manifistName);
            m_manifestBundle = AssetBundle.LoadFromFile(m_manifestPath);
            m_manifest = m_manifestBundle.LoadAsset<AssetBundleManifest>(manifistName);
        }

        public string[] GetAllDependencies(string assetBundleName)
        {
            return m_manifest.GetAllDependencies(assetBundleName);
        }
    }
}