using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 表现层 - 根据数据做一些具体的事情(克隆, 生成, 改UI....)
/// </summary>
public class MapCtrl : MonoBehaviour
{

    Dictionary<ObjType, GameObject> allPrefabs; // 所有的预设
    GameObject[,] mapObjs; // 地图上所有的物体

    public int floor; // 初始的楼层

    // Use this for initialization
    void Start()
    {
        // 注册事件
        MapManager.GetSingle().onPickUpEvent += PickUpProp;
        MapManager.GetSingle().onOpenDoorEvent += OpenDoor;

        mapObjs = new GameObject[8, 8];

        // 初始化
        LoadPrefab(); // 加载预设
        MapManager.GetSingle().LoadAllMap(); // 加载地图信息

        // 先生成背景(路)
        //CloneRoad();
        // 根据地图信息生成某一层楼
        DrawMap(floor);
    }

    // 加载预设
    void LoadPrefab()
    {
        allPrefabs = new Dictionary<ObjType, GameObject>();
        ObjType[] allValue = (ObjType[])Enum.GetValues(ObjType.None.GetType());
        // 根据枚举加载所有的预设
        for (int i = 0; i < allValue.Length; i++)
        {
            GameObject tmp = Resources.Load<GameObject>("Prefabs/" + allValue[i].ToString());
            if (tmp != null)
            {
                allPrefabs.Add(allValue[i], tmp);
            }
            else
            {
                Debug.LogError(allValue[i].ToString() + " 加载失败");
            }
        }
    }

    // 生成背景(全是路)
    void CloneRoad()
    {
        GameObject roadPrefab = Resources.Load<GameObject>("Prefabs/Road");
        // 生成 8*8 的路
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var tmpRoad = Instantiate(roadPrefab);
                tmpRoad.transform.SetParent(this.transform);
                tmpRoad.transform.localScale = Vector3.one * 3.15f;
                tmpRoad.transform.localPosition = new Vector3(j, -i, 0);
            }
        }
    }

    // 根据地图的二维数组生成某一层的地图
    void DrawMap(int num)
    {
        int[,] map = MapManager.GetSingle().GetMap(num);
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                GameObject tmpObj;
                if (allPrefabs.TryGetValue((ObjType)map[i, j], out tmpObj))
                {
                    GameObject tmp = Instantiate(tmpObj);
                    tmp.name = tmpObj.name;
                    tmp.transform.SetParent(this.transform);
                    tmp.transform.localScale = Vector3.one * 3.15f;
                    tmp.transform.localPosition = new Vector3(j, -i, 0);

                    mapObjs[i, j] = tmp; // 生成的同时顺便储存
                }
            }
        }
    }

    private void PickUpProp(int x, int y)
    {
        // 如何根据坐标将道具获取
        if (mapObjs[x, y] != null)
        {
            mapObjs[x, y].SetActive(false);
        }
    }

    private void OpenDoor(int x, int y)
    {
        if (mapObjs[x, y] != null)
        {
            mapObjs[x, y].SetActive(false);
        }
    }

    private void OnDestroy()
    {
        MapManager.GetSingle().onPickUpEvent -= PickUpProp;
        MapManager.GetSingle().onOpenDoorEvent -= OpenDoor;
    }
}
