using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetRuntime
{
    public abstract class AsyncTemplate<T> where T : Loaded
    {
        internal delegate AsyncRequest CreateRequest(string targetName, IRequestNote note);
        protected static Dictionary<string, T> m_loadedCache;


        internal static void GetOrRequestLoaded(string targetName, IRequestNote note,
            CreateRequest onNeedCreateRequest,
            System.Func<string, string> GetHandlerName)
        {
            T loaded = null;
            // 已经加载完成，则直接返回票据
            if (m_loadedCache.TryGetValue(targetName, out loaded))
            {
                note.OnRequestOver(loaded);
                return;
            }

            var handlerName = GetHandlerName(targetName);
            // 看当前是否已经存在了相同的加载申请
            AsyncRequest request = RequestHandleDriver.GetRequestHandler(handlerName);

            // 没有申请则创建申请
            if (request == null)
            {
                request = onNeedCreateRequest(targetName, note);
                RequestHandleDriver.RegisterRequestHandler(handlerName, request);
            }
            // 往异步申请的结束回调上绑定note的签名
            request.onRequestDone += note.OnRequestOver;
        }

    }
}