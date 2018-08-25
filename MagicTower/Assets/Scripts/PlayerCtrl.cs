using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

    // 记录主角在 Map数组 中的坐标
    public int x;
    public int y;

    bool canMove;

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

    bool CanMove(int x, int y)
    {
        int result = MapManager.GetSingle().GetMapPoint(x, y);
        if (result == 0) // 空地 - 直接移动
        {
            return true;
        }
        else if (result >= 20 && result < 30)
        {
            // 捡起钥匙 - 1将钥匙从地图中清除, 20->0
            //           2. 将具体的物体隐藏
            MapManager.GetSingle().PickUpProp(x, y);
            return true;
        }
        else if (result >= 10 && result < 20)
        {
            // 门 -> 有没有对应的钥匙
            return MapManager.GetSingle().OpenDoor(x, y);
        }

        return false;
    }

    IEnumerator MoveCD()
    {
        canMove = false; // 移动一次之后不能移动了
        yield return new WaitForSeconds(0.2f);
        canMove = true;  // 一定时间后又可以移动了
    }
}
