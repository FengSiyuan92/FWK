using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class GameDrive : MonoBehaviour {

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    private IEnumerator Start()
    {
        // 初始化旧AssetManager
        // 加载本地资源
        // 加载旧的登录场景=> 登录场景进行版本号比对,资源更新等逻辑

        var modules = GetComponentsInChildren<FModuleInterface>();
        System.Array.Sort(modules, (a, b) => a.RelativeOrder - b.RelativeOrder);

        for (int i = 0; i < modules.Length; i++)
        {
            var module = modules[i];
            yield return module.OnPrepare();
        }

        for (int i = 0; i < modules.Length; i++)
        {
            var module = modules[i];
            module.OnInitialize();
        }

        for (int i = 0; i < modules.Length; i++)
        {
            var module = modules[i];
            CallFuncManager.InstallRepeatFunc(CallFuncType.update, module.OnRefresh, false);
        }
    }

}
