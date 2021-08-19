using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeCall
{
    public static void Call<T>(System.Action<T> action, T param)
    {
        if (action != null)
        {
            try
            {
                action(param);
            }
            catch(System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
    }

    public static void Call(System.Action action)
    {
        if (action != null)
        {
            try
            {
                action();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
    }
}
