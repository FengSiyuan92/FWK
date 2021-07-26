using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AssetUtils {
    public const string AppName = "";           
    /// <summary>
    /// 计算字符串的MD5值
    /// </summary>
    public static string md5(string source)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        string destString = "";
        for (int i = 0; i < md5Data.Length; i++)
        {
            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        destString = destString.PadLeft(32, '0');
        return destString;
    }

    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string GetMd5(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }

    static string m_PersistentPath;
    public static string PersistentPath => m_PersistentPath;

    static string m_StreamAssetPath;
    public static string StreamAssetPath => m_StreamAssetPath;


    public static string GetStreamFilePath(string filePath)
    {
        Debug.Log("m_StreamAssetPath = " + m_StreamAssetPath);
        return string.Format("file://{0}/{1}", m_StreamAssetPath, filePath);
    }


    public static string GetPersistentFilePath(string filePath)
    {
        Debug.Log("m_PersistentPath = " + m_PersistentPath);
        return string.Format("file://{0}/{1}", m_PersistentPath, filePath);
    }
}

    /// <summary>
    /// 重置加载的资源的shader，解决加载资源shader丢失的问题
    /// </summary>
    /// <param name="obj"></param>
    public static void ResetShader(UnityEngine.Object obj)
    {

        List<Material> listMat = new List<Material>();
        if (obj is Material)
        {

            Material m = obj as Material;

            listMat.Add(m);

        }
        else if (obj is GameObject)
        {
            GameObject go = obj as GameObject;
            Renderer[] rends = go.GetComponentsInChildren<Renderer>();
            if (null != rends)
            {
                foreach (Renderer item in rends)
                {
                    Material[] materialsArr = item.sharedMaterials;
                    foreach (Material m in materialsArr)
                        listMat.Add(m);
                }
            }
        }
        for (int i = 0; i < listMat.Count; i++)
        {
            Material m = listMat[i];
            if (null == m)
                continue;
            var shaderName = m.shader.name;
            var newShader = Shader.Find(shaderName);
            if (newShader != null)
                m.shader = newShader;
        }
    }

    public static void LogBundleDtExist(string bundleName)
    {
        Debug.LogErrorFormat("AssetBundle what's bundleName = {0} don't exist", bundleName);
    }


    public static void LogAssetEmpty(string assetName, string type)
    {
        Debug.LogErrorFormat("nonexistent assetname = {0}  or  unconformable type(T) = {1} ", assetName, type);
    }

    static AssetUtils()
    {
        m_PersistentPath = Application.persistentDataPath;

        m_StreamAssetPath = Application.streamingAssetsPath;
    }
}
