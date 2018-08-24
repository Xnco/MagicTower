using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class MapManager : MonoBehaviour
    {
        Dictionary<ObjType, ObjectPool<GameObject>> pool;

        int[,] initMap;

        void Awake()
        {
            pool = new Dictionary<ObjType, ObjectPool<GameObject>>();
            initMap = new int[8, 8];
            InitRoad();
        }

        // 初始化道路
        void InitRoad()
        {
            GameObject road = Resources.Load<GameObject>("Prefabs/Road");

            for (int i = 0; i < initMap.GetLength(0); i++)
            {
                for (int j = 0; j < initMap.GetLength(1); j++)
                {
                    GameObject tmp = Instantiate<GameObject>(road);
                    tmp.name = road.name;
                    tmp.transform.SetParent(this.transform);
                    tmp.transform.localPosition = new Vector3(j, -i, 0);
                    tmp.transform.localScale = Vector3.one * 3.15f;
                }
            }
        }

        void Start()
        {
            GameManager.GetSingle().NewGame(); // 新游戏, 加载游戏存档

            // 加载1楼信息
            Floor floor = GameManager.GetSingle().GetFloor(1);

            for (int i = 0; i < floor.map.GetLength(0); i++)
            {
                for (int j = 0; j < floor.map.GetLength(1); j++)
                {
                    GameObject prefab = Resources.Load<GameObject>("Prefabs/" + ((ObjType)floor.map[i, j]).ToString());
                    GameObject tmp = Instantiate(prefab);
                    tmp.transform.SetParent(this.transform);
                    tmp.transform.localPosition = new Vector3(j, -i, 0);
                    tmp.transform.localScale = Vector3.one * 3.15f;
                }
            }

        }

        void GetObj(int num)
        {
            ObjType type = (ObjType)num;
            ObjectPool<GameObject> tmpPool;
            if (pool.TryGetValue(type, out tmpPool))
            {
                 tmpPool.Get();
            } 
        }
    }
}

