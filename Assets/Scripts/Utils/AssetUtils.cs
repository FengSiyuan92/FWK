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
    public static string GetStringMD5(string source)
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
    public static string GetFileMD5ByPath(string file)
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

        Debug.Log("m_PersistentPath = " + m_PersistentPath);
        Debug.Log("m_StreamAssetPath = " + m_StreamAssetPath);
    }
}
