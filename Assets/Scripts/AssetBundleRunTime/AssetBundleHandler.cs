using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class RequestHandler
{
    protected string m_id;
    public event OnRequestDone onRequestDone;

    public abstract bool isDone { get; }
    public virtual void Dispose()
    {
        onRequestDone = null;
    }

    public void InvokeCallback()
    {
        if (onRequestDone != null)
        {
            onRequestDone();
        }
    }

    public void AssignID(string id)
    {
        m_id = id;
    }

    public bool isID(string id)
    {
        return m_id == id;
    }
}

public delegate void OnRequestDone();

public class MAssetBundleCreateRequest :  RequestHandler
{
    AssetBundleCreateRequest request;


    public AssetBundle assetBundle
    {
        get
        {
            if (isDone)
            {
                return request.assetBundle;
            }
            return null;
        }
    }

    public override bool isDone
    {
        get
        {
            return request.isDone;
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        request = null;
    }

    public MAssetBundleCreateRequest() { }

    public void SetRequest(AssetBundleCreateRequest request)
    {
        this.request = request;
    }
}

public class MAssetBundleRequest : RequestHandler
{
    AssetBundleRequest request;

    public Object asset
    {
        get
        {
            if (isDone)
            {
                return request.asset;
            }
            return null;
        }
    }

    public override bool isDone
    {
        get
        {
            return request.isDone;
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        request = null;
    }

    public MAssetBundleRequest() { }

    public void SetRequest(AssetBundleRequest request)
    {
        this.request = request;
    }
}

