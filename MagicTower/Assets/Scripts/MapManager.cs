using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public delegate void IntDelegate(int x, int y);

/// <summary>
/// 负责数据的加载 - 储存
/// </summary>
public class MapManager
{
    private static MapManager instance;
    public static MapManager GetSingle()
    {
        if (instance == null)
        {
            instance = new MapManager();
        }
        return instance;
    }

    private Dictionary<int, int[,]> allMap; // 所有楼层
   

    public int curFloor; // 主角当前所在的楼层
    public int[,] curMap; // 当前所在楼层具体的 Map

    public event IntDelegate onPickUpEvent; // 使用道具的事件 

    private MapManager()
    {
        allMap = new Dictionary<int, int[,]>();
    }

    // 读取所有地图 - txt xml json sql ...
    public void LoadAllMap()
    {
        TextAsset[] allMapText = Resources.LoadAll<TextAsset>("Sava/Init/Map"); // 读取 Map 下的所有文本
        for (int i = 0; i < allMapText.Length; i++)
        {
            TextAsset tmpText = allMapText[i];
            int[,] map = LoadMap(tmpText);
            allMap.Add(int.Parse(tmpText.name), map); // 根据楼层号 将 具体的 8*8 的地图存到字典中
        }

        Debug.Log("所有地图读取完成");
    }

    // 从文本中读取地图 - 只负责解析文本
    private int[,] LoadMap(TextAsset loadAsset)
    {
        int[,] map = new int[8, 8];

        string num = loadAsset.name; // 获取文本的名字
        string allText = loadAsset.text;  // 获取文本的内容

        string[] allLines = allText.Split('\n');
        for (int i = 0; i < allLines.Length; i++)
        {
            string singleLine = allLines[i]; // 单独取出一行

            string[] words = singleLine.Split(',');
            for (int j = 0; j < words.Length; j++)
            {
                int word = int.Parse(words[j].Trim());  // 得到具体的某个数字 - Trim() 删除两端空格

                map[i, j] = word; // 将文本内容解析存到一个 8*8 的int数组中
            }
        }
        return map;
    }

    // 读取某一层的地图
    public int[,] GetMap(int num)
    {
        int[,] map;
        if (allMap.TryGetValue(num, out map))
        {
            // 读取地图的时候 顺便记录下当前的层数和当前的地图
            curFloor = num;
            curMap = map;
            return map;
        }

        Debug.LogError("读取第"+ num +"层地图失败");
        return null;
    }

    // 获取地图上某一个点的信息
    public int GetMapPoint(int x, int y)
    {
        if (x < 0 || x >= curMap.GetLength(0) ||
            y < 0 || y >= curMap.GetLength(1))
        {
            return -1;
        }
        return curMap[x, y]; 
    }

    // 拾取道具
    public void PickUpProp(int x, int y)
    {
        // 从数据层将道具拾取了 - 拾取到道具, 根据道具编号将信息存到字典中
        int oldNum = curMap[x, y]; // 道具的编号
        curMap[x, y] = 0;
        
        // 表现层将道具隐藏
        if (onPickUpEvent != null)
        {
            onPickUpEvent(x, y);
        }
    }
}
