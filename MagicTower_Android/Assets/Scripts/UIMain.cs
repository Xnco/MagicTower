using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoBehaviour {

    Dictionary<string, Transform> allRoads;
    public int curFloor; // 当前所在楼层

	// Use this for initialization
	void Start () {
        //InitRoad();
        GetRoad();
        //InitMapByFloor(curFloor);

        Transform saveBtn = transform.Find("Save");
        saveBtn.GetComponent<Button>().onClick.AddListener(
            ()=> {
                // 保存每个楼层的地图信息
                MapManager.GetSingle().Save();

                // 保存楼层数
                string content = "";
                content += "Floor:" + curFloor + "\n"; // 保存当前的楼层数
                // 主角的信息全部要保存
                Player player = transform.Find("Player").GetComponent<Player>();
                content += "PosX:" + player.pos.x + "\n";
                content += "PosY:" + player.pos.y + "\n";
                content += "HP:" + player.hp;
                File.WriteAllText(Application.dataPath + "/Level/Save/Info.txt", content);
                }
         );

        Transform startBtn = transform.Find("Start");
        startBtn.GetComponent<Button>().onClick.AddListener(
            () =>
            {
                MapManager.GetSingle().isNewGame = true;
                InitMapByFloor(curFloor);
            }
            );

        Transform loadBtn = transform.Find("Load");
        loadBtn.GetComponent<Button>().onClick.AddListener(
            () => {
                MapManager.GetSingle().isNewGame = false; // 改变加载地图的路径
                // 加载当前的楼层
                Dictionary<string, string> tmpInfo = new Dictionary<string, string>();
                string[] contents = File.ReadAllLines(Application.dataPath + "/Level/Save/Info.txt");
                for (int i = 0; i < contents.Length; i++)
                {
                    string[] infos = contents[i].Split(':');
                    tmpInfo.Add(infos[0], infos[1]);
                }

                curFloor = int.Parse(tmpInfo["Floor"]);
                // Player
                int x = int.Parse(tmpInfo["PosX"]);
                int y = int.Parse(tmpInfo["PosY"]);
                Player player = transform.Find("Player").GetComponent<Player>();
                player.pos = new Vector2(x, y);
                player.SetPosition(x, y);

                InitMapByFloor(curFloor);
            }
            );
    }

    // 获取场景中所有的路
    void GetRoad()
    {
        allRoads = new Dictionary<string, Transform>();
        Transform map = this.transform.Find("Map");
        for (int i = 0; i < map.childCount; i++)
        {
            // 遍历所有 Road, 都根据名字存到字典中
            Transform tmpRoad = map.GetChild(i);
            allRoads.Add(tmpRoad.name, tmpRoad);
        }
    }

    // 通过 层数 生成地图
    private void InitMapByFloor(int floor)
    {
        // 得到具体一个楼层的信息
        Dictionary<string, string> floorData = MapManager.GetSingle().GetFloor(floor);
        foreach (var item in floorData)
        {
            // 根据楼层信息, 把所有物体都放到对应的地板中
            Transform tmpRoad = GetRoadByName(item.Key);
            if (tmpRoad != null)
            {
                GameObject tmpObj = ObjectPool.GetSingle().GetObj(item.Value); // 直接到对象池中取物体
                tmpObj.transform.SetParent(tmpRoad);
                tmpObj.transform.localPosition = Vector3.zero;
            }
        }
    }

    // 从当前楼层字典中移除物体
    public void  RemoveObj(string roadName)
    {
        RemoveObj(curFloor, roadName);
    }

    // 移除具体某个楼层的物体
    public void RemoveObj(int floor, string roadName)
    {
        // 根据楼层数, 获取楼层字典
        Dictionary<string, string> floorData = MapManager.GetSingle().GetFloor(floor);
        if (floorData != null)
        {
            // 从数据层面移除物体, 下次加载楼层的时候, 就不会重复加载了
            floorData.Remove(roadName);
        }
    }

    public void ChangedFloor(int offset)
    {
        // 上楼前, 要将1楼的物体, 全部清空
        MTBase[] objs = GetComponentsInChildren<MTBase>();
        foreach (var item in objs)
        {
            // 只是单纯得将物体都存到对象池, 实际上物体没有被拾取
            item.SaveObj();
        }

        // 再加载二楼的物体
        curFloor += offset;
        InitMapByFloor(curFloor);
    }

    // 通过坐标获取路
    public Transform GetRoadByPos(float x, float y)
    {
        string name = x + "_" + y;
        return GetRoadByName(name);
    }

    // 通过名字获取路
    public Transform GetRoadByName(string name)
    {
        if (allRoads.ContainsKey(name))
        {
            return allRoads[name];
        }
        return null;
    }

	// Update is called once per frame
	void Update () {
		
	}

    void InitRoad()
    {
        Transform roadPrefab = transform.Find("Map/Road");
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                GameObject tmpRoad = Instantiate(roadPrefab.gameObject);
                tmpRoad.name = i + "_" + j;
                tmpRoad.SetActive(true);
                tmpRoad.transform.SetParent(roadPrefab.parent);
            }
        }
    }
}
