using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource audioSource;

    private void Awake()
    {
        // 设置单例实例
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 确保只有一个 AudioManager 存在
            return;
        }

        DontDestroyOnLoad(gameObject); // 防止在场景切换时销毁

        // 获取 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on AudioManager.");
        }
    }

    private void Start()
    {
        PlayBGM(Config.BGM1);
    }

    public void PlayBGM(string path)
    {
        AudioClip ac = Resources.Load<AudioClip>(path); // 从 Resources 加载音频
        if (ac != null)
        {
            audioSource.clip = ac;
            audioSource.Play();
        }
        else
        {
            Debug.LogError($"Failed to load AudioClip at path: {path}");
        }
    }
}

