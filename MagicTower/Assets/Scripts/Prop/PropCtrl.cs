using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropCtrl : MonoBehaviour {

    public ObjType mType;

    // 被使用的方法
    public virtual void BeUsed()
    {
        // 默认被使用直接存入对象池
        ObjectPool.Instance.SetObj(this.mType, this.gameObject);
    }
}
