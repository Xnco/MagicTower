using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool {

    private static ObjectPool instance;
    public static  ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }

    // 所有预设
    Dictionary<ObjType, GameObject> allPrefab;
    // 对象池
    Dictionary<ObjType, Stack<GameObject>> pool;

    private ObjectPool()
    {
        allPrefab = new Dictionary<ObjType, GameObject>();
        pool = new Dictionary<ObjType, Stack<GameObject>>();
    }

    public GameObject GetObj(ObjType varType)
    {
        Stack<GameObject> tmpStack;
        if (pool.TryGetValue(varType, out tmpStack))
        {
            if (tmpStack.Count > 0)
            {
                var tmpObject = tmpStack.Pop();
                tmpObject.gameObject.SetActive(true);
                return tmpObject;
            }
        }

        // 获取预设克隆一个
        var tmpPrefab = LoadPrefab(varType);
        var tmpObj = GameObject.Instantiate(tmpPrefab);
        tmpObj.name = tmpPrefab.name;
        return tmpObj;
    }

    private GameObject LoadPrefab(ObjType varType)
    {
        GameObject tmpPrefab;
        if (!allPrefab.TryGetValue(varType, out tmpPrefab))
        {
            tmpPrefab = Resources.Load<GameObject>("Prefabs/" + varType.ToString());
        }
        return tmpPrefab;
    }

    public void SetObj(ObjType varType, GameObject varObj)
    {
        varObj.SetActive(false);
        Stack<GameObject> tmpStack;
        if (!pool.TryGetValue(varType, out tmpStack))
        {
            tmpStack = new Stack<GameObject>();
            pool.Add(varType, tmpStack);
        }
        tmpStack.Push(varObj);
    }

}
