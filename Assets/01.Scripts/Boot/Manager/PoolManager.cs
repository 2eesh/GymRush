using UnityEngine;
using System.Collections.Generic;

public interface IPoolable
{
    void OnSpawn();
    void OnDespawn();
}
 
public class PoolManager : SingletonMonoBehaviour<PoolManager>
{
    private readonly Dictionary<GameObject, Queue<GameObject>> pools = new();
    private readonly Dictionary<GameObject, GameObject> instanceToPrefab = new();
    private readonly Dictionary<GameObject, Transform> poolRoots = new();
    
    public void CreatePool<T>(T prefab, int initialSize) where T : Component, IPoolable
    {
        CreatePoolInternal(prefab.gameObject, initialSize);
    }
    
    public T Get<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component, IPoolable
    {
        GameObject obj = GetInternal(prefab.gameObject, position, rotation);
 
        var component = obj.GetComponent<T>();
        component.OnSpawn();
        return component;
    }
 
    public T Get<T>(T prefab) where T : Component, IPoolable => Get(prefab, Vector3.zero, Quaternion.identity);
    
    public void Return<T>(T instance) where T : Component, IPoolable
    {
        GameObject obj = instance.gameObject;
 
        if (!instanceToPrefab.ContainsKey(obj))
        {
            Debug.LogWarning($"[PoolManager] 풀 소속이 아닌 오브젝트: {obj.name}. Destroy로 처리.");
            Destroy(obj);
            return;
        }
 
        if (!obj.activeSelf)
        {
            Debug.LogWarning($"[PoolManager] 중복 Return 감지: {obj.name}");
            return;
        }
 
        instance.OnDespawn();
        ReturnInternal(obj);
    }

    private void CreatePoolInternal(GameObject prefab, int initialSize)
    {
        if (pools.ContainsKey(prefab))
            return;
 
        pools[prefab] = new Queue<GameObject>();
 
        var root = new GameObject($"{prefab.name}_Pool").transform;
        root.SetParent(transform);
        poolRoots[prefab] = root;
 
        for (int i = 0; i < initialSize; i++)
        {
            var obj = CreateInstance(prefab);
            obj.SetActive(false);
            pools[prefab].Enqueue(obj);
        }
    }
 
    private GameObject GetInternal(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pools.TryGetValue(prefab, out var queue))
        {
            CreatePoolInternal(prefab, 0);
            queue = pools[prefab];
        }

        queue.TryDequeue(out GameObject obj);
        if (obj == null)
        {
            obj = CreateInstance(prefab);
        }
        
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }
 
    private void ReturnInternal(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(poolRoots[instanceToPrefab[obj]]);
        pools[instanceToPrefab[obj]].Enqueue(obj);
    }
 
    private GameObject CreateInstance(GameObject prefab)
    {
        var obj = Instantiate(prefab, poolRoots[prefab]);
        instanceToPrefab[obj] = prefab;
        return obj;
    }
}