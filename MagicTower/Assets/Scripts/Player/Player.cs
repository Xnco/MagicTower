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

    private float hp;
    private float mp;
    private float atk;
    private float def;
    private float gold;
    private float exp;
    private float level;

    public float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
        }
    }

    public float Mp
    {
        get
        {
            return mp;
        }

        set
        {
            mp = value;
        }
    }

    public float Atk
    {
        get
        {
            return atk;
        }

        set
        {
            atk = value;
        }
    }

    public float Def
    {
        get
        {
            return def;
        }

        set
        {
            def = value;
        }
    }

    public float Gold
    {
        get
        {
            return gold;
        }

        set
        {
            gold = value;
        }
    }

    public float Exp
    {
        get
        {
            return exp;
        }

        set
        {
            exp = value;
        }
    }

    public float Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    private Player()
    {
        allPropNum = new Dictionary<int, int>();
    }

    // 添加道具
    public void AddProp(int num)
    {
        // 
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
