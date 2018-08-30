using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtrotyProp : PropCtrl {

    public int hp;
    public int mp;
    public int exp;
    public int lv;
    public int gold;

    public override void BePickUp()
    {
        Player.Instance.Hp += hp;
        Player.Instance.Mp += mp;
        Player.Instance.Exp += exp;
        Player.Instance.Level += lv;
        Player.Instance.Gold += gold;

        base.BePickUp();
    }
}
