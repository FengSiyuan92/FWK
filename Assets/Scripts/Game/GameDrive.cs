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
        var modules = GetComponentsInChildren<FModuleInterface>();
        for (int i = 0; i < modules.Length; i++)
        {
            var module = modules[i];
            yield return module.onPrepare();
        }

        for (int i = 0; i < modules.Length; i++)
        {
            var module = modules[i];
            module.onInitialize();
            CallFuncManager.InstallRepeatFunc(CallFuncType.update, module.onRefresh, false);
        }
    }
}
