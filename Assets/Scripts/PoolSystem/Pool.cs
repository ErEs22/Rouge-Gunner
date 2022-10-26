using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 单个对象的对象池
/// </summary>
[System.Serializable]
public class Pool
{
    public GameObject Prefab => prefab;
    public int Size => size;
    public int RuntimeSize => queue.Count;
    [SerializeField] GameObject prefab;
    [SerializeField] int size = 1;
    Queue<GameObject> queue;
    Transform parent;
    public void Initialize(Transform parent)//初始化对象池，根据size生成相应大小的对象队列
    {
        queue = new Queue<GameObject>();
        this.parent = parent;
        for (int i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }
    private GameObject Copy()//生成一个新的对象加入对象池
    {
        var copy = GameObject.Instantiate(prefab, parent);
        copy.SetActive(false);
        return copy;
    }
    private GameObject AvailableObject()//可用对象
    {
        GameObject availableObject = null;
        if (queue.Count > 0 && !queue.Peek().activeSelf)//如果队列不为空并且第一个对象不是激活的，则该对象为可用对象，否则新生成一个对象
        {
            availableObject = queue.Dequeue();
        }
        else
        {
            //queue.Enqueue(Copy());
            availableObject = Copy();
        }
        queue.Enqueue(availableObject);
        return availableObject;
    }
    public GameObject PreparedObject()//准备可用对象
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.transform.position = position;
        preparedObject.SetActive(true);
        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.SetActive(true);
        return preparedObject;
    }
    public GameObject PreparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;
        preparedObject.SetActive(true);
        return preparedObject;
    }
}
