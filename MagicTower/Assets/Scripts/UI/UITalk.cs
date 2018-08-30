using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UITalk : MonoBehaviour {

    public Transform content;
    public Text nameText;
    public Text textText;
    public Image head;

    public static UITalk instance;

    // 对话开始
    public UnityAction onTalkStart;

    // 对话结束
    public UnityAction onTalkEnd;

	// Use this for initialization
	void Start () {
        instance = this;
    }

    // 展示名字和 n 句话
    public IEnumerator StartTalk(TalkInfo[] texts)
    {
        // 关闭主角的操作
        if (onTalkStart != null)
        {
            onTalkStart();
        }
        content.gameObject.SetActive(true);

        for (int i = 0; i < texts.Length; i++)
        {
            nameText.text = texts[i].name; // 改名字
            
            string tmpText = texts[i].content; // 改内容
            //textText.color = texts[i].myColor;

            head.sprite = texts[i].headPortrait; // 改头像

            //textText.text = tmpText; // 直接显示一句话
            for (int j = 1; j < tmpText.Length; j++)
            {
                textText.text = tmpText.Substring(0, j+1);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(3);
        }

        content.gameObject.SetActive(false);
        // 打开主角操作
        if (onTalkEnd != null)
        {
            onTalkEnd();
        }
    }
}
