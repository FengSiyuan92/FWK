using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// manifest暂时使用同步加载方案，如果出现问题，可以改成异步加载
/// </summary>
public class ManifestHelper : MonoBehaviour
{
    // 这个字段可以根据宏定义来确定不同平台下的不同文件夹
    const string PREFILE = "StandaloneWindows";
    const string manifest = "assetbundlemanifest";

    string m_manifestPath;
    string m_BaseDownloadingURL;
    string m_bundlePre;
    AssetBundle m_manifestBundle;
    AssetBundleManifest m_manifest;

    public ManifestHelper()
    {
        m_manifestPath = GetManifestPath();
        m_BaseDownloadingURL = AssetUtils.GetRelativePath();
        Debug.LogFormat("m_manifestPath = {0} || m_BaseDownloadingURL = {1}", m_manifestPath, m_BaseDownloadingURL);

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
        m_bundlePre = Application.streamingAssetsPath + "/" + PREFILE;
        var allABs = m_manifest.GetAllAssetBundles();
        for (int i = 0; i < allABs.Length; i++)
        {
            vessel.Add(allABs[i], Path.Combine(m_bundlePre, allABs[i]));
        }
    }

    /// <summary>
    /// 获取不同平台下的manifest路径
    /// </summary>
    /// <returns></returns>
    static string GetManifestPath()
    {
        return Path.Combine(Application.streamingAssetsPath, PREFILE, PREFILE);
    }

}
