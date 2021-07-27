using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetRuntime
{
    public class RequestHandleDriver
    {
        static Dictionary<string, AsyncRequest> m_requestHandlers = new Dictionary<string, AsyncRequest>(15);

        public static bool Runing => m_requestHandlers.Count != 0;

        public static void Drive()
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
                        handler.OnRequestOver();
                        needDeleteHandlers.Add(keys[i]);
                    }
                }

                while (needDeleteHandlers.Count > 0)
                {
                    var last = needDeleteHandlers.Count - 1;
                    var key = needDeleteHandlers[last];
                    var handler = m_requestHandlers[key];
                    m_requestHandlers.Remove(key);
                    needDeleteHandlers.RemoveAt(last);
                }

                ListPool<string>.Push(keys);
                ListPool<string>.Push(needDeleteHandlers);
            }
        }


        public static AsyncRequest GetRequestHandler(string handlerKey)
        {
            AsyncRequest handler = null;
            m_requestHandlers.TryGetValue(handlerKey, out handler);
            return handler;
        }


        public static void RegisterRequestHandler(string key, AsyncRequest handler)
        {
            m_requestHandlers.Add(key, handler);
        }


    }
}