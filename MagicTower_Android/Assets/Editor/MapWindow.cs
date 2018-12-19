using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapWindow : EditorWindow {

    public static string oldString;
    public static string newString;

    public static int floor;

    [MenuItem("关卡工具/打开工具窗口")]
	public static void OpenWindow()
    {
        MapWindow window = GetWindow<MapWindow>();
        window.titleContent = new GUIContent("关卡编辑器");
    }

    private void OnGUI()
    {
        if (Selection.activeGameObject != null && Selection.activeGameObject.name == "Map")
        {
            EditorGUILayout.LabelField("地图编辑功能");
            floor = EditorGUILayout.IntField(floor); // 层数
            if (File.Exists(Application.streamingAssetsPath + "/Level/New/" + floor + ".txt"))
            {
                GUI.contentColor = Color.red;
                if (GUILayout.Button("地图文件已存在, 替换地图 (慎点)"))
                {
                    CreateFileByMap();
                }

                GUI.contentColor = Color.black;

                if (GUILayout.Button("加载地图文件"))
                {
                    ReadFile();
                }
            }
            else
            {
                if (GUILayout.Button("生成地图文件"))
                {
                    CreateFileByMap(); 
                }
            }
            
         
            if (GUILayout.Button("清空地图"))
            {
                ClearMap();
            }
        }
        else
        {
            EditorGUILayout.LabelField("请选中 Map物体 再编辑关卡"); // 显示一行文本
        }

        EditorGUILayout.LabelField("替换预设名字"); // 显示一行文本

        oldString = EditorGUILayout.TextField(oldString);
        newString = EditorGUILayout.TextField(newString);

        if (GUILayout.Button("改名"))
        {
            RenamePrefab();
        }
    }

    public static void RenamePrefab()
    {
        // 让 New 中的所有文本都改

        // 获取 Level 这个文件夹下的所有文件夹
        string[] allDir = Directory.GetDirectories(Application.streamingAssetsPath + "/Level/");
        foreach (var item in allDir)
        {
            //读取该文件夹下的所有 *.txt 的文件
            string[] allTextFiles = Directory.GetFiles(item, "*.txt");

            for (int i = 0; i < allTextFiles.Length; i++)
            {
                RenameOneFile(allTextFiles[i]);
            }
        }
    }

    // 对一个txt文件做 oldstring替换成 newstring
    static void RenameOneFile(string path)
    {
        string[] allLines = File.ReadAllLines(path);

        string content = "";
        foreach (var line in allLines)
        {
            string[] temp = line.Split(':');
            if (temp[1] == oldString)
            {
                temp[1] = newString;
            }
            content += temp[0] + ":" + temp[1] + "\n";
        }

        File.WriteAllText(path, content);
    }

    //[MenuItem("关卡工具/生成地图文件")]
    public static void CreateFileByMap()
    {
        // 根据地图(Map)中摆放的场景生成一个文件
        GameObject map = Selection.activeGameObject;

        string text = "";
        // 遍历所有的子物体
        for (int i = 0; i < map.transform.childCount; i++)
        {
            Transform tmpRoad = map.transform.GetChild(i);
            if (tmpRoad.childCount != 0)
            {
                // 路(Road) 中有其他物体 - 将这个物体记录

                string content = tmpRoad.name + ":" + tmpRoad.GetChild(0).name;
                text += content + "\n";
            }
        }

        string path = Application.streamingAssetsPath + "/Level/New/" + floor + ".txt";
        File.WriteAllText(path, text);
    }

    //[MenuItem("关卡工具/加载地图文件")]
    public static void ReadFile()
    {
        GameObject map = Selection.activeGameObject;
        // 读取一个本地文件 - 根据本地文件的 RoadName:ObjName
        string[] allLines = File.ReadAllLines(Application.streamingAssetsPath + "/Level/New/" + floor + ".txt");
        for (int i = 0; i < allLines.Length; i++)
        {
            string[] singleObj = allLines[i].Split(':'); // 根据 : 切割 
            Transform road = map.transform.Find(singleObj[0]); // 子物体(Road)的名字

            GameObject objPrefab = Resources.Load<GameObject>("Prefabs/" + singleObj[1]);// Road中的物体名字
            GameObject tmpObj = GameObject.Instantiate(objPrefab);
            tmpObj.name = objPrefab.name; // 去掉 (clone)
            tmpObj.transform.SetParent(road);
            tmpObj.transform.localPosition = Vector3.zero;
        }
    }

    //[MenuItem("关卡工具/清空地图")]
    public static void ClearMap()
    {
        GameObject map = Selection.activeGameObject;

        // 遍历所有的子物体 - 所有子物体都销毁
        for (int i = 0; i < map.transform.childCount; i++)
        {
            Transform tmpRoad = map.transform.GetChild(i);
            if (tmpRoad.childCount != 0)
            {
                GameObject.DestroyImmediate(tmpRoad.GetChild(0).gameObject);
            }
        }
    }

}
