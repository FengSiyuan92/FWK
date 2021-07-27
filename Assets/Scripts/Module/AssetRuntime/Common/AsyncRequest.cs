using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AssetRuntime
{
    public delegate Loaded LoadedCreater();
    public delegate void OnRequestDone(Loaded loaded);

    /// <summary>
    /// 异步操作申请
    /// </summary>
    public abstract class AsyncRequest : IReuseObject
    {
        protected string m_Id;
        internal event OnRequestDone onRequestDone;
        Loaded m_Loaded;
        LoadedCreater m_Creater;
        AsyncOperation m_Operation;

        public bool isDone {
            get
            {
                if (m_Operation == null)
                {
                    var a = 0;
                }
                return m_Operation.isDone;
            }
        }
        public bool IsID(string id) => m_Id == id;

        /// <summary>
        /// 当请求完成时的回调
        /// </summary>
        public virtual void OnRequestOver()
        {
            if (m_Creater != null)
            {
                m_Loaded = m_Creater();
            }

            if (onRequestDone != null)
            {
                onRequestDone(m_Loaded);
            }
        }

        public void AssignID(string id)
        {
            m_Id = id;
        }

        /// <summary>
        /// 设置loaded创建器
        /// </summary>
        /// <param name="creater"></param>
        public void SetLoadedCreater(LoadedCreater creater)
        {
            m_Creater = creater;
        }

        public virtual void SetAsynOperation(AsyncOperation operation)
        {
            m_Operation = operation;
        }

        public virtual void Clear()
        {
            m_Id = null;
            onRequestDone = null;
            m_Loaded = null;
            m_Creater = null;
            m_Operation = null;
        }
    }

   

}