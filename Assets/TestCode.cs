using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FAction;
public class TestCode : MonoBehaviour
{
    public void Test()
    {
        //AssetManager.GetPrefab("Cube1", (prefab) =>
        // {
        //    var go=  GameObject.Instantiate(prefab);
        //     DestroyImmediate(go);
        //     AssetManager.ReturnPrefab("Cube1");
        //});

    }


    FAction.FAction action;
     void Update()
    {
        if (!GameDrive.Executable)
        {
            return;
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            XLuaManager.DoString("(require 'Core.main').Test()");

        }



    }
}
