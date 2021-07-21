using UnityEngine;
using System.Collections.Generic;

public delegate void MessageListener(params object[] parameters);
public class Message
{
    public static void Initialize()
    {
        m_handlers = new Dictionary<MsgType, List<MessageListener>>(new MsgTypeComparer());
    }

    static Dictionary<MsgType, List<MessageListener>> m_handlers;

    class MsgTypeComparer : IEqualityComparer<MsgType>
    {
        public bool Equals(MsgType x, MsgType y)
        {
            return (int)x == (int)y;
        }

        public int GetHashCode(MsgType obj)
        {
            return (int)obj;
        }
    }

    public static void AddListener(MsgType msgType, MessageListener listener)
    {
        List<MessageListener> list = null;
        if (!m_handlers.TryGetValue(msgType, out list))
        {
            list = ListPool<MessageListener>.Get();
            m_handlers.Add(msgType, list);
        }
        list.Add(listener);
    }


    public static void RemoveListener(MsgType msgType, MessageListener listener)
    {
        List<MessageListener> list = null;
        if (m_handlers.TryGetValue(msgType, out list))
        {
            list.Remove(listener);
            if (list.Count == 0)
            {
                m_handlers.Remove(msgType);
                ListPool<MessageListener>.Push(list);
            }
        }
    }

    public static void Send(MsgType msgType,params object[] parameters)
    {
        List<MessageListener> list = null;
        if (m_handlers.TryGetValue(msgType, out list))
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i](parameters);
            }
        }
    }


}
