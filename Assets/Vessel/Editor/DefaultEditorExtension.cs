/****************************************************************
* FileName:     DefaultEditorExtension.cs 
* Author:       fengsy01 
* Version:      1.0 
* UnityVersion：2019.4.28f1 
* Date:         2022-11-03 16:20 
* Description:    
* History: 
* ****************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

using System.Linq;
using System.Reflection;
using Object = UnityEngine.Object;

namespace VesselTool
{
	public class TypeCollectpr
	{
		private static Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));
		private static Dictionary<string, System.Type> editorTypes = new Dictionary<string, Type>();

		public static Type Get(string k)
		{
			editorTypes.TryGetValue(k, out System.Type t);
			return t;
		}

		static TypeCollectpr()
		{
			var types = editorAssembly.GetTypes();
			var ceType = typeof(CustomEditor);
			var files = ceType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			editorTypes.Clear();
			var exTypeName = files.Single(f => f.Name == "m_InspectedType");
			for (int i = 0; i < types.Length; i++)
			{
				var t = types[i];
				var attribute = t.GetCustomAttribute<CustomEditor>();
				if (attribute == null) continue;

				var exType = exTypeName.GetValue(attribute) as System.Type;
				if (exType == null) continue;

				//Debug.Log($"{exType.FullName}  :  {t.FullName}");

				if (!editorTypes.ContainsKey(exType.FullName))
				{
					editorTypes.Add(exType.FullName, t);
				}
				//else
				//{
				//	var t1 = exType.FullName;
				//	var t2 = editorTypes[t1].FullName;
				//	var t3 = t.FullName;
				//	Debug.Log($"repeat register {t1} : {t2}=> {t3}");
				//}

			}
		}
	}


	public abstract class DecoratorEditor : Editor
	{

		private static readonly object[] EMPTY_ARRAY = new object[0];

		#region Editor Fields


		private System.Type decoratedEditorType;

		private Editor editorInstance;

		#endregion

		private static Dictionary<string, MethodInfo> decoratedMethods = new Dictionary<string, MethodInfo>();

		protected Editor EditorInstance
		{
			get
			{
				if (editorInstance == null && targets != null && targets.Length > 0)
				{
					editorInstance = Editor.CreateEditor(targets, decoratedEditorType);
				}

				if (editorInstance == null)
				{
					Debug.LogError("Could not create editor !");
				}

				return editorInstance;
			}
		}

		public DecoratorEditor()
		{
			var flags = BindingFlags.NonPublic | BindingFlags.Instance;
			var attributes = this.GetType().GetCustomAttributes(typeof(CustomEditor), true) as CustomEditor[];
			var field = attributes.Select(editor => editor.GetType().GetField("m_InspectedType", flags)).First();
			var editedObjectType = field.GetValue(attributes[0]) as System.Type;
			decoratedEditorType = TypeCollectpr.Get(editedObjectType.FullName);
		}

		protected virtual void OnDisable()
		{
			if (editorInstance != null)
			{
				try
				{
					DestroyImmediate(editorInstance);
				}
				catch { }
			}
		}

		/// <summary>
		/// Delegates a method call with the given name to the decorated editor instance.
		/// </summary>
		protected bool CallInspectorMethod(string methodName)
		{
			MethodInfo method = null;

			// Add MethodInfo to cache
			if (!decoratedMethods.ContainsKey(methodName))
			{
				var flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

				method = decoratedEditorType.GetMethod(methodName, flags);

				decoratedMethods[methodName] = method;
			}

			method = decoratedMethods[methodName];
			if (method != null)
			{
				method.Invoke(EditorInstance, EMPTY_ARRAY);
			}

			return method != null;
		}

		protected virtual void OnSceneGUI()
		{
			CallInspectorMethod("OnSceneGUI");
		}

		protected override void OnHeaderGUI()
		{
			CallInspectorMethod("OnHeaderGUI");
		}

		public override void OnInspectorGUI()
		{
			EditorInstance.OnInspectorGUI();
		}

		public override void DrawPreview(Rect previewArea)
		{
			EditorInstance.DrawPreview(previewArea);
		}

		public override string GetInfoString()
		{
			return EditorInstance.GetInfoString();
		}

		public override GUIContent GetPreviewTitle()
		{
			return EditorInstance.GetPreviewTitle();
		}

		public override bool HasPreviewGUI()
		{
			return EditorInstance.HasPreviewGUI();
		}

		public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
		{
			EditorInstance.OnInteractivePreviewGUI(r, background);
		}

		public override void OnPreviewGUI(Rect r, GUIStyle background)
		{
			EditorInstance.OnPreviewGUI(r, background);
		}

		public override void OnPreviewSettings()
		{
			EditorInstance.OnPreviewSettings();
		}

		public override void ReloadPreviewInstances()
		{
			EditorInstance.ReloadPreviewInstances();
		}

		public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
		{
			return EditorInstance.RenderStaticPreview(assetPath, subAssets, width, height);
		}

		public override bool RequiresConstantRepaint()
		{
			return EditorInstance.RequiresConstantRepaint();
		}

		public override bool UseDefaultMargins()
		{
			return EditorInstance.UseDefaultMargins();
		}
	}


}