using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetRuntime
{
    public partial class Bundle
    {
        /// <summary>
        /// 加载bundle的异步操作申请
        /// </summary>
        class MLoadBundleRequest : AsyncRequest
        {
            AssetBundleCreateRequest request;

            public AssetBundle assetBundle => isDone ? request.assetBundle : null;

            public override void SetAsynOperation(AsyncOperation operation)
            {
                base.SetAsynOperation(operation);
                request = operation as AssetBundleCreateRequest;
            }

            public override void Clear()
            {
                base.Clear();
                request = null;
            }

            public override void OnRequestOver()
            {
                base.OnRequestOver();
                m_requestPool.Push(this);
            }
        }
    }
}
