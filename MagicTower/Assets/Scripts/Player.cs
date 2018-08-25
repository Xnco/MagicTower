using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private static Player instance;
     
    public static Player Instance
    {
        get {
            if (instance == null)
            {
                instance = new Player();
            }
            return instance;
        } 
    }

    private Dictionary<int, int> allPropNum; // 只针对道具的数量

    private Player()
    {
        allPropNum = new Dictionary<int, int>();
    }

    // 添加道具
    public void AddProp(int num)
    {
        if (allPropNum.ContainsKey(num))
        {
            allPropNum[num]++; // 道具之前拾取过, 数量++
        }
        else
        {
            allPropNum.Add(num, 1); // 道具之前没拾取过, 新建一个键值对
        }
    }

    // 获取玩家某个道具的数量
    public int GetPropNum(int num)
    {
        if (allPropNum.ContainsKey(num))
        {
            return allPropNum[num];
        }
        return 0;
    }

    // 使用道具
    public void UserProp(int num)
    {
        if (allPropNum.ContainsKey(num))
        {
            allPropNum[num]--;
        }
    }
     
}
