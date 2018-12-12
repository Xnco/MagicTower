using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTDoor : MTBase {

    public float animationTime;
    Animator mAn;
    bool isOpen;

    // Use this for initialization
    void Start () {
        mAn = GetComponent<Animator>();
        isOpen = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override bool Through(Player player)
    {
        if (isOpen) // 如果已经开过了, 就不能重复开
        {
            return false;
        }

        string tmp = this.name.Substring(0, this.name.Length - 4);
        if (player.GetKey(tmp) > 0)
        {
            // 开门动画
            mAn.SetTrigger("Trigger");
            player.ChangedKey(tmp, -1);
            isOpen = true;
            StartCoroutine(SaveObj(animationTime));
            return false;
        }
        return false;
    }

    public override void ResetObj()
    {
        isOpen = false;
    }
}
