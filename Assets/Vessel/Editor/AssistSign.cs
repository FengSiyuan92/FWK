/****************************************************************
* FileName:     AssistSign.cs 
* Author:       fengsy01 
* Version:      1.0 
* UnityVersion：2019.4.28f1 
* Date:         2022-11-07 14:22 
* Description:    
* History: 
* ****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Reflection;
using Extension;

namespace VesselTool
{

    public class AssistSign
    {
        static bool opened = false;
        static Color32 iconColor = new Color32(136, 243, 166, 255);
        static Texture VesselTex;
        static Texture SignTex;
        static FieldInfo field;
        [InitializeOnLoadMethod]
        static void RegHierarchyGUI()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnDrawHierarchy;
            VesselTex = Resources.Load<Texture>("fui_root");
            SignTex = Resources.Load<Texture>("fui_mark");
            var type = typeof(Vessel);
            field = type.GetField("values", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            opened = EditorPrefs.GetBool("VESSEL_HIERARCHY_OPENED", true);
        }

        public static bool Opened
        {
            get
            {
                return opened;
            }
            set
            {
                if (value == opened)
                {
                    return;
                }
                opened = value;
                EditorPrefs.SetBool("VESSEL_HIERARCHY_OPENED", opened);
                EditorApplication.RepaintHierarchyWindow();
            }
        }

        static bool Include(Vessel v, GameObject go)
        {
            var values = field.GetValue(v) as UnityEngine.Object[];
            if (values == null)
            {
                return false;
            }

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] is Component c && c!=null && c.gameObject == go)
                {
                    return true;
                }
            }
            return false;
        }

        static void OnDrawHierarchy(int instanceId, Rect r)
        {
            if (!opened) return;
            var go = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
            if (go == null) return;

            var vessel = go.GetComponentInParent<Vessel>();
            if (vessel == null) return;


            if (vessel.gameObject == go)
            {
                CustomUI.DrawTexture(new Rect(30, r.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight), VesselTex, iconColor);
            }
            else if (Include(vessel, go))
            {
                CustomUI.DrawTexture(new Rect(30, r.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight), SignTex, iconColor);
            }
        }
    }

}