using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour {

    public float hp;
    public float atk;
    public float def;
    public float gold;
    public float exp;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void BeAttack(float varAtk)
    {
        hp -= varAtk;
    }
}
