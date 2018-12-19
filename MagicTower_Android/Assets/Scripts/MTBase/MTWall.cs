using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTWall : MTBase {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override bool Through(Player player)
    {
        //throw new System.NotImplementedException();
        AudioManager.GetSingle().Play("1");
        return false;
    }
}
