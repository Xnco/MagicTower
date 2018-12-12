using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 魔塔道具的基类
public abstract class MTBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 返回值为 true, 代表可以穿过, 返回值为 false 代表, 不能穿过
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public abstract bool Through(Player player);

    /// <summary>
    /// 单纯地存到对象池中, 切换楼层的时候使用
    /// </summary>
    public virtual void SaveObj()
    {
        ObjectPool.GetSingle().SaveObj(this.gameObject);
    }

    /// <summary>
    /// 直接将自己存到对象池中 - 从字典中移除自己 - 我被拾取
    /// </summary>
    public virtual void RemoveObj()
    {
        // 从当前楼层的字典中移除
        GetComponentInParent<UIMain>().RemoveObj(this.transform.parent.name);
        ObjectPool.GetSingle().SaveObj(this.gameObject);
    }

    /// <summary>
    /// 等待 time 秒, 再讲自己存到对象池中
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    protected IEnumerator SaveObj(float time)
    {
        yield return new WaitForSeconds(time);
        RemoveObj();
    }

    // 重置物体
    public virtual void ResetObj() { }
}
