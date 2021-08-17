using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ModuleOrder :ScriptableObject
{
    [SerializeField]
    string[] m_modules;
    [SerializeField]
    int[] m_relativeOrders;

    Dictionary<string, int> orderMap;

    public int RelativeOrder(string moduleName)
    {
        if (orderMap == null)
        {
            orderMap = new Dictionary<string, int>(m_modules.Length);
            for (int i = 0; i < m_modules.Length; i++)
            {
                orderMap.Add(m_modules[i], m_relativeOrders[i]);
            }
        }

        return orderMap[moduleName];
    }
}

#if UNITY_EDITOR

public class ModuleOrderEditor :Editor
{
    [MenuItem("GameObject/Create")]
    public static void CreateModuleInstance()
    {

    }
}



#endif