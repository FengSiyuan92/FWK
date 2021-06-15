using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using UnityEngine;
public class CoroutineAgent : MonoBehaviour
{
}



public class wCoroutineManager
{

    public static void Initialize()
    {
        //m_Main = GameDrive.instance;
    }

    private static MonoBehaviour m_Main;
    private static CoroutineAgent m_Private;
   
    public static Coroutine StartCoroutine(IEnumerator routine, bool destroyWhenLoadScene = true)
    {
        if (!destroyWhenLoadScene)
        {
            return m_Main.StartCoroutine(routine);
        }
        if (m_Private == null)
        {
            m_Private = new GameObject("CoroutineAgent").AddComponent<CoroutineAgent>();
        }
        return m_Private.StartCoroutine(routine);
    }
}

public class Yield
{
    static WaitForEndOfFrame endOfFrame;
    public static WaitForEndOfFrame waitForEndFrame
    {
        get
        {
            if (endOfFrame == null)
            {
                endOfFrame = new WaitForEndOfFrame();
            }
            return endOfFrame;
        }
    }
}