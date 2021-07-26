using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetRuntime
{
    public abstract class AsyncTemplate<T> where T : Loaded
    {

        static Dictionary<string, T> m_LoadedCahe = new Dictionary<string, T>(10);

        internal delegate AsyncRequest CreateRequest(string targetName, IRequestNote note);

        internal static void GetOrRequestLoaded(string targetName, IRequestNote note, CreateRequest onNeedCreateRequest)
        {
            T loaded = null;
            // 已经加载完成，则直接返回票据
            if (m_LoadedCahe.TryGetValue(targetName, out loaded))
            {
                note.RequestOver(loaded);
                return;
            }
            // 看当前是否已经存在了相同的加载申请
            AsyncRequest request = RequestHandleDriver.GetRequestHandler(targetName);

            // 没有申请则创建申请
            if (request == null)
            {
                request = onNeedCreateRequest(targetName, note);
                RequestHandleDriver.RegisterRequestHandler(targetName, request);
            }
            // 往异步申请的结束回调上绑定note的签名
            request.onRequestDone += note.RequestOver;
        }

    }
}