using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetRuntime
{
    public partial class Asset
    {
        class GetAssetNote : IRequestNote
        {
            public string targetAssetName;
            public System.Action<Object> onAssetLoaded;
            public LoadedBundle loadedBundle;

            public string TargetName => targetAssetName;

            public void RequestOver(Loaded loaded)
            {
                loaded.AddReferenceCount();
                var loadedAsset = loaded as LoadedAsset;
                SafeCall.Call(onAssetLoaded, loadedAsset.Asset);
                m_GetAssetNotePool.Push(this);
            }

            public void Clear()
            {
                targetAssetName = null;
                onAssetLoaded = null;
                loadedBundle = null;
            }
        }

        class LoadAssetNote : IRequestNote
        {
            public string targetAssetName;
            public System.Action<Object> onAssetLoaded;


            public string TargetName => targetAssetName;


            public void RequestOver(Loaded loaded)
            {
                var bundle = loaded as LoadedBundle;

                var note = m_GetAssetNotePool.Get();
                note.targetAssetName = targetAssetName;
                note.onAssetLoaded = onAssetLoaded;
                note.loadedBundle = bundle;

                GetOrRequestLoaded(targetAssetName, note, CreateLoadAssetHandler, GetHandlerName);
                m_LoadAssetNotePool.Push(this);
            }

            public void Clear()
            {
                targetAssetName = null;
                onAssetLoaded = null;
            }
        }
    }
}