using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetRuntime
{
    public abstract class Loaded
    {
        protected int m_referenceCount;
        public void AddReferenceCount()
        {
            m_referenceCount++;
            Debug.Log("m_referenceCount = " + m_referenceCount);
        }
        public void TryUnload()
        {
            m_referenceCount--;
            if (m_referenceCount == 0)
            {
                OnUnload();
            }
            Debug.Log("m_referenceCount = " + m_referenceCount);
        }
        protected abstract void OnUnload();

        public void FocusUnload()
        {
            OnUnload();
        }
    }

}