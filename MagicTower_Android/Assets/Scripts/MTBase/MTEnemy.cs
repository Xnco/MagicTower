using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTEnemy : MTBase {


    public float hp;
    public float atk;
    public float def;

    // Use this for initialization
    void Start () {
		
	}

    public override bool Through(Player player)
    {
        // 战斗 - 主角先攻击

        float  playerDam = GetDam(player.atk, this.def);
        float enemyDam = GetDam(this.atk, player.def);
        int atkNum = 0;
        // 敌人的攻击次数, 在敌人被打死之前能攻击的次数
        if (this.hp % playerDam == 0)
        {
            atkNum = (int)(this.hp / playerDam) - 1;
        }
        else
        {
            atkNum = (int)(this.hp / playerDam);
        }
       
        // 可以对主角造成的总伤害
        float damSum = atkNum * enemyDam;
        if (player.hp > damSum)
        {
            // 主角胜利
            player.hp -= damSum; // 主角掉血
            RemoveObj(); // 自己死亡
            return true;
        }
        else
        {
            // 敌人胜利
            return false;
        }
    }

    // 最低伤害为 1
    float GetDam(float atk, float def)
    {
        if (def >= atk)
        {
            return 1;
        }
        return atk - def;
    }
}
