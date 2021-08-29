﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using UnityEngine.U2D;
using UnityEditor.U2D;

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
        var outpath = AssetUtils.GetWerServerBundlePath();
        UpdateFileIndex(outpath);
        //导出环境检查
        CheckOutputPath(outpath);

        //构建Sprite映射
        var spriteMap = CollectSpriteMap();
        WriteMap(spriteMap, Combine(outpath, AssetUtils.SpriteMap));

        //构建资源映射
        var assetMap = CollectAssetMap();
        WriteMap(assetMap, Combine(outpath, AssetUtils.AssetMap));

        // 构建bundle包
        BuildBundle(outpath, target);

        // TODO:导出luazip包
        GenLuaScript(outpath);

        // 收集收集版本文件
        var map =  CollectFileInfos(outpath);
        WriteFileInfos(map, outpath);
    }

    [MenuItem("AssetBundle/Test")]
    static void Test()
    {
        CollectSpriteMap();
    }


    static void UpdateFileIndex(string outputPath)
    {
        BundleStartWith = outputPath.Length;
    }



    static Dictionary<string, string> CollectSpriteMap()
    {
        var cache = new Dictionary<string, string>();

        var all = AssetDatabase.FindAssets("t:spriteatlas");
        foreach (var guid in all)
        {

            var path = AssetDatabase.GUIDToAssetPath(guid);
            var import = AssetImporter.GetAtPath(path);
            var abName = import.assetBundleName;
            if (string.IsNullOrEmpty(abName)) continue;
            var asset = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
            var packages = asset.GetPackables();
            var atlasName = Path.GetFileNameWithoutExtension(path);

            foreach (var inpack in packages)
            {
                var fullPath = GetFullPath(AssetDatabase.GetAssetPath(inpack));
                // 文件夹需要取文件夹内的sprite
                if (inpack is DefaultAsset)
                {
                    var director = Directory.CreateDirectory(fullPath);
                    var filePath = director.GetFiles();
                    foreach (var file in filePath)
                    {
                        if (file.FullName.EndsWith(".meta"))
                        {
                            continue;
                        }
                        var fileName = Path.GetFileNameWithoutExtension(file.FullName);
                        if (!cache.ContainsKey(fileName))
                        {
                            cache.Add(fileName, atlasName);
                        }
                       
                    }
                }
                else
                {
                    var fileName = Path.GetFileNameWithoutExtension(fullPath);
                    if (!cache.ContainsKey(fileName))
                    {
                        cache.Add(fileName, atlasName);
                    }
               
                }
            }
        }
   
        return cache;
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

    static Dictionary<string, FileInfo> CollectFileInfos(string outputPath)
    {
        Dictionary<string, FileInfo> map = new Dictionary<string, FileInfo>();

        var files = Directory.GetFiles(outputPath, "*.", SearchOption.AllDirectories);
    
        foreach (var filePath in files)
        {
            if (Directory.Exists(filePath))
            {
                continue;
            }
            var file = new System.IO.FileInfo(filePath);
            var fullName = file.FullName;
            var fileName = Path.GetFileNameWithoutExtension(fullName);
            if (fullName.EndsWith(".manifest") || fileName == AssetUtils.FileDetail ||fileName == AssetUtils.VersionFileName) continue;

            FileInfo info = new FileInfo();
            info.BundleName = GetBundleName(fullName);
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
        return GetFullPath("/Tools/WebServer/Assets");
    }

    static string GetFullPath(string assetDataPath)
    {
        var dataPath = Application.dataPath;
        var projectPath = dataPath.Remove(dataPath.Length - "Assets".Length - 1);
        return Combine(projectPath, assetDataPath);
    }
    static string GetBundleName(string bundlePath)
    {
        return bundlePath.Replace("\\", "/").Substring(BundleStartWith);
    }

    class FileInfo
    {
        public string BundleName;
        public long size;
        public string md5;
    }

    static void WriteFileInfos(Dictionary<string, FileInfo> map, string outputPath)
    {
        var filePath = Combine(outputPath, AssetUtils.FileDetail);
        System.IO.FileInfo file = new System.IO.FileInfo(filePath);
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
    static void WriteMap(Dictionary<string, string> map, string filePath)
    {

        System.IO.FileInfo file = new System.IO.FileInfo(filePath);
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

    static void WriteSpriteMap(Dictionary<string, string> map, string outputPath)
    {

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
