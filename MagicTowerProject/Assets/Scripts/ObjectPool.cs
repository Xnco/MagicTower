using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public delegate T OnCreateObject<T>();

    public delegate void OnPutObject<T>(T obj);

    public class ObjectPool<T> where T : class
    {
        public OnCreateObject<T> onCreateFun;
        private OnPutObject<T> onPutFun;
        private Queue<T> Pool;//对象池
        public ObjectPool(OnCreateObject<T> createFun = null, OnPutObject<T> putFun = null)
        {
            Pool = new Queue<T>();
            onCreateFun = createFun;
            onPutFun = putFun;
        }
        //放入对象池
        public void Put(T item)
        {
            if (onPutFun != null)
            {
                onPutFun(item);//对象的初始化方法
            }
            Pool.Enqueue(item);//加入对象池
        }
        //从对象池中获取一个对象
        public T Get()
        {
            if (Pool.Count > 0)
            {
                return Pool.Dequeue();//出队
            }
            if (onCreateFun != null)
            {
                return onCreateFun();//创建一个对象
            }
            return default(T);//返回T类型的默认值
        }
    }
}
