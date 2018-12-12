using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTGoUpstairs : MTBase {

    public int offset;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override bool Through(Player player)
    {
        // 切换楼层
        // UIMain
        GetComponentInParent<UIMain>().ChangedFloor(offset);

        return true;
    }
}
