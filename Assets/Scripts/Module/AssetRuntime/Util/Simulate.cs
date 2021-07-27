using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
    public class AssetSimulate 
{
    public const string simulateKey = "ASSET_BUNDLE_SIMULATE";
    static int m_simulateModel = -1;
    public static bool USE_ASSET_BUNDLE
    {
        get
        {
            if (m_simulateModel == -1)
            {
                m_simulateModel = EditorPrefs.GetBool(simulateKey, true) ? 1 : 0;
            }
            return m_simulateModel == 1;
        }
    }

    public const string useWebServer = "SIMULATE_USE_WEB_SERVER";
    static int webServer = -1;

    /// <summary>
    /// 是否走下载更新模式
    /// </summary>
    public static bool USE_WEB_SERVER
    {
        get
        {
            if (!USE_ASSET_BUNDLE)
            {
                return false;
            }
            if (webServer == -1)
            {
                webServer = EditorPrefs.GetBool(useWebServer, true) ? 1 : 0;
            }
            return webServer == 1;
        }
    }

}
#endif