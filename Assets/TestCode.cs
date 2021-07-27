using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public void Test()
    {
        AssetManager.GetPrefab("Cube1", (prefab) =>
         {
            var go=  GameObject.Instantiate(prefab);
             DestroyImmediate(go);
             AssetManager.ReturnPrefab("Cube1");
        });

    }

}
