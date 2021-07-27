using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;

public class BuildAssetBundle
{
    static int BundleStartWith = 0;

    static string Combine(string str1, string str2)
    {
        return string.Format("{0}/{1}", str1, str2);
    }

    [MenuItem("AssetBundle/BuildAndroid")]
    static void BuildAndroid()
    {
        var target = BuildTarget.Android;
        var outpath = Combine(GetWebServerPath(), GetPlatformPath(target));
        UpdateFileIndex(outpath);

        var assetMap = CollectAssetMap();
        WriteAssetMap(assetMap, outpath);

        CheckOutputPath(outpath);
        BuildBundle(outpath, target);

        var map =  CollectBundleMap(outpath);
        WriteBundleMap(map, outpath);
    }

    [MenuItem("AssetBundle/Test")]
    static void Test()
    {

    }


    static void UpdateFileIndex(string outputPath)
    {
        BundleStartWith = outputPath.Length;
    }
    /// <summary>
    /// 搜集资源映射
    /// </summary>
    /// <returns></returns>
    static Dictionary<string, string> CollectAssetMap()
    {
        Dictionary<string, string> assetToBundleMap = new Dictionary<string, string>();
        var bundles = AssetDatabase.GetAllAssetBundleNames();
        foreach (var bundle in bundles)
        {
            var assets = AssetDatabase.GetAssetPathsFromAssetBundle(bundle);
            foreach (var asset in assets)
            {
                assetToBundleMap.Add(Path.GetFileNameWithoutExtension(asset),  bundle);
            }
        }
        return assetToBundleMap;
    }

    static Dictionary<string, BundleInfo> CollectBundleMap(string outputPath)
    {
        Dictionary<string, BundleInfo> map = new Dictionary<string, BundleInfo>();
        DirectoryInfo dir = new DirectoryInfo(outputPath);
        var files = dir.GetFiles();
        foreach (var file in files)
        {
            var fullName = file.FullName;
            var fileName = Path.GetFileNameWithoutExtension(fullName);
            if (fullName.EndsWith(".manifest") || 
                fileName == AssetUtils.FileDetail ||
                fileName == AssetUtils.AssetMap) continue;

            BundleInfo info = new BundleInfo();
            info.BundleName = GetBundleName(fullName);
            Debug.Log(info.BundleName);
            info.md5 = AssetUtils.GetFileMD5ByPath(file.FullName);
            info.size = file.Length;

            map.Add(info.BundleName, info);
        }
        return map;
    }

    /// <summary>
    /// 通过资源路径获取资源名
    /// </summary>
    /// <param name="assetPath"></param>
    /// <returns></returns>
    static string GetAssetFileName (string assetPath)
    {
        if (assetPath.StartsWith("Assets/AssetBundles"))
        {
            return Path.GetFileNameWithoutExtension(assetPath);
        }
        return null;
    }

    /// <summary>
    /// 构建assetbundle
    /// </summary>
    /// <param name="buildTarget"></param>
    /// <returns></returns>
    static AssetBundleManifest BuildBundle(string outputPath, BuildTarget target)
    {
        StringBuilder assetRecord = new StringBuilder();
        StringBuilder bundlePathRecord = new StringBuilder();
        var manifist = BuildPipeline.BuildAssetBundles(
            outputPath,
            BuildAssetBundleOptions.UncompressedAssetBundle
            |BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension
            , target);
        return manifist;
    }

    /// <summary>
    /// 检查输出路径
    /// </summary>
    /// <param name="path"></param>
    static void CheckOutputPath(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    /// <summary>
    /// 获取webserver路径
    /// </summary>
    /// <returns></returns>
    static string GetWebServerPath()
    {
        var dataPath = Application.dataPath;
        var projectPath = dataPath.Remove(dataPath.Length - "Assets".Length);
        return Combine(projectPath, "Tools/WebServer/Assets");
    }

    /// <summary>
    /// 获取平台文件夹
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    static string GetPlatformPath(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android: return "android";
            case BuildTarget.iOS:return "ios";
            default:
              
                break;
        }
        return "";
    }

    static string GetBundleName(string bundlePath)
    {
        return bundlePath.Replace("\\", "/").Substring(BundleStartWith);
    }

    class BundleInfo
    {
        public string BundleName;
        public long size;
        public string md5;
    }

    static void WriteBundleMap(Dictionary<string, BundleInfo> map, string outputPath)
    {
        var filePath = Combine(outputPath, AssetUtils.FileDetail);
        FileInfo file = new FileInfo(filePath);
        if (file.Exists)
        {
            file.Delete();
        }
        var stream = file.CreateText();

        StringBuilder sb = new StringBuilder();
        foreach (var item in map)
        {
            var bundleInfo = item.Value;
            sb.AppendLine(string.Format("{0}|{1}|{2}", bundleInfo.BundleName, bundleInfo.size, bundleInfo.md5));
        }

        stream.Write(sb.ToString());
        stream.Flush();
        stream.Close();
      
    }

    /// <summary>
    /// 写入资源和bundle的映射
    /// </summary>
    /// <param name="map"></param>
    /// <param name="target"></param>
    static void WriteAssetMap(Dictionary<string, string> map, string outputPath)
    {
        var filePath = Combine(outputPath, AssetUtils.AssetMap);
        FileInfo file = new FileInfo(filePath);
        if (file.Exists)
        {
            file.Delete();
            
        }

        var stream = file.CreateText();

        StringBuilder sb = new StringBuilder();
        foreach (var item in map)
        {
            var assetName = item.Key;
            var bundleName = item.Value;
            sb.AppendLine(string.Format("{0}|{1}", assetName, bundleName));
        }
        stream.Write(sb.ToString());
        stream.Flush();
        stream.Close();
    }
}
