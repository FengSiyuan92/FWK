using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetRuntime
{
    public abstract class Loaded
    {
        uint m_referenceCount;
        public void AddReferenceCount()
        {
            m_referenceCount++;
        }
        public void TryUnload()
        {
            m_referenceCount--;
            if (m_referenceCount == 0)
            {
                OnUnload();
            }
        }
        protected abstract void OnUnload();

        public void FocusUnload()
        {
            OnUnload();
        }
    }

}