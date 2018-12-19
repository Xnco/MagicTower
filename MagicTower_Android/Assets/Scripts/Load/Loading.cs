using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    Slider slider;

    // Use this for initialization
    void Start()
    {
        MapManager.GetSingle();

        //Debug.Log(Application.streamingAssetsPath);
        //Debug.Log(Application.persistentDataPath); 
        // 同步加载
        //File.ReadAllBytes();
        //GameObject go = Resources.Load<GameObject>("Prefab/Wall1");
        //SceneManager.LoadScene();
        slider = transform.Find("Slider").GetComponent<Slider>();
        StartCoroutine(LoadTexture());

    }

    IEnumerator LoadTexture()
    {
        string[] allNeedLoad = { };
        for (int i = 0; i < allNeedLoad.Length; i++)
        {
            ResourceRequest rr = Resources.LoadAsync("Prefab/Wall1");
            yield return rr;
            slider.value += 0.5f / allNeedLoad.Length; // 每次加载完一个, 就手动增加一点进度
            GameObject go1 = rr.asset as GameObject;
            Instantiate(go1);
        }

        slider.value = 0.5f;
        //yield return new WaitForSeconds(2);

        AsyncOperation ao = SceneManager.LoadSceneAsync("Main");
        while (!ao.isDone)
        {
            yield return null;
            slider.value = 0.5f + ao.progress / 2; // 每帧更新一次加载的进度
        }

        // 多用于加载网络资源 - 一定是异步的
        //WWW www = new WWW(@"http://img31.mtime.cn/mg/2014/11/05/094932.13810820.jpg");
        //WWW www = new WWW(@"file:///C:\Users\Jay\Pictures\lbxx.jpg");
        //while (true)
        //{
        //    if (www.isDone)
        //    {
        //        break;
        //    }
        //}
        //yield return www; // 当 www 加载完或者加载失败之后, 继续方法

        //GetComponent<Renderer>().material.mainTexture = www.texture;
        //}

        //ResourceRequest rr = Resources.LoadAsync("Prefab/Wall1");
        //yield return rr;
        //GameObject go1 = rr.asset as GameObject;
        //Instantiate(go1);
    }
}