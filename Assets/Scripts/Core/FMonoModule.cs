using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMonoModule : MonoBehaviour, FModuleInterface
{
    public F_MODULE_STATE STATE
    {
        get;
        set;
    }

    public virtual int RelativeOrder => 100;

    public virtual IEnumerator OnPrepare()
    {
        STATE = F_MODULE_STATE.RUNNING;
        yield break;
    }

    public virtual void OnInitialize()
    {
      
    }

    public virtual void OnPause()
    {
       
    }


    public virtual void OnRefresh()
    {
        
    }

    public virtual void OnResume()
    {
      
    } 

    public virtual void Restart()
    {

    }
}
