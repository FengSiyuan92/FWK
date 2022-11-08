/****************************************************************
* FileName:     Vessel.cs 
* Author:       fengsy01 
* Version:      1.0 
* UnityVersion：2019.4.28f1 
* Date:         2022-11-03 17:17 
* Description:    
* History: 
* ****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extension
{
    public class Vessel : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        protected string[] names;

        [SerializeField]
        [HideInInspector]
        protected Object[] values;

    }
}