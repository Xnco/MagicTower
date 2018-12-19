using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Dialog : MonoBehaviour {

    public static Dialog instance;

    GameObject dialogPrefab;

	// Use this for initialization
	void Start () {
        instance = this;

        dialogPrefab = Resources.Load<GameObject>("Prefabs/UI/Dialog");
    }
	
	public void Open(string content, UnityAction callBack)
    {
        // 克隆的同时顺便设置父级, 做的锚点才是正常的
       GameObject dialog =  Instantiate(dialogPrefab, this.transform);
        //dialog.transform.SetParent(this.transform);
        dialog.transform.localPosition = Vector3.zero;

        dialog.transform.Find("Text").GetComponent<Text>().text = content;

        dialog.transform.Find("Confirm").GetComponent<Button>().onClick.AddListener(
            () =>
            {
                // 点击确定关闭界面 - 还要做 某事
                Destroy(dialog);
                if (callBack != null)
                {
                    callBack();
                }
            }
            );

        dialog.transform.Find("Cancel").GetComponent<Button>().onClick.AddListener(
            ()=>
            {
                // 点击取消关闭界面
                Destroy(dialog);
            }
            );
    }
}
