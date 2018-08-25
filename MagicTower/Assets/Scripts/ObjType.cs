using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjType
{
    None = 0,
    //Player = 1,

    // 2 - 9 墙等
    Wall = 2,
    Hot,

    // 门/钥匙 10 - 20
    Door_Yellow = 10,
    Key_Yellow,

    Door_Blue = 12,
    Key_Blue,

    // 直接使用的道具 50 - 100


    // 武器 100+
    Knife1 = 102,
    Knife2 = 103,

    // 防具200+


}
