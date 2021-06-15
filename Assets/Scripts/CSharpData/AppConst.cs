using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AppConst：
/// </summary>
public class AppConst  {

#if UNITY_EDITOR_WIN
    public const string AssetDir  = "StreamingAssets/StandaloneWindows";
#elif UNITY_EDITOR_OS
#endif

    public const bool DebugMode = false;
    public const string AppName = "Test";
    public const string WebUrl = "http://192.168.103.88/Res/";      //测试更新地址
}
