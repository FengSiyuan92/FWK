using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class AssetSimulate 
{

    const string simulateKey = "ASSET_BUNDLE_SIMULATE";
    static int m_simulateModel = -1;
    public static bool simulateModel
    {
        get
        {
            if (m_simulateModel == -1)
            {
                m_simulateModel = EditorPrefs.GetBool(simulateKey, true) ? 1 : 0;
            }
            return m_simulateModel == 1;
        }
        set
        {
            EditorPrefs.SetBool(simulateKey, value);
            m_simulateModel = value ? 1 : 0;
        }
    }

}
#endif