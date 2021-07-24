using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestHandleDrive : FMonoModule
{
    static Dictionary<string, RequestHandler> m_requestHandlers;

    public override void onRefresh()
    {
        if (m_requestHandlers.Count > 0)
        {
            var keys = ListPool<string>.Get();
            var needDeleteHandlers = ListPool<string>.Get();

            keys.AddRange(m_requestHandlers.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                var handler = m_requestHandlers[keys[i]];
                if (handler.isDone)
                {
                    handler.CreateLoaded();
                    handler.InvokeCallback();
                    needDeleteHandlers.Add(keys[i]);
                }
            }

            while (needDeleteHandlers.Count > 0)
            {
                var last = needDeleteHandlers.Count - 1;
                var key = needDeleteHandlers[last];
                var handler = m_requestHandlers[key];
                if (handler is MAssetBundleCreateRequest)
                {
                    MAssetBundleCreateRequestPool.Push(handler as MAssetBundleCreateRequest);
                }
                else
                {
                    MAssetRequestPool.Push(handler as MAssetRequest);
                }
                m_requestHandlers.Remove(key);
                needDeleteHandlers.RemoveAt(last);
            }
            ListPool<string>.Push(keys);
            ListPool<string>.Push(needDeleteHandlers);
        }
    }


    public static RequestHandler GetRequestHandler(string handlerKey)
    {
        RequestHandler handler = null;
        m_requestHandlers.TryGetValue(handlerKey, out handler);
        return handler;
    }


    public static void RegisterRequestHandler(string key, RequestHandler handler)
    {
        m_requestHandlers.Add(key, handler);
    }


}
