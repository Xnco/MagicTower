using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCtrl : MonoBehaviour {

    // 对话内容
    public TalkInfo[] content;

    public void StartTalk()
    {
        StartCoroutine(UITalk.instance.StartTalk(content));
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Start"))
        {
            StartTalk();
        }
    }
}

[System.Serializable]
public struct TalkInfo
{
    public string name;
    public string content;
    public Sprite headPortrait;
    //public Color myColor;
}
 