using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace AssetRuntime
{
    [LuaCallCSharp]
    public class InterruptNote
    {
        protected bool isInterrupt;

        public virtual void OnInterrupt()
        {
            isInterrupt = true;
        }
    }


    public partial class Asset
    {
        class GetAssetNote : InterruptNote, IRequestNote
        {
            public string targetAssetName;
            public System.Action<Object> onAssetLoaded;
            public Bundle.LoadedBundle loadedBundle;
            internal LoadBundleRequestNote parent;

            public string TargetName => targetAssetName;

            public void OnRequestOver(Loaded loaded)
            {
                loaded.AddReferenceCount();

                if (isInterrupt)
                {
                    loaded.TryUnload();
                    m_RequestBundleNotePool.Push(this.parent);
                    m_GetAssetNotePool.Push(this);
                    return;
                }

                var loadedAsset = loaded as LoadedAsset;
                SafeCall.Call(onAssetLoaded, loadedAsset.Asset);
                m_RequestBundleNotePool.Push(this.parent);
                m_GetAssetNotePool.Push(this);
            }

            public void Clear()
            {
                isInterrupt = false;
                targetAssetName = null;
                onAssetLoaded = null;
                loadedBundle = null;
                parent = null;
            }
        }

        class LoadBundleRequestNote : InterruptNote, IRequestNote
        {
            public string targetAssetName;
            public System.Action<Object> onAssetLoaded;
            public string TargetName => targetAssetName;
            GetAssetNote note;
            public void OnRequestOver(Loaded loaded)
            {
                // 资源对bundle的引用加一
                loaded.AddReferenceCount();

                // 如果在bundle加载过程中就被终止
                if (isInterrupt)
                {
                    loaded.TryUnload();
                    m_RequestBundleNotePool.Push(this);
                    return;
                }

                var bundle = loaded as Bundle.LoadedBundle;
                note = m_GetAssetNotePool.Get();
                note.targetAssetName = targetAssetName;
                note.onAssetLoaded = onAssetLoaded;
                note.loadedBundle = bundle;
                note.parent = this;

                GetOrRequestLoaded(targetAssetName, note, CreateLoadAssetHandler, GetHandlerName);
            }

            public override void OnInterrupt()
            {
                base.OnInterrupt();
                if (note!= null)
                {
                    note.OnInterrupt();
                }
            }

            public void Clear()
            {
                note = null;
                targetAssetName = null;
                onAssetLoaded = null;
                isInterrupt = false;
            }
        }

    }
}