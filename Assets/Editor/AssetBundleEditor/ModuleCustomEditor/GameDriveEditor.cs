using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GameDrive))]
public class GameDriverEditor : Editor
{

    struct TempInfo
    {
        public string name;
        public int order;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var driver = target as GameDrive;
        var childCount = driver.transform.childCount;
        List<string> moduleNames = new List<string>();
        for (int i = 0; i < childCount; i++)
        {
            var mod = driver.transform.GetChild(i).GetComponent<FMonoModule>();
            if (mod != null)
            {
                moduleNames.Add(mod.Name);
            }
        }

        if (driver.moduleName == null)
        {
            driver.moduleName = new string[0];
        }
        if (driver.orders == null)
        {
            driver.orders = new int[0];
        }

        var originalLength = driver.moduleName.Length;
        for (int i = originalLength - 1; i >= 0; i--)
        {
            var name = driver.moduleName[i];
            if (!moduleNames.Contains(name))
            {
                driver.moduleName[i] = null;
                originalLength--;
            }
        }

        List<string> empty = new List<string>();
        for (int i = 0; i < moduleNames.Count; i++)
        {
            if (System.Array.IndexOf(driver.moduleName, moduleNames[i]) == -1)
            {
                empty.Add(moduleNames[i]);
                originalLength++;
            }
        }

        if (originalLength != driver.moduleName.Length)
        {
            string[] newres = new string[originalLength];
            int[] newOrder = new int[originalLength];
            int o = 0;
            for (int i = 0; i < driver.moduleName.Length; i++)
            {
                var k = driver.moduleName[i];
                if (k != null)
                {
                    newres[o] = k;
                    newOrder[o++] = driver.orders[i];
                }
            }

            for (int i = 0; i < empty.Count; i++)
            {
                newres[o] = empty[i];
                newOrder[o++] = 0;
            }

            driver.moduleName = newres;
            driver.orders = newOrder;

            EditorSceneManager.MarkAllScenesDirty();
        }

        EditorGUILayout.TextArea("Module运行顺序调整:");
        var dirty = false;
        for (int i = 0; i < driver.moduleName.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.TextArea(driver.moduleName[i]);
            var old = driver.orders[i];
            var newOrder = EditorGUILayout.IntField(old);
            if (old != newOrder)
            {
                dirty = true;
                driver.orders[i] = newOrder;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (dirty)
        {
            EditorSceneManager.MarkAllScenesDirty();
        }


        if (EditorGUILayout.DropdownButton(new GUIContent("Sort"), FocusType.Keyboard))
        {
            List<TempInfo> temp = new List<TempInfo>();
            for (int i = 0; i < driver.moduleName.Length; i++)
            {
                var info = new TempInfo();
                info.name = driver.moduleName[i];
                info.order = driver.orders[i];
                temp.Add(info);

            }

            temp.Sort((a, b) => a.order - b.order);

            for (int i = 0; i < temp.Count; i++)
            {
                driver.moduleName[i] = temp[i].name;
                driver.orders[i] = temp[i].order;
            }

            EditorSceneManager.MarkAllScenesDirty();
            Repaint();
        }


    }

}