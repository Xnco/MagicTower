using System.IO; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour{

    private static MapManager instance;

    public static MapManager GetSingle()
    {
        if (instance == null)
        {
            GameObject tmp = new GameObject("MapManager");
            instance = tmp.AddComponent<MapManager>();
            DontDestroyOnLoad(tmp);
        }
        return instance;
    }

    Dictionary<int, Dictionary<string, string>> allFloors;

    string saveDir;
    string savePath;
    string newPath; 

    public bool isNewGame;

    private void Awake()
    {
        StartCoroutine(LoadStreamingAssets());

        allFloors = new Dictionary<int, Dictionary<string, string>>();
        newPath = Application.persistentDataPath + "/Level/New/{0}.txt";

        saveDir = Application.persistentDataPath + "/Level/Save";
        savePath = saveDir + "/{0}.txt";
    }

    // 转移数据到 persistent 中 -> 转移之后, 所有的正常读写都在 persistent 中
    public IEnumerator LoadStreamingAssets()
    {
        string verPath = Application.persistentDataPath + "/Version.txt";
        // 版本号不存在 - 第一次打开 || 版本号存在, 但是版本不同
        if (!File.Exists(verPath) || File.ReadAllText(verPath) != AppConst.Version)
        {
            #region 写入数据 和 版本号
            // 加载楼层地图信息
            string initPath = Application.streamingAssetsPath + "/Level/New/{0}.txt";
            for (int i = 1; i <= AppConst.FloorNumber; i++)
            {
                string streamingAssets = string.Format(initPath, i);

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                streamingAssets = "file:///" + streamingAssets;
#endif

                WWW www = new WWW(streamingAssets);
                yield return www;

                // 写入到 persistentDataPath

                string targetDir = Application.persistentDataPath + "/Level/New";
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }
                string targetPath = targetDir + "/" + i + ".txt";

                Debug.Log(targetPath);
                File.WriteAllText(targetPath, www.text);
            }

            // 写入版本号
            File.WriteAllText(verPath, AppConst.Version);
            #endregion
        }
        else
        {
            Debug.Log("版本一致, 不需要更新数据");
        }
    }

    // 通过楼层数获取楼层信息
    public Dictionary<string, string> GetFloor(int floor)
    {
        if (!allFloors.ContainsKey(floor))
        {
             LoadFloor(floor); // 这个楼层没加载过 
        }
        return allFloors[floor];
    }

    // 根据楼层数, 在文本路径中加载一个楼层文本, 将信息解析出来存到 allFloors 中
    private void LoadFloor(int floor)
    {
        // tmpDic 存了某一个楼层的信息
        Dictionary<string, string> tmpDic = new Dictionary<string, string>();
        string newPath = string.Format(this.newPath, floor);
        string savePath = string.Format(this.savePath, floor);
        string path;
        if (isNewGame || !File.Exists(savePath))
        {
            path = newPath;
        }
        else
        {
            path = savePath;
        }

        if (!File.Exists(path))
        {
            Debug.LogError("不能加载第" + floor + "层的文本, 请检查: " + path);
            return;
        }
        string[] allLines = File.ReadAllLines(path);
        for (int i = 0; i < allLines.Length; i++)
        {
            string[] singleObj = allLines[i].Split(':'); // 根据 : 切割 
            tmpDic.Add(singleObj[0], singleObj[1]); // 将 地板-物体  的信息存到字典中
        }
        allFloors.Add(floor, tmpDic); // 再将 string,string 的字典存到 allFloors
    }

    // 存档
    public void Save()
    {
        if (!Directory.Exists(saveDir))
        {
            Directory.CreateDirectory(saveDir);
        }

        // 保存所有楼层的信息
        foreach (var item in allFloors)
        {
            string path = string.Format(savePath, item.Key);
            string content = "";
            foreach (var info in item.Value)
            {
                string single = info.Key + ":" + info.Value + "\n";
                content += single;
            }
            File.WriteAllText(path, content);
        }
    }

    public void SavePlayerAndFloor(int floor, Player player)
    {
        string content = "";
        content += "Floor:" + floor + "\n"; // 保存当前的楼层数
        content += "PosX:" + player.pos.x + "\n";
        content += "PosY:" + player.pos.y + "\n";
        content += "HP:" + player.hp;
        File.WriteAllText(saveDir + "/Info.txt", content);
    }

    public int LoadPlayerAndFloor(Player player)
    {
        Dictionary<string, string> tmpInfo = new Dictionary<string, string>();
        string[] contents = File.ReadAllLines(saveDir + "/Info.txt");
        for (int i = 0; i < contents.Length; i++)
        {
            string[] infos = contents[i].Split(':');
            tmpInfo.Add(infos[0], infos[1]);
        }

        int x = int.Parse(tmpInfo["PosX"]);
        int y = int.Parse(tmpInfo["PosY"]);
        player.pos = new Vector2(x, y);
        player.SetPosition(x, y);

        int floor = int.Parse(tmpInfo["Floor"]);
        return floor;
    }
}
