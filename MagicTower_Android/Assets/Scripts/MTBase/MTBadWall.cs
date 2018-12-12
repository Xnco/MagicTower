using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTBadWall : MTBase {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override bool Through(Player player)
    {
        RemoveObj();
        return true;
    }
}
