/****************************************************************
* FileName:     GUIStyleViewer.cs 
* Author:       fengsy01 
* Version:      1.0 
* UnityVersion：2019.4.28f1 
* Date:         2022-11-07 13:23 
* Description:    
* History: 
* ****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEditor;

namespace VesselTool
{
    public class GUIStyleViewer : EditorWindow
    {
        string searchInput = string.Empty; //搜索框输入内容

        public Vector2 scorllPos = Vector2.zero;

        [MenuItem("Tools/GUIStyle查看器")]
        static void ShowWindow()
        {
            GetWindow(typeof(GUIStyleViewer));
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal("HelpBox");
            GUILayout.Label("搜索:", GUILayout.Width(30));
            searchInput = EditorGUILayout.TextField(searchInput); //搜索框
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            scorllPos = EditorGUILayout.BeginScrollView(scorllPos); //滚动排布
            foreach (GUIStyle style in GUI.skin)
            {
                if (style.name.ToLower().Contains(searchInput.ToLower()))
                {
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal("HelpBox");
                    if (GUILayout.Button(style.name, style)) //以button形式显示
                    {
                        EditorGUIUtility.systemCopyBuffer = style.name; //点击时，将名字赋给系统剪切板
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(style.name);
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10);
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }
}