using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

    // 记录主角在 Map数组 中的坐标
    public int x;
    public int y;
    public bool canMove;

	// Use this for initialization
	void Start () {
        // 初始化玩家的位置
        this.transform.localPosition = new Vector3(y, -x);
        canMove = true;
    }
	
	// Update is called once per frame
	void Update () {
        Move();
    }

    void Move()
    {
        if (!canMove)
        {
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (CanMove(x - 1, y))
            {
                transform.Translate(0, 1, 0);
                x--; // 移动之后再更新主角坐标
               
                StartCoroutine(MoveCD());
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (CanMove(x + 1, y))
            {
                transform.Translate(0, -1, 0);
                x++; // 移动之后再更新主角坐标
                StartCoroutine(MoveCD());
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (CanMove(x, y-1))
            {
                transform.Translate(-1, 0, 0);
                y--; // 移动之后再更新主角坐标
                StartCoroutine(MoveCD());
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (CanMove(x, y + 1))
            {
                transform.Translate(1, 0, 0);
                y++; // 移动之后再更新主角坐标

                StartCoroutine(MoveCD());
            }
        }
    }

    // 判断能否移动的方法，返回true移动，返回false不能移动
    bool CanMove(int x, int y)
    {
        int result = MapManager.GetSingle().GetMapPoint(x, y);
        Debug.Log("这边是" + result);
        // 越界
        if (result == -1) 
        {
            return false;
        }
        // 空地 - 直接移动
        else if (result == 0) 
        {
            return true;
        }
        // 钥匙或墙
        else if (result >= 10 && result < 20 )
        {
            // 拾取钥匙
            if (result % 2 == 1)
            {
                // 从地图上拾取道具
                MapManager.GetSingle().PickUpProp(x, y);
                Player.Instance.AddProp(result);  // 玩家添加道具
                return true;
            }
            // 开门 - 需要对应钥匙
            else
            {
                // 先判断玩家有没有钥匙
                if (Player.Instance.GetPropNum(result + 1) > 0)
                {
                    Player.Instance.UserProp(result + 1); // 使用一个道具
                    MapManager.GetSingle().PickUpProp(x, y);
                    StartCoroutine(MoveCD(0.3f));
                }
                return false; // 开门不能走过去
            }
        }
        // 直接使用的道具
        else if (result >= 50 && result < 100)
        {
            MapManager.GetSingle().PickUpProp(x, y);
        }
        return false;
    }

    IEnumerator MoveCD(float time = 0.1f)
    {
        canMove = false; // 移动一次之后不能移动了
        yield return new WaitForSeconds(time);
        canMove = true;  // 一定时间后又可以移动了
    }
}
