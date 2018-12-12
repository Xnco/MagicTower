using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    private static ObjectPool instance;

    public static ObjectPool GetSingle()
    {
        if (instance == null)
        {
            GameObject tmpObj = new GameObject("ObjPool");
            instance = tmpObj.AddComponent<ObjectPool>();
        }
        return instance;
    }

    Dictionary<string, List<GameObject>> pool;
    Dictionary<string, GameObject> prefabPool;

    private void Awake()
    {
        pool = new Dictionary<string, List<GameObject>>();
        prefabPool = new Dictionary<string, GameObject>();
    }

    // 通过名字取得物体
    public GameObject GetObj(string name)
    {
        // 要取得一个物体, 先到对象池中取
        List<GameObject> tmpList;
        if (pool.TryGetValue(name, out tmpList))
        {
            // 取到了集合, 再判断集合中时候是否存在物体
            if (tmpList.Count > 0)
            {
                GameObject go = tmpList[0];
                tmpList.RemoveAt(0); // 取出后, 要从集合中移除
                go.SetActive(true);
                MTBase tmpBase = go.GetComponent<MTBase>();
                if (tmpBase != null)
                {
                    // 如果是个魔塔物体, 从对象池中将物体取出来, 将物体重置
                    tmpBase.ResetObj(); 
                }
                return go;
            }
        }

        // 第二种情况, 对象池中没有对应的物体
        GameObject goPrefab = GetPrefab(name);
        if (goPrefab == null)
        {
            Debug.LogError("加载不到 " + name +" 物体");
            return null;
        }
        GameObject newGO = GameObject.Instantiate(goPrefab);
        newGO.name = goPrefab.name; // 去掉 (clone)
        return newGO;
    }

    // 存物体到对象池中
    public void SaveObj(GameObject varGO)
    {
        List<GameObject> tempList;
        if (!pool.TryGetValue(varGO.name, out tempList))
        {
            // 取不到集合
            tempList = new List<GameObject>();
            pool.Add(varGO.name, tempList);
        }
        varGO.SetActive(false); // 隐藏
        varGO.transform.SetParent(this.transform); // 顺便把物体变成我的子物体
        tempList.Add(varGO);
    }

    private GameObject GetPrefab(string name)
    {
        GameObject go;
        if (!prefabPool.TryGetValue(name, out go))
        {
            go = Resources.Load<GameObject>("Prefabs/" + name);
            prefabPool.Add(name, go); // 预设是可以重复使用的, 加载之后存在字典中
        }
        return go;
    }
}
