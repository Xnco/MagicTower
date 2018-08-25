using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPProp : PropCtrl {

    public int hp;

    public override void BeUsed()
    {
        Player.Instance.Hp += hp;
        base.BeUsed();
    }
}
