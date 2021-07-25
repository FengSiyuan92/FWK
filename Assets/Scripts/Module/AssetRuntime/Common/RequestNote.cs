using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetRuntime
{
    /// <summary>
    /// 异步加载所需的数据记录
    /// 将类型定义在对应manager的内部，方便访问私有对应manager中的私有函数
    /// </summary>

    interface IRequestNote : IReuseObject
    {
        void RequestOver(Loaded loaded);
        string TargetName { get; }
    }
}