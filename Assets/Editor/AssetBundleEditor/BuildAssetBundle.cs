using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;



public class BuildAssetBundle
{
    const string LuaCodeDir = "LuaCode";

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

        //构建资源映射
        var assetMap = CollectAssetMap();
        WriteAssetMap(assetMap, outpath);

        //导出环境检查
        CheckOutputPath(outpath);
        BuildBundle(outpath, target);

        // 导出luazip包
        GenLuaScript(outpath);

        // 收集收集版本文件
        var map =  CollectFileMap(outpath);
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

    static Dictionary<string, BundleInfo> CollectFileMap(string outputPath)
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
                fileName == AssetUtils.AssetMap ||
                fileName == AssetUtils.VersionFileName) continue;

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
        var manifist = BuildPipeline.BuildAssetBundles(
            outputPath,BuildAssetBundleOptions.ChunkBasedCompression, target);
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
    /// 写入资源和对应bundle的映射
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

    /// <summary>
    /// 把lua代码打包并加密
    /// </summary>
    static void GenLuaScript(string outputDir)
    {
        var path = Application.dataPath + "/" + LuaCodeDir;
        var frameWorkPath = path + "/Framework";
        var logicPath = path + "/Scripts";


        var frameWorkFile = outputDir + "/" + AssetUtils.GetStringMD5(AssetUtils.LuaFramework);
        var logicFile = outputDir + "/" + AssetUtils.GetStringMD5(AssetUtils.LuaScripts);

    }
}
