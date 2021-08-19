using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(-10000)]
public class GameDrive : MonoBehaviour {

    [HideInInspector][SerializeField]
    public string[] moduleName;

    [HideInInspector][SerializeField]
    public int[] orders;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 驱动器是否可执行(只有在各个模块都成功初始化后,改字段才为true)
    /// </summary>
    public static bool Executable { get; private set; } = false;
    FModuleInterface[] modules;

    private IEnumerator Start()
    {
        // 初始化旧AssetManager
        // 加载本地资源
        // 加载旧的登录场景=> 登录场景进行版本号比对,资源更新等逻辑
        modules = GetComponentsInChildren<FModuleInterface>();
        SortModule(modules);

#if UNITY_EDITOR
        // log模块名称和顺序
        var order = "";
        for (int i = 0; i < modules.Length; i++)
        {
            order += modules[i].Name + ",";
        }
        Debug.Log("按顺序初始化模块:" + order);
#endif

        // 准备模块运行
        for (int i = 0; i < modules.Length; i++)
        {
            var module = modules[i];
          
            yield return module.OnPrepare();
            Debug.Log(module.Name + "准备完成");
        }

        // 模块初始化
        for (int i = 0; i < modules.Length; i++)
        {
            var module = modules[i];
            module.OnInitialize();
            Debug.Log(module.Name + "初始化完成");
        }
        Executable = true;
    }

    private void Update()
    {
        if (!Executable) return;
   
        for (int i = 0; i < modules.Length; i++)
        {
            var module = modules[i];
            module.OnRefresh();
            //Debug.Log(module.Name + "刷新完成");
        }
    }

    public void Pause()
    {
        if (!Executable) { throw new System.Exception("驱动器没有初始化成功,无法调用暂停"); } 
        for (int i = 0; i < modules.Length; i++)
        {
            var module = modules[i];
            module.STATE = F_MODULE_STATE.PAUSE;
            module.OnPause();
            //Debug.Log(module.Name + "停止运行");
        }
        this.enabled = false;
    }

    public void Resume()
    {
        if (!Executable) { throw new System.Exception("驱动器没有初始化成功,无法调用恢复"); }

        for (int i = 0; i < modules.Length; i++)
        {
            var module = modules[i];
            module.STATE = F_MODULE_STATE.RUNNING;
            module.OnResume();
            //Debug.Log(module.Name + "恢复运行");
        }
        this.enabled = true;
    }


    void SortModule(FModuleInterface[] modules)
    {
        Dictionary<string, int> relativeOrder = new Dictionary<string, int>(moduleName.Length);
        for (int i = 0; i < moduleName.Length; i++)
        {
            relativeOrder.Add(moduleName[i], orders[i]);
        }

        System.Func<string, int> getOrder = (moduleName) =>
        {
            int order = 0;
            relativeOrder.TryGetValue(moduleName, out order);
            return order;
        };

        System.Array.Sort(modules, (a, b) => getOrder(a.Name) - getOrder(b.Name));
    }
}