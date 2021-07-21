using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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