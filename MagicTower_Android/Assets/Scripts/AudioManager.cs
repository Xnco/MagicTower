using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager instance;

    public static AudioManager GetSingle()
    {
        if (instance == null)
        {
            GameObject tmp = new GameObject("AudioManager");
            instance = tmp.AddComponent<AudioManager>();
            DontDestroyOnLoad(tmp);
        }
        return instance;
    }

    Dictionary<string, AudioClip> allAudioClip;
    List<AudioSource> allAudioSource;

    public void Awake()
    {
        allAudioClip = new Dictionary<string, AudioClip>();
        allAudioSource = new List<AudioSource>();
    }

    // 传入声音的名字, 根据名字播放对应的音效
    public void Play(string name)
    {
        AudioClip ac = GetAudio(name); // 获取到声音片段

        // 找一个空闲的播放器
        AudioSource free = allAudioSource.Find((x) => !x.isPlaying);
        if (free == null)
        {
            // 没有空闲的 - 新加一个组件
            AudioSource audio = this.gameObject.AddComponent<AudioSource>();
            audio.clip = ac;
            audio.playOnAwake = false;
            audio.Play(); // 播放一个音效

            allAudioSource.Add(audio);
        }
        else
        {
            // 有空闲的, 改成对应的音效, 播放
            free.clip = ac;
            //free.volume = 
            free.Play();
        }
    }

    // 获取声音片段
    public AudioClip GetAudio(string name)
    {
        if (!allAudioClip.ContainsKey(name))
        {
            AudioClip ac = Resources.Load<AudioClip>("Audio/" + name);
            allAudioClip.Add(name, ac);
        }
        return allAudioClip[name];
    }
}
