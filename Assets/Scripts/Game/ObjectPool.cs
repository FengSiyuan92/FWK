using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    定义了常用数据结构的对象池，减少频繁new临时变量产生的gc。每次新增一个静态类对象池之前需要先考虑gc是否频繁，如不频繁，建议构建Object对象缓存在自己身上即可。

    获取一个List的方法    ListPool<T>.Get()   使用完成之后 需要手动调用 ListPool<T>.Put()

 */
public class ObjectPool<T> where T : new()
{
    Stack<T> pool = new Stack<T>();
    System.Action<T> onPush;

    public T Pop()
    {
        if (pool.Count > 0)
        {
            return pool.Pop();
        }
        return new T();
    }

    public void Push(T instance)
    {
        if (instance != null)
        {
            if (onPush!= null)
            {
                onPush(instance);
            }
            pool.Push(instance);
        }
    }

    public ObjectPool() { }

    public ObjectPool(System.Action<T> onPush)
    {
        this.onPush = onPush;
    }
    
    public void Cut(int maxSize)
    {
        if (pool.Count > maxSize)
        {
            for (int i = 0; i < pool.Count - maxSize; i++)
            {
                pool.Pop();
            }
        }
    }
}


#region  集合对象池
public class ListPool<T>
{
    public static ObjectPool<List<T>> pool;

    static ListPool()
    {
        pool = new ObjectPool<List<T>>((list)=>
        {
            list.Clear();
        });
    }

    public static List<T> Get()
    {
        return pool.Pop();
    }

    public static void Put(List<T> instance)
    {
        pool.Push(instance);
    }
}

public class HashSetPool<T>
{
    public static ObjectPool<HashSet<T>> pool;

    static HashSetPool()
    {
        pool = new ObjectPool<HashSet<T>>((table) =>
        {
            table.Clear();
        });
    }

    public static HashSet<T> Get()
    {
        return pool.Pop();
    }

    public static void Put(HashSet<T> instance)
    {
        pool.Push(instance);
    }
}

#endregion

public class MAssetBundleCreateRequestPool
{
    public static ObjectPool<MAssetBundleCreateRequest> pool;

    static MAssetBundleCreateRequestPool()
    {
        pool = new ObjectPool<MAssetBundleCreateRequest>((handler) =>
        {
            handler.Dispose();
        });
    }
    public static MAssetBundleCreateRequest Get(AssetBundleCreateRequest request)
    {
        var handler = pool.Pop();
        handler.SetRequest(request);
        return handler;
    }

    public static void Put(MAssetBundleCreateRequest instance)
    {
        pool.Push(instance);
    }
}

public class MAssetBundleRequestPool
{
    public static ObjectPool<MAssetBundleRequest> pool;
    static MAssetBundleRequestPool()
    {
        pool = new ObjectPool<MAssetBundleRequest>((handler) =>
        {
            handler.Dispose();
        });
    }

    public static MAssetBundleRequest Get(AssetBundleRequest request)
    {
        var handler = pool.Pop();
        handler.SetRequest(request);
        return handler;
    }

    public static void Put(MAssetBundleRequest instance)
    {
        pool.Push(instance);
    }
}

public class LoadedBundlePool
{
    public static ObjectPool<LoadedAssetBundle> pool;
    static LoadedBundlePool()
    {
        pool = new ObjectPool<LoadedAssetBundle>((loaded) =>
        {
            loaded.Clear();
        });
    }
    public static LoadedAssetBundle Get(AssetBundle bundle)
    {
        var loaded = pool.Pop();
        loaded.SetBundle(bundle);
        return loaded;
    }

    public static void Put(LoadedAssetBundle instance)
    {
        pool.Push(instance);
    }
}