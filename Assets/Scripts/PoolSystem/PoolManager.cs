using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 
/// </summary>
public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] playerProjectilePools;//玩家子弹对象池
    [SerializeField] Pool[] enemyPools;//敌人对象池
    [SerializeField] Pool[] enemyProjectilePools;//敌人子弹对象池
    static Dictionary<GameObject, Pool> dictionary;//对象池字典，包含所有对象池
    private void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();
        Initialize(playerProjectilePools);
        Initialize(enemyPools);
        Initialize(enemyProjectilePools);
    }
#if UNITY_EDITOR
    private void OnDestroy()
    {
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyPools);
        CheckPoolSize(enemyProjectilePools);
    }
#endif
    private void CheckPoolSize(Pool[] pools)//检查对象池的大小
    {
        foreach (var pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning(string.Format("Pool:{0} has a runtime size {1} bigger than its initial size {2}!",
                    pool.Prefab.name,
                    pool.RuntimeSize,
                    pool.Size));
            }
        }
    }
    private void Initialize(Pool[] pools)//初始化对象池，加入字典
    {
        foreach (var pool in pools)
        {
#if UNITY_EDITOR
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("Same prefab in multiple pools Prefab:" + pool.Prefab.name);
                continue;
            }
#endif
            dictionary.Add(pool.Prefab, pool);
            Transform poolParent = new GameObject("Pool:" + pool.Prefab.name).transform;//创建新的空对象作为父物体
            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }

    /// <summary>
    /// 从对象池取出使用
    /// </summary>
    /// <param name="prefab">取出的对象</param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find prefab:" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject();
    }
    public static GameObject Release(GameObject prefab, Vector3 position)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find prefab:" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position);
    }
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find prefab:" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position, rotation);
    }
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could NOT find prefab:" + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position, rotation, localScale);
    }
}
