using System.IO; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    private static MapManager instance;

    public static MapManager GetSingle()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("MapManager");
            instance = go.AddComponent<MapManager>();
            DontDestroyOnLoad(go);
        }
        return instance;
    }

    Dictionary<int, Dictionary<string, string>> allFloors;

    string savePath;
    string newPath;

    public bool isNewGame;

    private void Awake()
    {
        allFloors = new Dictionary<int, Dictionary<string, string>>();

        //Debug.LogError(Application.persistentDataPath);

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        newPath = "file://" + Application.streamingAssetsPath + "/Level/New/{0}.txt";
        savePath = Application.streamingAssetsPath + "/Level/Save/{0}.txt";
#elif UNITY_ANDROID
        //newPath = Application.streamingAssetsPath + "/Level/New/{0}.txt";
        //savePath =  Application.streamingAssetsPath + "/Level/Save/{0}.txt";
#endif
    }

    // 通过楼层数获取楼层信息
    public Dictionary<string, string> GetFloor(int floor)
    {
        if (!allFloors.ContainsKey(floor))
        {
            //LoadFloor(floor); // 这个楼层没加载过 
            StartCoroutine(LoadFloor(floor));
        }
        return allFloors[floor];
    }

    // 根据楼层数, 在文本路径中加载一个楼层文本, 将信息解析出来存到 allFloors 中
    //private void LoadFloor(int floor)
    private IEnumerator LoadFloor(int floor)
    {
        // tmpDic 存了某一个楼层的信息
        Dictionary<string, string> tmpDic = new Dictionary<string, string>();
        string newPath = string.Format(this.newPath, floor);
        string savePath = string.Format(this.savePath, floor);
        string path;
        //if (isNewGame || !File.Exists(savePath))
        if (isNewGame)
        {
            path = newPath;
        }
        else
        {
            path = savePath;
        }
        //Debug.LogError(path);
        //if (File.Exists(path))
        //{
            WWW www = new WWW(path);
            yield return www;
            string[] allLines = www.text.Split('\n');
            for (int i = 0; i < allLines.Length; i++)
            {
                string[] singleObj = allLines[i].Split(':'); // 根据 : 切割 
                tmpDic.Add(singleObj[0], singleObj[1]); // 将 地板-物体  的信息存到字典中
            }
            allFloors.Add(floor, tmpDic); // 再将 string,string 的字典存到 allFloors
        //}
        //else
        //{
        //    Debug.LogError("不能加载第" + floor + "层的文本, 请检查: " + path);
        //    //return;
        //}
    }

    public void Save()
    {
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
}
