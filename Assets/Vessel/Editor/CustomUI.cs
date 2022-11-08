/****************************************************************
* FileName:     FloatWindow.cs 
* Author:       fengsy01 
* Version:      1.0 
* UnityVersion：2019.4.28f1 
* Date:         2022-11-03 19:30 
* Description:    
* History: 
* ****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System;


namespace VesselTool
{
    public class CustomUI
    {

        public static void ShowDialog(Vector2 pos, string content, System.Action confirm, System.Action cancel)
        {

            var targetPos = new Rect(pos.x, pos.y, 0, 0); ;
            float width = 200;
            float height = 100;

            Action<Rect> drawAction = (r) =>
            {

                GUILayout.Label(content, EditorStyles.wordWrappedLabel);


                if (confirm != null)
                {
                    if (GUI.Button(new Rect(r.width * 0.15f, r.height * 0.65f, r.width * 0.3f, r.height * 0.3f), "确定"))
                    {
                        EditorWindow.GetWindow<PopupWindow>().Close();
                        confirm();
                    }
                }

                if (cancel != null)
                {
                    if (GUI.Button(new Rect(r.width * 0.55f, r.height * 0.65f, r.width * 0.3f, r.height * 0.3f), "算了"))
                    {
                        EditorWindow.GetWindow<PopupWindow>().Close();
                        cancel();
                    }
                }

            };

            PopupWindow.Show(targetPos, new DialogContent(new Vector2(width, height), drawAction));
        }

        static Material drawMat;

        public static void DrawTexture(Rect r, Texture t, Color color, string tooptip = null)
        {
            drawMat = drawMat ?? new Material(Shader.Find("Sprites/Default"));
            drawMat.color = color;
            EditorGUI.DrawPreviewTexture(r, t, drawMat);
            if (!string.IsNullOrEmpty(tooptip))
            {
                var content = new GUIContent() { tooltip = tooptip };
                DrawTooptipArea(r, content);
            }
        }

        public static void DrawTooptipArea(Rect rect, GUIContent content)
        {
            var bgColor = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0, 0, 0, 0);
            GUI.Box(rect, content);
            GUI.backgroundColor = bgColor;
        }


        public static void ShowInputField(Vector2 pos, string title, string defaultValue, System.Action<string> confirm, System.Action cancel)
        {

            var targetPos = new Rect(pos.x, pos.y, 0, 0); ;
            float width = 200;
            float height = 100;
            string changedValue = defaultValue;
            Action<Rect> drawAction = (r) =>
            {

                GUILayout.Label(title, EditorStyles.wordWrappedLabel);


                changedValue = EditorGUILayout.TextField(changedValue);

                if (confirm != null)
                {
                    if (GUI.Button(new Rect(r.width * 0.15f, r.height * 0.65f, r.width * 0.3f, r.height * 0.3f), "确定"))
                    {
                        var window = EditorWindow.GetWindow<PopupWindow>();
                        window.Close();
                        UnityEngine.Object.DestroyImmediate(window);
                        confirm(changedValue);
                    }
                }

                if (cancel != null)
                {
                    if (GUI.Button(new Rect(r.width * 0.55f, r.height * 0.65f, r.width * 0.3f, r.height * 0.3f), "算了"))
                    {
                        EditorWindow.GetWindow<PopupWindow>().Close();
                        cancel();
                    }
                }

            };

            PopupWindow.Show(targetPos, new DialogContent(new Vector2(width, height), drawAction));
        }


        public static void ShowFloatWindow(Vector2 pos, string tip, Action<float, float> drawAc, Action confirm, Action cancel)
        {
            var targetPos = new Rect(pos.x, pos.y, 0, 0); ;
            float width = 200;
            float height = 200;

            Vector2 currentPercent = Vector2.zero;
            Action<Rect> drawAction = (r) =>
            {

                float startHeight = 0;
                if (!string.IsNullOrEmpty(tip))
                {
                    GUILayout.Label(tip, EditorStyles.wordWrappedLabel);
                }
                if (drawAc != null)
                {
                    currentPercent = GUILayout.BeginScrollView(currentPercent, GUILayout.MaxHeight(130), GUILayout.MinHeight(0));
                    drawAc(180, -1);
                    GUILayout.EndScrollView();
                }

                if (confirm != null)
                {
                    if (GUI.Button(new Rect(r.width * 0.15f, r.height * 0.85f, r.width * 0.3f, r.height * 0.1f), "确定"))
                    {
                        EditorWindow.GetWindow<PopupWindow>().Close();
                        confirm();
                    }
                }

                if (cancel != null)
                {
                    if (GUI.Button(new Rect(r.width * 0.55f, r.height * 0.85f, r.width * 0.3f, r.height * 0.1f), "算了"))
                    {
                        EditorWindow.GetWindow<PopupWindow>().Close();
                        cancel();
                    }
                }

            };

            PopupWindow.Show(targetPos, new DialogContent(new Vector2(width, height), drawAction));
        }


        class DialogContent : PopupWindowContent
        {

            Action<Rect> DrawAction;

            Vector2 windowSize;

            internal DialogContent(Vector2 size, Action<Rect> drawAction)
            {
                DrawAction = drawAction;

                windowSize = size;
            }

            public override Vector2 GetWindowSize()
            {
                return windowSize;
            }


            public override void OnGUI(Rect rect)
            {
                DrawAction(rect);
            }
        }

    }
}