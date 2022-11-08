/****************************************************************
* FileName:     CustomEvent.cs 
* Author:       fengsy01 
* Version:      1.0 
* UnityVersion：2019.4.28f1 
* Date:         2022-11-03 18:25 
* Description:    
* History: 
* ****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VesselTool
{
    public class CustomEvent
    {

        public static bool IsMouseDown(Rect r, out float x, out float y)
        {
            x = 0;
            y = 0;
            var currentEvent = Event.current;
            if (currentEvent.type == EventType.MouseDown)
            {
                var pos = currentEvent.mousePosition;
                x = pos.x;
                y = pos.y;
                var inrect = r.Contains(pos);
                if (inrect)
                {
                    currentEvent.Use();
                }
                return inrect;
            }

            return false;
        }
        public static bool IsMouseDown(Rect r)
        {
            return IsMouseDown(r, out float x, out float y);
        }

        public static bool IsDragIn(Rect r, System.Func<Object[], bool> acceptCondition, out float x, out float y, out Object obj)
        {
            x = 0;
            y = 0;
            obj = null;
            var currentEvent = Event.current;

            if (currentEvent.type == EventType.DragPerform || currentEvent.type == EventType.DragUpdated)
            {
                // Debug.Log($"{currentEvent.type.ToString()} ");
                var pos = currentEvent.mousePosition;
                x = pos.x;
                y = pos.y;
                var inrect = r.Contains(pos);
                if (!inrect) return false;

                var refs = DragAndDrop.objectReferences;
                if (!acceptCondition(refs))
                {
                    return false;
                }

                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                // Debug.Log($"{currentEvent.type.ToString()} count = {refs.Length}");

                if (currentEvent.type == EventType.DragPerform && refs.Length > 0)
                {
                    DragAndDrop.AcceptDrag();
                    obj = refs[0];
                    currentEvent.Use();
                    return true;
                }

                currentEvent.Use();
                return false;
            }

            return false;

        }


        public static bool IsBackBtn()
        {
            var currentEvent = Event.current;
            var btn = currentEvent.button;
            Debug.Log(btn);
            Debug.Log(currentEvent);
            return true;
        }


        public static bool IsForwardBtn()
        {
            var currentEvent = Event.current;
            var btn = currentEvent.button;
            return true;
        }

    }
}
