using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMonoModule : MonoBehaviour, FModuleInterface
{
    public F_MODULE_STATE STATE
    {
        get;
        private set;
    }

    public IEnumerator onPrepare()
    {
        STATE = F_MODULE_STATE.RUNNING;
        yield break;
    }

    public virtual void onInitialize()
    {
      
    }

    public virtual void onPause()
    {
       
    }


    public virtual void onRefresh()
    {
        
    }

    public virtual void onResume()
    {
      
    } 
}
