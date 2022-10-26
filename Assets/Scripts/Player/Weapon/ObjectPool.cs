using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    private Dictionary<string, List<GameObject>> pool;
    public GameObject bullet;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        pool = new Dictionary<string, List<GameObject>>();
        pool.Add("Bullet", new List<GameObject>() { bullet });
    }

    // 取对象
    public GameObject GetObjectFromPool(string objName, Vector3 pos, Quaternion qua)
    {
        GameObject go;
        // 当池子中有相应的键值对，并且里面有可以使用的对象，则直接拿出来用
        if (pool.ContainsKey(objName) && pool[objName].Count > 0)
        {
            //把相应的键值对中的第一个对象取出来，并从池子中移除
            go = pool[objName][0];
            pool[objName].RemoveAt(0);
            //激活这个对象
            go.SetActive(true);
        }
        else
        {
            //若不满足上面条件，则实例化一个新的对象出来
            go = Instantiate(Resources.Load("Prefab/Bullet/" + objName) as GameObject);
        }
        //设置一下得到的对象的位置以及旋转角度
        go.transform.position = pos;
        go.transform.rotation = qua;
        return go;
    }

    //存对象(参数为我们即将存入的对象)
    public void PushObjectToPool(GameObject go)
    {
        string prefabName = go.name.Split('(')[0];
        // 判断池子中有没有相应的键值对，没有则创建一个新的键值对，有则直接往里存
        if (pool.ContainsKey(prefabName))
        {
            pool[prefabName].Add(go);
        }
        else
        {
            // 在池子中创建一个新的键值对，并初始化List
            pool[prefabName] = new List<GameObject>() { go };
        }
        // 取消激活该对象
        go.SetActive(false);
    }
}


