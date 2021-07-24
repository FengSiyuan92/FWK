using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*
    定义了常用数据结构的对象池，减少频繁new临时变量产生的gc。每次新增一个静态类对象池之前需要先考虑gc是否频繁，
    如不频繁，建议构建Object对象缓存在自己身上即可。

    获取一个List的方法    ListPool<T>.Get()   使用完成之后 需要手动调用 ListPool<T>.Push()

 */
public class ObjectPool<T> where T : new()
{
    Stack<T> pool = new Stack<T>();
    Action<T> _onPush;
    Action<T> _onPop;
    Action<T> _onDispose;
    int _maximum;

    public T Pop()
    {
        if (pool.Count > 0)
        {
            T item = pool.Pop();
            if (_onPop != null)
            {
                _onPop(item);
            }
            return item;
        }
        return new T();
    }

    public void Push(T instance)
    {
        if (instance != null)
        {
            if (_onPush!= null)
            {
                _onPush(instance);
            }
            pool.Push(instance);
        }
    }

    public ObjectPool() { }

    public ObjectPool(Action<T> onPop, Action<T> onPush, Action<T> onDispose, int maximum = -1)
    {
        this._onPop = onPop;
        this._onPush = onPush;
        this._onDispose = onDispose;
        this._maximum = maximum;
    }

    /// <summary>
    /// 将池容量进行裁剪,被裁剪掉的对象将会执行onDispose回调(如果有的话)
    /// 传入负数将使用默认数量(构造时传入的值),如果默认数量也为-1,则不进行裁剪
    /// </summary>
    /// <param name="targetCount">目标数量</param>
    public void Cut(int targetCount = -1)
    {
        targetCount = targetCount == -1 ? _maximum : targetCount;

        if (pool.Count > targetCount)
        {
            for (int i = 0; i < pool.Count - _maximum; i++)
            {
                T item = pool.Pop();
                if (_onDispose != null)
                {
                    _onDispose(item);
                }
            }
        }
    }
}


public class ListPool<T>
{
    public static ObjectPool<List<T>> pool;

    static ListPool()
    {

        pool = new ObjectPool<List<T>>(null, (list) =>
        {
            list.Clear();
        }, null, 5);
    }

    public static List<T> Get()
    {
        return pool.Pop();
    }

    public static void Push(List<T> instance)
    {
        pool.Push(instance);
    }
}

public class HashSetPool<T>
{
    public static ObjectPool<HashSet<T>> pool;

    static HashSetPool()
    {
        pool = new ObjectPool<HashSet<T>>(null, (table) =>
        {
            table.Clear();
        }, null, 3);
    }

    public static HashSet<T> Get()
    {
        return pool.Pop();
    }

    public static void Push(HashSet<T> instance)
    {
        pool.Push(instance);
    }
}


public class MAssetBundleCreateRequestPool
{
    public static ObjectPool<MAssetBundleCreateRequest> pool;

    static MAssetBundleCreateRequestPool()
    {
        pool = new ObjectPool<MAssetBundleCreateRequest>(null, (handler) =>
         {
             handler.Dispose();
         }, null, 10);
    }
    public static MAssetBundleCreateRequest Get(AssetBundleCreateRequest request)
    {
        var handler = pool.Pop();
        handler.SetRequest(request);
        return handler;
    }

    public static void Push(MAssetBundleCreateRequest instance)
    {
        pool.Push(instance);
    }

    public Loaded loaded;
}

public class MAssetRequestPool
{
    public static ObjectPool<MAssetRequest> pool;
    static MAssetRequestPool()
    {
        pool = new ObjectPool<MAssetRequest>(null, (handler) =>
         {
             handler.Dispose();
         }, null, 5);
    }

    public static MAssetRequest Get(AssetBundleRequest request)
    {
        var handler = pool.Pop();
        handler.SetRequest(request);
        return handler;
    }

    public static void Push(MAssetRequest instance)
    {
        pool.Push(instance);
    }
}

public class LoadedBundlePool
{
    public static ObjectPool<LoadedAssetBundle> pool;
    static LoadedBundlePool()
    {
        pool = new ObjectPool<LoadedAssetBundle>(null, (loaded) =>
        {
            loaded.Clear();
        }, null, 10);
    }
    public static LoadedAssetBundle Get(AssetBundle bundle)
    {
        var loaded = pool.Pop();
        loaded.SetBundle(bundle);
        return loaded;
    }

    public static void Push(LoadedAssetBundle instance)
    {
        pool.Push(instance);
    }
}

public class LoadedAssetPool
{
    public static ObjectPool<LoadedAsset> pool;

    static LoadedAssetPool()
    {
        pool = new ObjectPool<LoadedAsset>(null, (loaded) =>
        {
            loaded.Clear();
        }, null, 10);
    }
    public static LoadedAsset Get( UnityEngine.Object asset, LoadedAssetBundle bundle)
    {
        var loaded = pool.Pop();
        loaded.SetInfo(bundle, asset);
        return loaded;
    }

    public static void Push(LoadedAsset instance)
    {
        pool.Push(instance);
    }
}