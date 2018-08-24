using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class GameManager
    {

        private static GameManager instance;

        public static GameManager GetSingle()
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }

        private GameManager()
        {
            allFloor = new Dictionary<int, Floor>();
        }

        private Dictionary<int, Floor> allFloor; // 所有的楼层

        public void NewGame()
        {
            Load("Init"); // 新游戏加载初始存档
        }

        /// <summary>
        /// 加载存档
        /// </summary>
        /// <param name="path"></param>
        private void Load(string path)
        {
            TextAsset[] texts = Resources.LoadAll<TextAsset>("Sava/" + path);
            for (int i = 0; i < texts.Length; i++)
            {
                TextAsset ta = texts[i];
                int[,] map = new int[8, 8];

                int num = int.Parse(ta.name); // 获取楼层信息

                // 获取地图信息
                string[] allLines = ta.text.Split('\n'); 
                for (int j = 0; j < allLines.Length; j++)
                {
                    map[i, j] = int.Parse(allLines[j]);
                }

                Floor floor = new Floor(num, map);
                allFloor.Add(floor.num, floor);  // 地图添加到字典中
            }
        }

        private void Sava()
        {

        }

        /// <summary>
        /// 根据楼层号获取楼层信息
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public Floor GetFloor(int num)
        {
            Floor floor;
            if (allFloor.TryGetValue(num, out floor))
            {
                return floor;
            }
            return null;
        }
    }
}

