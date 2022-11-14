/****************************************************************
* FileName:     GameObjecEditorExtension.cs 
* Author:       fengsy01 
* Version:      1.0 
* UnityVersion：2019.4.28f1 
* Date:         2022-11-03 16:23 
* Description:    
* History: 
* ****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using Extension;

namespace VesselTool
{
    enum VesselNodeType
    {
        NoRoot,
        Root,
        SubRoot,
    }

    [CustomEditor(typeof(GameObject))]
    public sealed class GameObjecEditorExtension : DecoratorEditor
    {
        static Texture linkTex;
        static Texture vesselTex;
        static Texture infoTex;

        static Texture treeTex;
        static Texture collectedTex;
        static Texture nestTex;

        static Texture goupTex;
        static Texture godownTex;
        static Texture backTex;
        static Texture forwardTex;

        System.Type[] VesselType;

        VesselNodeType nodeType = VesselNodeType.NoRoot;
        Vessel selfVessel;
        Vessel parentVessel;

        Editor __selfVesselEditor;
        Editor __parentVesselEditor;

        float startX;
        float endX;
        static string LAST_ADD_KEY;
        static int defaultSelectIndex;
        static float singleLineHeight;

        Editor selfVesselEditor
        {
            get
            {
                if (__selfVesselEditor != null && (__selfVesselEditor.target != selfVessel || selfVessel == null))
                {
                    DestroyImmediate(__selfVesselEditor);
                    __selfVesselEditor = null;
                }

                if (selfVessel != null && __selfVesselEditor == null)
                {
                    __selfVesselEditor = Editor.CreateEditor(selfVessel);
                }

                return __selfVesselEditor;
            }
        }
        Editor parentVesselEditor
        {
            get
            {
                if (__parentVesselEditor != null && (__parentVesselEditor.target != parentVessel || parentVessel == null))
                {
                    DestroyImmediate(__parentVesselEditor);
                    __parentVesselEditor = null;
                }

                if (parentVessel != null && __parentVesselEditor == null)
                {
                    __parentVesselEditor = Editor.CreateEditor(parentVessel);
                }

                return __parentVesselEditor;
            }
        }

        private void OnEnable()
        {
            var t = target as GameObject;

            selfVessel = null;

            vesselTex = vesselTex ?? Resources.Load<Texture>("fui_root");
            linkTex = linkTex ?? Resources.Load<Texture>("fui_link");
            infoTex = infoTex ?? Resources.Load<Texture>("fui_list");
            treeTex = treeTex ?? Resources.Load<Texture>("fui_tree");
            nestTex = nestTex ?? Resources.Load<Texture>("fui_nest");

            collectedTex = collectedTex ?? Resources.Load<Texture>("fui_collected");

            goupTex = goupTex ?? Resources.Load<Texture>("fui_goup");
            godownTex = godownTex ?? Resources.Load<Texture>("fui_godown");
            backTex = backTex ?? Resources.Load<Texture>("fui_btn_back");
            forwardTex = forwardTex ?? Resources.Load<Texture>("fui_btn_forward");

            if (VesselType == null)
            {
                var vesselAssembly = Assembly.Load("Assembly-CSharp");
                var targetType = typeof(Vessel);
                VesselType = vesselAssembly.GetTypes().Where(p => p.IsSubclassOf(targetType)).ToArray();
                var lastAdd = EditorPrefs.GetString(LAST_ADD_KEY, "");
                defaultSelectIndex = 0;
                if (!string.IsNullOrEmpty(lastAdd))
                {
                    for (int i = 0; i < VesselType.Length; i++)
                    {
                        if (VesselType[i].Name == lastAdd)
                        {
                            defaultSelectIndex = i;
                            break;
                        }
                    }
                }
            }
        }


        protected override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
            var drawGo = target as GameObject;
            UpdateVessel();
            singleLineHeight = EditorGUIUtility.singleLineHeight;
            var r = EditorGUILayout.GetControlRect(false, 2 * singleLineHeight);
            startX = r.x;
            endX = r.x + r.width;
            DrawLinkState(drawGo, r);
            DrawVesselState(drawGo, r);

            var flag = DrawVesselListOpenIcon(drawGo, r);
            DrawVesselDetailInfo(flag);

            DrawNestIcon(drawGo, r);

            DrawHierarchySwitchIcon(drawGo, r);

            flag = DrawCollectedInfoIcon(drawGo, r);
            DrawCollectedInfoList(flag);


            DrawOperatorBtn(drawGo, r);
            EditorGUILayout.Space();
        }


        #region draw link state
        const string LinkedOpenDesc = @"链接状态标志(开启)，通过拖拽自身组件至浮窗区可快速绑定节点至容器";
        const string LinkedCloseDesc = @"链接状态标志(关闭中),当前自身或父节点中无容器对象，无法进行绑定操作";
        const string drawTitleDesc = "请为引用的变量命名";

        /// <summary>
        /// 绘制链接状态
        /// </summary>
        /// <param name="go"></param>
        /// <param name="r"></param>
        void DrawLinkState(GameObject go, Rect r)
        {

            startX += 5;
            float clickStartx = startX;

            var signRect = new Rect(startX, r.y, singleLineHeight, singleLineHeight);

            var linked = selfVessel != null || parentVessel != null;
            var color = linked ? Color.green : Color.white;
            CustomUI.DrawTexture(signRect, linkTex, color, linked ? LinkedOpenDesc : LinkedCloseDesc);
            startX += EditorGUIUtility.singleLineHeight;

            if (!linked)
            {
                startX += 85;
                return;
            };

            var vesselObject = selfVessel ?? parentVessel;
            startX += 5;
            var vesselRect = new Rect(startX, r.y, 80, singleLineHeight);
            GUI.Label(vesselRect, vesselObject.gameObject.name);
            startX += 80;

            var skipRect = new Rect(clickStartx, r.y, startX - clickStartx, singleLineHeight);

            if (CustomEvent.IsMouseDown(skipRect, out float x, out float y))
            {
                PingVessel(vesselObject);
            }

            // 拖拽监听
            if (CustomEvent.IsDragIn(r, AcceptDragCondition, out float dragx, out float dragy, out Object selected))
            {
                System.Action<string> onConfirmAddNode = (name) =>
                {
                    if (selfVessel != null)
                    {
                        AddObjectForSelf(name, selected);
                    }
                    else
                    {
                        AddObjectForParent(name, selected);
                    }
                };

                CustomUI.ShowInputField(new Vector2(dragx, dragy), drawTitleDesc, GetDefaultNodeName(selected), onConfirmAddNode, EmptyAction);
            }
        }
        #endregion



        #region draw vessel root state

        const string RootVesselDesc = @"当前游戏对象存在'根容器'
点击将删除该游戏对象的容器";
        const string SubrootVesselDesc = @"当前游戏对象存在'子容器'（父节点中存在其他容器）
点击将删除该游戏对象的容器";
        const string NorootVesselDesc = @"当前游戏对象中不存在任何容器
点击将会为该游戏对象创建新容器";

        static string[] Type2Desc = new string[] { NorootVesselDesc, RootVesselDesc, SubrootVesselDesc };
        static Color32[] Type2Color = new Color32[] { Color.white, Color.green, Color.yellow };
        const string DeleteNoEmptyVesselTip = @"容器中存在被序列化的对象，删除容器将会失去这些节点的序列化信息，确定要删除吗？";

        /// <summary>
        /// 容器状态绘制
        /// </summary>
        /// <param name="go"></param>
        /// <param name="r"></param>
        void DrawVesselState(GameObject go, Rect r)
        {
            startX += 15;
            var iconRect = new Rect(startX, r.y, singleLineHeight, singleLineHeight);
            startX += singleLineHeight;
            CustomUI.DrawTexture(iconRect, vesselTex, Type2Color[(int)nodeType], Type2Desc[(int)nodeType]);

            if (CustomEvent.IsMouseDown(iconRect, out float clickX, out float clickY))
            {
                switch (nodeType)
                {
                    case VesselNodeType.NoRoot:
                        AddVessel(clickX, clickY, parentVessel != null);
                        break;
                    case VesselNodeType.Root:

                    case VesselNodeType.SubRoot:
                        if (IsEmptyVessel(selfVesselEditor))
                        {
                            RemoveVessel();
                        }
                        else
                        {
                            System.Action onConfirm = () =>
                            {
                                RemoveVessel();
                            };

                            System.Action<float, float> dca = (width, height) =>
                            {
                                DrawSerializedSimple(selfVesselEditor, null, width, height);
                            };

                            CustomUI.ShowFloatWindow(new Vector2(clickX, clickY), DeleteNoEmptyVesselTip, dca, onConfirm, EmptyAction);
                        }
                        break;
                    default:
                        break;
                }

            }
        }
        #endregion


        #region draw vessel detail info icon
        const string VesselIconDesc1 = @"当节点存在容器时，将展示此按钮。
通过点击按钮可以，'展开/关闭'详情界面，以查看容器中包含的引用信息";

        bool DrawVesselListOpenIcon(GameObject go, Rect r)
        {
            if (selfVessel == null) return false;

            startX += 10;
            var k = "_VPS_" + go.name;
            var flag = EditorPrefs.GetBool(k, false);

            bool dirty = false;
            // 绘制散列详情图标
            var iconRect = new Rect(startX, r.y, singleLineHeight, singleLineHeight);
            startX += singleLineHeight;

            var color1 = flag ? Color.green : Color.white;
            CustomUI.DrawTexture(iconRect, infoTex, color1, VesselIconDesc1);
            if (CustomEvent.IsMouseDown(iconRect))
            {
                flag = !flag;
                dirty = true;
                EditorPrefs.SetBool(k, flag);

            }
            return flag;
        }

        #endregion

        const string HierarchySwitchDesc = @"层级辅助提示功能，开启后Hierarchy视图将标记容器和被收纳节点";

        void DrawHierarchySwitchIcon(GameObject go, Rect r)
        {
            // 绘制树形详情图标
            endX -= singleLineHeight + 10;
            var iconRect = new Rect(endX, r.y, singleLineHeight, singleLineHeight);
            var flag = AssistSign.Opened;
            var color = flag ? Color.green : Color.white;
            CustomUI.DrawTexture(iconRect, treeTex, color, HierarchySwitchDesc);

            if (CustomEvent.IsMouseDown(iconRect))
            {
                flag = !flag;
                AssistSign.Opened = flag;
            }
        }

        #region draw skip operator btn
        const string GoUpDesc = @"递归父节点，并跳转至最近的一个容器上";
        const string GoDownDesc = @"广度遍历当前子节点，并跳转至最近的一个容器上";
        const string GoBackDesc = @"向前遍历与当前平级的节点,并跳转至最近的一个容器上";
        const string GoForwardDesc = @"向后遍历与当前平级的节点，并跳转至最近的一个容器上";

        // 绘制操作按钮
        void DrawOperatorBtn(GameObject go, Rect r)
        {
            var fix = r.x;
            var y = r.y + singleLineHeight;

            int len = 4;
            Rect[] opRect = new Rect[len];
            for (int i = 0; i < len; i++)
            {
                fix += 5;
                opRect[i] = new Rect(fix, y, singleLineHeight, singleLineHeight);
                fix += singleLineHeight;
            }

            // 0.向上
            var rect = opRect[0];
            var valid = parentVessel != null;
            var color = valid ? Color.white : Color.gray;
            CustomUI.DrawTexture(rect, goupTex, color, GoUpDesc);
            if (valid && CustomEvent.IsMouseDown(rect))
            {
                PingVessel(parentVessel);
            }

            // 1.平级向上
            rect = opRect[1];
            Vessel target = FindBackVessel(go.transform);
            valid = target != null;
            color = valid ? Color.white : Color.gray;
            CustomUI.DrawTexture(rect, backTex, color, GoBackDesc);
            if (valid && CustomEvent.IsMouseDown(rect))
            {
                PingVessel(target);
            }

            // 2. 跳转下级
            rect = opRect[2];
            target = FindChildVessel(go.transform);
            valid = target != null;
            color = valid ? Color.white : Color.gray;
            CustomUI.DrawTexture(rect, godownTex, color, GoDownDesc);
            if (valid && CustomEvent.IsMouseDown(rect))
            {
                PingVessel(target);
            }

            // 3.平级向下
            rect = opRect[3];
            target = FindForwardVessel(go.transform);
            valid = target != null;
            color = valid ? Color.white : Color.gray;
            CustomUI.DrawTexture(rect, forwardTex, color, GoForwardDesc);
            if (valid && CustomEvent.IsMouseDown(rect))
            {
                PingVessel(target);
            }
        }
        #endregion



        #region draw vess detail info
        const string titleTip = "该容器包含如下引用:";
        const string deletaTip = "X";
        void DrawVesselDetailInfo(bool flag)
        {
            if (!flag) return;
            EditorGUILayout.BeginVertical("frameBox");

            var obj = selfVesselEditor.serializedObject;
            SerializedProperty pnames = obj.FindProperty("names");
            SerializedProperty pvalues = obj.FindProperty("values");
            var count = pnames.arraySize;

            EditorGUILayout.LabelField(titleTip);

            bool dirty = false;
            int deleteIndex = -1;
            for (int i = 0; i < count; i++)
            {
                EditorGUILayout.BeginHorizontal("badge");
                var n = pnames.GetArrayElementAtIndex(i);
                var name = n.stringValue;
                var v = pvalues.GetArrayElementAtIndex(i);
                var value = v.objectReferenceValue;
                var newName = EditorGUILayout.DelayedTextField(name);
                if (newName != name)
                {
                    n.stringValue = newName;
                    dirty = true;
                }
                GUI.enabled = false;
                EditorGUILayout.ObjectField(value, value== null? typeof(Object): value.GetType());
                GUI.enabled = true;

                if (GUILayout.Button(deletaTip, GUILayout.MaxWidth(20)))
                {
                    deleteIndex = i;
                }

                EditorGUILayout.EndHorizontal();
            }
            if (deleteIndex != -1)
            {
                RemoveByIndex(selfVesselEditor, deleteIndex);
            }

            if (dirty)
            {
                obj.ApplyModifiedProperties();
                EditorUtility.SetDirty(selfVessel);
            }

            EditorGUILayout.EndVertical();
        }

        #endregion

        const string NestDesc = @"当节点是子容器时，展示该按钮
颜色表示当前容器与父节点容器是否'已嵌套'(父节点容器中引用了该节点容器)
如未嵌套，点击该按钮可将容器加入到父节点中，建立嵌套
如已嵌套，点击该按钮将解除嵌套关系";

        const string nestTip = "为引用变量命名";
        /// <summary>
        /// 绘制嵌套按钮
        /// </summary>
        void DrawNestIcon(GameObject go, Rect r)
        {
            if (selfVessel == null || parentVessel == null) return;

            startX += 10;
            var iconRect = new Rect(startX, r.y, singleLineHeight, singleLineHeight);
            var index = IndexOf(parentVesselEditor, selfVessel);
            var color = index == -1 ? Color.white : Color.green;

            CustomUI.DrawTexture(iconRect, nestTex, color, NestDesc);
            if (CustomEvent.IsMouseDown(iconRect, out float cx, out float cy))
            {
                if (index == -1)
                {
                    System.Action<string> onConfirmAddNode = (name) =>
                    {

                        AddObjectForParent(name, selfVessel);

                    };

                    CustomUI.ShowInputField(new Vector2(cx, cy), nestTip, GetDefaultNodeName(selfVessel), onConfirmAddNode, EmptyAction);
                }
                else
                {
                    RemoveObjectForParent(selfVessel);
                }
            }

        }


        #region  draw Collected info
        const string collectedKey = "VESSEL_COLLECTED_DETAIL";
        const string CollectedDesc = @"被引用信息
可以通过点击按钮，切换被引用信息的展示情况";
        const string collectTipSelf = "被自身容器引用信息:";
        const string collectTipParent = "被父节点容器引用信息:";

        bool DrawCollectedInfoIcon(GameObject go, Rect r)
        {
            endX -= singleLineHeight + 10;
            var iconRect = new Rect(endX, r.y, singleLineHeight, singleLineHeight);
            var flag = EditorPrefs.GetBool(collectedKey, false);

            var color = flag ? Color.green : Color.white;
            CustomUI.DrawTexture(iconRect, collectedTex, color, CollectedDesc);
            if (CustomEvent.IsMouseDown(iconRect))
            {
                flag = !flag;
                EditorPrefs.SetBool(collectedKey, flag);
            }
            return flag;
        }


        void DrawCollectedInfoList(bool flag)
        {
            if (!flag) return;
            var go = target as GameObject;
            // 被自身引用的节点
            if (selfVessel != null)
            {
                DrawCollectedListByVessel(selfVessel, selfVesselEditor, go, collectTipSelf);
            }

            // 父节点存在容器
            if (parentVessel != null)
            {
                DrawCollectedListByVessel(parentVessel, parentVesselEditor, go, collectTipParent);
            }
        }

        static List<int> needDrawIndexCache = new List<int>();
        static void DrawCollectedListByVessel(Vessel vessel, Editor vesselEditor, GameObject go, string tip)
        {
            var obj = vesselEditor.serializedObject;
            SerializedProperty pnames = obj.FindProperty("names");
            SerializedProperty pvalues = obj.FindProperty("values");
            var count = pnames.arraySize;
            needDrawIndexCache.Clear();
            for (int i = 0; i < count; i++)
            {
                var v = pvalues.GetArrayElementAtIndex(i);
                var value = v.objectReferenceValue;
                if (value is Component c && c.gameObject == go)
                {
                    needDrawIndexCache.Add(i);
                }
            }

            if (needDrawIndexCache.Count > 0)
            {
                int deleteIndex = -1;
                bool dirty = true;
                EditorGUILayout.BeginVertical("frameBox");
                EditorGUILayout.LabelField(tip);
                for (int i = 0; i < needDrawIndexCache.Count; i++)
                {
                    var collectedIndex = needDrawIndexCache[i];
                    EditorGUILayout.BeginHorizontal("badge");
                    var n = pnames.GetArrayElementAtIndex(collectedIndex);
                    var name = n.stringValue;
                    var v = pvalues.GetArrayElementAtIndex(collectedIndex);
                    var value = v.objectReferenceValue;
                    var newName = EditorGUILayout.DelayedTextField(name);
                    if (newName != name)
                    {
                        n.stringValue = newName;
                        dirty = true;
                    }
                    GUI.enabled = false;
                    EditorGUILayout.ObjectField(value, value == null ? typeof(Object):  value.GetType());
                    GUI.enabled = true;

                    if (GUILayout.Button(deletaTip, GUILayout.MaxWidth(20)))
                    {
                        deleteIndex = collectedIndex;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                if (deleteIndex != -1)
                {
                    RemoveByIndex(vesselEditor, deleteIndex);
                }

                if (dirty)
                {
                    obj.ApplyModifiedProperties();
                    EditorUtility.SetDirty(vessel);
                }
            }
        }



        #endregion

        bool AcceptDragCondition(Object[] objs)
        {
            if (objs == null || objs.Length == 0) return false;

            var first = objs[0];
            if (first == selfVessel) return false;

            return true;
        }

        void AddObjectForSelf(UnityEngine.Object obj)
        {
            Add(selfVesselEditor, GetDefaultNodeName(obj), obj);
        }
        void AddObjectForParent(UnityEngine.Object obj)
        {
            Add(parentVesselEditor, GetDefaultNodeName(obj), obj);
        }

        void AddObjectForSelf(string key, UnityEngine.Object obj)
        {
            Add(selfVesselEditor, key, obj);
        }
        void AddObjectForParent(string key, UnityEngine.Object obj)
        {
            Add(parentVesselEditor, key, obj);
        }
        void RemoveObjectForSelf(UnityEngine.Object obj)
        {
            Remove(selfVesselEditor, obj);
        }
        void RemoveObjectForParent(UnityEngine.Object obj)
        {
            Remove(parentVesselEditor, obj);
        }
        void RemoveObjectForSelf(string key)
        {
            Remove(selfVesselEditor, key);
        }
        void RemoveObjectForParent(string key)
        {
            Remove(parentVesselEditor, key);
        }
   
        void MoveSerilize(Editor src, Editor dst, List<int> index)
        {
            var srcobj = src.serializedObject;
            var srcnames = srcobj.FindProperty("names");
            var srcvalues = srcobj.FindProperty("values");

            var srcCount = srcnames.arraySize;

            var dstobj = dst.serializedObject;
            var dstnames = dstobj.FindProperty("names");
            var dstvalues = dstobj.FindProperty("values");

            var count = dstvalues.arraySize;
            var targetCount = count + index.Count;
            dstvalues.arraySize = targetCount;
            dstnames.arraySize = targetCount;

            index.Sort();

            for (int i = index.Count - 1; i >=0 ; i--)
            {
                //copy
                var srcIndex = index[i];
                var name = srcnames.GetArrayElementAtIndex(srcIndex).stringValue;
                var value = srcvalues.GetArrayElementAtIndex(srcIndex).objectReferenceValue;
                targetCount--;
                dstnames.GetArrayElementAtIndex(targetCount).stringValue = name;
                dstvalues.GetArrayElementAtIndex(targetCount).objectReferenceValue = value;

                // move
                for (int j = srcIndex; j < srcCount ; j++)
                {
                    srcnames.MoveArrayElement(j + 1, j);
                    srcvalues.MoveArrayElement(j + 1, j);
                }
            }

            srcnames.arraySize = srcCount - index.Count;
            srcvalues.arraySize = srcCount - index.Count;

            srcobj.ApplyModifiedProperties();
            dstobj.ApplyModifiedProperties();
            EditorUtility.SetDirty(src.target);
            EditorUtility.SetDirty(dst.target);
        }


        const string InsertTip = "父节容器中存在该节点或子节点的一些引用，如果继续添加容器，将会自动将这些引用挪动到新的容器中，确定继续吗?";
        void AddVessel(float x, float y, bool tipRepeat)
        {
            var go = target as GameObject;
            if (go == null) return;
            int selectIndex = defaultSelectIndex;
            var repeatIndexCollect = CheckCollectedInParent();
            // 当只有一个组件的情况下
            if (!tipRepeat && VesselType.Length == 1)
            {
                // 父类中已经存在了
                if (repeatIndexCollect != null && repeatIndexCollect.Count > 0)
                {
                    System.Action<float, float> drawCollected = (maxwidth, maxheight) =>
                    {
                        DrawSerializedSimple(parentVesselEditor, repeatIndexCollect, maxwidth, maxheight);
                    };

                    System.Action confirm = () =>
                    {
                        Undo.RecordObject(go, "ADD_VESSEL_COMPONENT");
                        Undo.RecordObject(parentVessel.gameObject, "MOVE_VESSEL_ITEM");
                        selfVessel = go.AddComponent(VesselType[selectIndex]) as Vessel;
                        MoveSerilize(parentVesselEditor, selfVesselEditor, repeatIndexCollect);
                    };
                    CustomUI.ShowFloatWindow(new Vector2(x, y), InsertTip, drawCollected, confirm, EmptyAction);
                }
                else
                {
                    Undo.RecordObject(go, "ADD_VESSEL_COMPONENT");
                    selfVessel = go.AddComponent(VesselType[selectIndex]) as Vessel;
                    EditorUtility.SetDirty(go);
                }
                return;
            }
           
            System.Action<float, float> draw = (maxWidth, maxHeight) =>
            {
                if (repeatIndexCollect != null && repeatIndexCollect.Count > 0)
                {
                    DrawSerializedSimple(parentVesselEditor, repeatIndexCollect, maxWidth, maxHeight);
                    EditorGUILayout.Space();
                }
                for (int i = 0; i < VesselType.Length; i++)
                {
                    var select = EditorGUILayout.ToggleLeft(VesselType[i].Name, i == selectIndex, GUILayout.MaxWidth(maxWidth));
                    if (select)
                    {
                        selectIndex = i;
                    }
                }
            };

            System.Action onconfirm = () =>
            {
                defaultSelectIndex = selectIndex;
                EditorPrefs.SetString(LAST_ADD_KEY, VesselType[selectIndex].FullName);
    
                // 父类中已经存在了
                if (repeatIndexCollect!= null && repeatIndexCollect.Count > 0)
                {
                    Undo.RecordObject(go, "ADD_VESSEL_COMPONENT");
                    Undo.RecordObject(parentVessel.gameObject, "MOVE_VESSEL_ITEM");
                    selfVessel = go.AddComponent(VesselType[selectIndex]) as Vessel;
                    MoveSerilize(parentVesselEditor, selfVesselEditor, repeatIndexCollect);
                }
                else
                {
                    Undo.RecordObject(go, "ADD_VESSEL_COMPONENT");
                    go.AddComponent(VesselType[selectIndex]);
                    EditorUtility.SetDirty(go);
                }
            };

            var tip = tipRepeat && (repeatIndexCollect!= null &&repeatIndexCollect.Count>0) ? InsertTip : (tipRepeat?"父节点中已经存在容器,确定继续添加？": "请选择需要添加的容器");
            CustomUI.ShowFloatWindow(new Vector2(x, y), tip, draw, onconfirm, EmptyAction);
        }

        /// <summary>
        /// 检查当前是否允许添加Vessel
        /// 1. 存在父节点的Vessel，并且父节点中添加了自身以及子节点的任何引用，都不能直接添加
        /// 
        /// </summary>
        List<int> CheckCollectedInParent()
        {
            if (parentVessel == null) return null;

            var transform = (target as GameObject).transform;
            List<int> result = new List<int>();
            var obj = parentVesselEditor.serializedObject;

            var pvalues = obj.FindProperty("values");
            var count = pvalues.arraySize;

            for (int i = 0; i < count; i++)
            {
                var value = pvalues.GetArrayElementAtIndex(i);
                var refObj = value.objectReferenceValue;
                if (refObj is Component c)
                {
                  var isChild = c.transform == transform ||  c.transform.IsChildOf(transform);
                    if (isChild)
                    {
                        result.Add(i);
                    }
                }
            }

            return result;
        }


        void RemoveVessel()
        {
            var go = target as GameObject;
            if (go == null) return;

            var old = go.GetComponent<Vessel>();
            if (old == null) return;

            Undo.RecordObject(go, "REMOVE_VESSEL");
            Vessel[] components = go.GetComponents<Vessel>();

            if (components != null && components.Length > 0)
            {
                for (int i = 0; i < components.Length; i++)
                {
                   UnityEngine.Object.DestroyImmediate(components[i]);
                }
            }
  
        }

        void UpdateVessel()
        {
            nodeType = VesselNodeType.NoRoot;
            var go = target as GameObject;
            if (go == null) return;

            selfVessel = go.GetComponent<Vessel>();
            parentVessel = FindParentVessel(go.transform);

            if (selfVessel != null)
            {
                nodeType = parentVessel == null ? VesselNodeType.Root : VesselNodeType.SubRoot;
            }
        }

        static void EmptyAction() { }

        static Vessel FindParentVessel(Transform t)
        {
            if (t == null) return null;

            var p = t.parent;
            if (p == null) return null;

            var target = p.GetComponent<Vessel>();
            return target ?? FindParentVessel(p);
        }

        static Vessel FindBackVessel(Transform t)
        {
            if (t == null || t.parent == null) return null;
            var parent = t.parent;
            var index = t.GetSiblingIndex();
            for (int i = 0; i < index; i++)
            {
                var target = parent.GetChild(i).GetComponent<Vessel>();
                if (target != null) return target;
            }
            return null;
        }

        static Vessel FindForwardVessel(Transform t)
        {
            if (t == null || t.parent == null) return null;
            var parent = t.parent;
            var index = t.GetSiblingIndex();
            for (int i = index + 1; i < parent.childCount; i++)
            {
                var target = parent.GetChild(i).GetComponent<Vessel>();
                if (target != null) return target;
            }
            return null;
        }

        static List<Transform> searchList1 = new List<Transform>(64);
        static List<Transform> searchList2 = new List<Transform>(64);
        static Vessel FindChildVessel(Transform t)
        {
            if (t == null || t.childCount == 0) return null;

            searchList1.Clear();
            searchList1.Add(t);

            while (true)
            {
                var hasResult = SearchChildList(searchList2, searchList1);
                if (!hasResult) return null;

                foreach (var item in searchList2)
                {
                    var target = item.GetComponent<Vessel>();
                    if (target != null) return target;
                }
                hasResult = SearchChildList(searchList1, searchList2);
                if (!hasResult) return null;
                foreach (var item in searchList2)
                {
                    var target = item.GetComponent<Vessel>();
                    if (target != null) return target;
                }
            }
        }
        static bool SearchChildList(List<Transform> result, List<Transform> target)
        {
            bool hasSerchResult = false;
            result.Clear();
            for (int i = 0; i < target.Count; i++)
            {
                var t = target[i];

                for (int j = 0; j < t.childCount; j++)
                {
                    result.Add(t.GetChild(j));
                    hasSerchResult = true;
                }
            }

            return hasSerchResult;

        }

        static bool IsEmptyVessel(Editor be)
        {
            if (be == null)
            {
                return true;
            }

            var obj = be.serializedObject;
            SerializedProperty pvalues = obj.FindProperty("values");

            var valueCount = pvalues.arraySize;
            for (int i = 0; i < valueCount; i++)
            {
                var p = pvalues.GetArrayElementAtIndex(i);
                if (p.objectReferenceValue != null)
                {
                    return false;
                }
            }

            return true;
        }


        static int IndexOf(Editor target, Object v)
        {
            if (target == null || v == null)
            {
                return -1;
            }

            var obj = target.serializedObject;
            SerializedProperty pvalues = obj.FindProperty("values");
            var c = pvalues.arraySize;
            for (int i = 0; i < c; i++)
            {
                var elementValue = pvalues.GetArrayElementAtIndex(i);
                if (elementValue.objectReferenceValue == v)
                {
                    return i;
                }
            }
            return -1;
        }


        static void Add(Editor target, string key, UnityEngine.Object v)
        {
            if (target == null || v == null)
            {
                return;
            }
            var obj = target.serializedObject;
            SerializedProperty pnames = obj.FindProperty("names");
            SerializedProperty pvalues = obj.FindProperty("values");

            var count = pvalues.arraySize;
            var oldIndex = -1;
            for (int i = 0; i < count; i++)
            {
                var elementValue = pvalues.GetArrayElementAtIndex(i);
                if (elementValue.objectReferenceValue == v)
                {
                    oldIndex = i;
                    break;
                }
            }

            if (oldIndex != -1)
            {
                //pnames.GetArrayElementAtIndex(oldIndex).stringValue = key;
                Debug.LogError("欲添加的对象引用已经存在于容器中，未执行重复添加");
                return;
            }
            else
            {
                Undo.RecordObject(target.target, "ADD_VESSEL_ITEM");
                pnames.InsertArrayElementAtIndex(count);
                pnames.GetArrayElementAtIndex(count).stringValue = key;
                pvalues.InsertArrayElementAtIndex(count);
                pvalues.GetArrayElementAtIndex(count).objectReferenceValue = v;
            }

            // TODO: sort by gameobject
            obj.ApplyModifiedProperties();
            EditorUtility.SetDirty(target.target);
        }

        static void Remove(Editor target, UnityEngine.Object v)
        {
            var index = ValueIndexOf(target, v);
            RemoveByIndex(target, index);
        }

        static void Remove(Editor target, string v)
        {
            var index = NameIndexOf(target, v);
            RemoveByIndex(target, index);
        }

        static void RemoveByIndex(Editor target, int index)
        {
            if (index == -1 || target == null) return;
            var obj = target.serializedObject;
            SerializedProperty pnames = obj.FindProperty("names");
            SerializedProperty pvalues = obj.FindProperty("values");
            Undo.RecordObject(target.target, "DELETE_VESSEL_ITEM");
            Delete(pnames, index);
            Delete(pvalues, index);
            obj.ApplyModifiedProperties();
            EditorUtility.SetDirty(target.target);
        }

        static void Delete(SerializedProperty sp, int index)
        {
            int count = sp.arraySize;
            for (int i = index; i < count; i++)
            {
                if (i + 1 < count)
                {
                    sp.MoveArrayElement(i + 1, i);
                }
            }
            sp.arraySize = count - 1;
        }

        static int NameIndexOf(Editor target, string key)
        {
            if (target == null) return -1;

            var obj = target.serializedObject;
            SerializedProperty pnames = obj.FindProperty("names");
            var count = pnames.arraySize;
            for (int i = 0; i < count; i++)
            {
                var elementValue = pnames.GetArrayElementAtIndex(i);
                if (elementValue.stringValue == key)
                {
                    return i;
                }
            }
            return -1;
        }

        static int ValueIndexOf(Editor target, UnityEngine.Object v)
        {
            if (target == null) return -1;

            var obj = target.serializedObject;
            SerializedProperty pvalus = obj.FindProperty("values");
            var count = pvalus.arraySize;
            for (int i = 0; i < count; i++)
            {
                var elementValue = pvalus.GetArrayElementAtIndex(i);
                if (elementValue.objectReferenceValue == v)
                {
                    return i;
                }
            }
            return -1;
        }

        static string GetDefaultNodeName(UnityEngine.Object obj)
        {
            return $"{obj.name}{obj.GetType().Name}";
        }

        static List<GUILayoutOption> options = new List<GUILayoutOption>();

        static void DrawSerializedSimple(Editor editor,List<int> assginIndexs,  float maxwidth = -1, float maxHeight = -1)
        {
            if (editor == null) return;

            var obj = editor.serializedObject;
            SerializedProperty pnames = obj.FindProperty("names");
            SerializedProperty pvalues = obj.FindProperty("values");

           
            options.Clear();
            if (maxwidth > 0)
            {
                options.Add(GUILayout.MaxWidth(maxwidth));

            }
            if (maxHeight > 0)
            {
                options.Add(GUILayout.MaxHeight(maxHeight));
            }

            var opts = options.ToArray();

            GUI.enabled = false;

            if (assginIndexs!= null && assginIndexs.Count > 0)
            {

                for (int i = 0; i < assginIndexs.Count; i++)
                {
                    var index = assginIndexs[i];
                    var k = pnames.GetArrayElementAtIndex(index).stringValue;
                    var v = pvalues.GetArrayElementAtIndex(index).objectReferenceValue;

                    if (opts.Length > 0)
                    {
                        EditorGUILayout.ObjectField(k, v, v == null ? typeof(Object) : v.GetType(), opts);
                    }
                    else
                    {
                        EditorGUILayout.ObjectField(k, v, v == null ? typeof(Object) : v.GetType());
                    }
                }
            }
            else
            {
                var count = pvalues.arraySize;
                for (int i = 0; i < count; i++)
                {
                    var name = pnames.GetArrayElementAtIndex(i).stringValue;
                    var value = pvalues.GetArrayElementAtIndex(i).objectReferenceValue;
                    if (opts.Length > 0)
                    {
                        EditorGUILayout.ObjectField(name, value, value == null ? typeof(Object): value.GetType(), opts);
                    }
                    else
                    {
                        EditorGUILayout.ObjectField(name, value, value == null ? typeof(Object) : value.GetType());
                    }

                }
            }
        
            GUI.enabled = true;
        }

        static void PingVessel(Vessel b)
        {
            if (b == null) return;
            Selection.activeObject = b;
            EditorGUIUtility.PingObject(b.gameObject);
        }
    }

}