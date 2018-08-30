using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorProp : PropCtrl {

    Animator mAn;

    public string triggerName = "Play";
    public float animationTime = 0.3f;

	// Use this for initialization
	void Start () {
        mAn = GetComponentInChildren<Animator>();
    }

    public override void BePickUp()
    {
        //base.BeUsed();
        StartCoroutine(PlayAnimation());
    }
    
    IEnumerator PlayAnimation()
    {
        mAn.SetTrigger(triggerName);
        yield return new WaitForSeconds(animationTime);
        base.BePickUp();
    }
}
