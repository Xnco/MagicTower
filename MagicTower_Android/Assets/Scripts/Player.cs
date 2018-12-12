using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private UIMain main;
    // 主角所在 map 中的位置
    public Vector2 pos;

    public float hp;
    public float atk;
    public float def;

    private Dictionary<string, int> allKeyNum;

    bool canMove;
    public float moveCD;

    // Use this for initialization
    void Start () {
        allKeyNum = new Dictionary<string, int>();
        main = GetComponentInParent<UIMain>();
        canMove = true;
    }
	
	// Update is called once per frame
	void Update () {
        InputKey();
    }

    private void InputKey()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Move(-1, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Move(1, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Move(0, -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Move(0, 1);
        }
    }

    // 移动
    void Move(int x_offset, int y_offset)
    {
        Transform road = main.GetRoadByPos(pos.x + x_offset, pos.y + y_offset);
        if (road == null || !canMove)
        {
            return;
        }
        if (road.childCount == 0)
        {
            // 移动的方向是空的路
            SetPosition(pos.x + x_offset, pos.y + y_offset);
        }
        else
        {
            // 有路, 但路上有某些东西
            Transform obj = road.GetChild(0);
            if (obj.GetComponent<MTBase>().Through(this))
            {
                // 如果返回true, 就更新坐标
                SetPosition(pos.x + x_offset, pos.y + y_offset);
            }
        }
    }

    // 设置坐标
    public void SetPosition(float x, float y)
    {
        Transform road = main.GetRoadByPos(x,  y);
        canMove = false;
        StartCoroutine(MoveCD());
        this.transform.position = road.position; // 更新物体的坐标
        pos = new Vector2(x, y); // 更新在地图上的坐标
    }

    IEnumerator MoveCD()
    {
        yield return new WaitForSeconds(moveCD);
        canMove = true;
    }

    // 添加钥匙 - Yellow
    public void ChangedKey(string name, int num)
    {
        if (allKeyNum.ContainsKey(name))
        {
            allKeyNum[name] += num;
        }
        else
        {
            allKeyNum.Add(name, num);
        }
    }

    // Yellow
    public int GetKey(string name)
    {
        if (allKeyNum.ContainsKey(name))
        {
            return allKeyNum[name];
        }
        return 0;
    }
}
