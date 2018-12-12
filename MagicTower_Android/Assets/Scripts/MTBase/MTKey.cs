using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTKey : MTBase {
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override bool Through(Player player)
    {
        // 去掉末尾最后三个字符
        //string tmp = this.name.Substring(0, this.name.Length - 3);
        // 用 "" 把 "Key" 替换掉
        string tmp = this.name.Replace("Key", "");
        player.ChangedKey(tmp, 1);
        RemoveObj(); // 当我被拾取的时候, 移除自己
        return true;
    }
}
