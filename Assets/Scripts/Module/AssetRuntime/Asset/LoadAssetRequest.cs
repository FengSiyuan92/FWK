using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetRuntime
{
    public partial class Asset
    {
        /// <summary>
        /// 加载资源的异步申请
        /// </summary>
        class MLoadAssetRequest : AsyncRequest
        {
            AssetBundleRequest request;

            public Object Asset => isDone ? request.asset : null;

            public override void SetAsynOperation(AsyncOperation operation)
            {
                base.SetAsynOperation(operation);
                request = operation as AssetBundleRequest;
            }
            public override void Clear()
            {
                base.Clear();
                request = null;
            }

            public override void OnRequestOver()
            {
                base.OnRequestOver();
                m_RequestPool.Push(this);
            }
        }
    }
}