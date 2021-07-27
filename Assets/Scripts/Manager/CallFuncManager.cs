using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CallFuncType
{
    Update,
    FixedUpdate,
    LateUpdate
}

internal class CallFuncAgent : MonoBehaviour
{
    List<Action> updates = new List<Action>();
    List<Action> lateUpdates = new List<Action>();
    List<Action> fixUpdates = new List<Action>();

    void Update()
    {
        for (int i = 0; i < updates.Count; i++)
        {
            if (updates[i]!=null)
            {
                updates[i]();
            }
            else
            {
                updates.RemoveAt(i--);
            }
        }
    }
    void FixedUpdate()
    {
        for (int i = 0; i < fixUpdates.Count; i++)
        {
            if (fixUpdates[i] != null)
            {
                fixUpdates[i]();
            }
            else
            {
                fixUpdates.RemoveAt(i--);
            }
        }
    }

    void LateUpdate()
    {
        for (int i = 0; i < lateUpdates.Count; i++)
        {
            if (lateUpdates[i] != null)
            {
                lateUpdates[i]();
            }
            else
            {
                lateUpdates.RemoveAt(i--);
            }
        }
    }

    public void AddFixedUpdate(Action fixedUpdate)
    {
        fixUpdates.Add(fixedUpdate);
    }

    public void AddLateUpdate(Action lateUpdate)
    {
        lateUpdates.Add(lateUpdate);
    }

    public void AddUpdate(Action update)
    {
        updates.Add(update);
    }

    public bool RemoveFixedUpdate(Action fixedUpdate)
    {
        return fixUpdates.Remove(fixedUpdate);
    }

    public bool RemoveLateUpdate(Action lateUpdate)
    {
        return lateUpdates.Remove(lateUpdate);
    }

    public bool RemoveUpdate(Action update)
    {
        return updates.Remove(update);
    }
}


public class CallFuncManager
{
    private static CallFuncAgent m_Main;
    private static CallFuncAgent m_Private;
    public static void Initialize()
    {
        //m_Main = GameDrive.instance.gameObject.AddComponent<CallFuncAgent>();
    }

    public static void InstallRepeatFunc(CallFuncType type, Action func, bool destroyWhenLoadScene)
    {
        CallFuncAgent agent = destroyWhenLoadScene ? m_Private : m_Main;
        if (agent == null)
            agent = new GameObject("CallFuncAgent").AddComponent<CallFuncAgent>();
        switch (type)
        {
            case CallFuncType.Update:
                agent.AddUpdate(func);
                break;
            case CallFuncType.FixedUpdate:
                agent.AddFixedUpdate(func);
                break;
            case CallFuncType.LateUpdate :
                agent.AddLateUpdate(func);
                break;
            default:
                break;
        }
    }

    public static void UnloadRepeatFunc(CallFuncType type, Action func)
    {
        switch (type)
        {
            case CallFuncType.Update:
                if (!m_Main.RemoveUpdate(func) && m_Private != null)
                    m_Private.RemoveUpdate(func);
                break;
            case CallFuncType.FixedUpdate:
                if (!m_Main.RemoveFixedUpdate(func) && m_Private != null)
                    m_Private.RemoveFixedUpdate(func);
                break;
            case CallFuncType.LateUpdate:
                if (!m_Main.RemoveLateUpdate(func) && m_Private != null)
                    m_Private.RemoveLateUpdate(func);
                break;
            default:
                break;
        }
    }
}
