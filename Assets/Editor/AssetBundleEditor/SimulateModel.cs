using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SimulateModel 
{

    const string item1 = "AssetBundle/使用AssetBundle加载资源";
    const string item2 = "AssetBundle/是否使用服务器资源(更新模式)";

    [MenuItem(item1, false, 100)]
    static void SerUseBundle()
    {
        var old = EditorPrefs.GetBool(AssetSimulate.simulateKey);
        EditorPrefs.SetBool(AssetSimulate.simulateKey, !old);
    }


    [MenuItem(item1, true, 100)]
    static bool SerUseBundleCheck()
    {
        Menu.SetChecked(
            item1,
            EditorPrefs.GetBool(AssetSimulate.simulateKey)
            );
        return true;

    }

    [MenuItem(item2, false, 101)]
    static void SetUseWebServer()
    {
        var old = EditorPrefs.GetBool(AssetSimulate.useWebServer);
        EditorPrefs.SetBool(AssetSimulate.useWebServer, !old);
    }


    [MenuItem(item2, true, 101)]
    static bool SetUseWebServerCheck()
    {
        bool useBundle = EditorPrefs.GetBool(AssetSimulate.simulateKey);
        Menu.SetChecked(
            item2,
            EditorPrefs.GetBool(AssetSimulate.useWebServer) && useBundle
            );

        return useBundle;
    }
}
