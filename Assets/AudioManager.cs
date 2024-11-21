using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource audioSource;

    private void Awake()
    {
        // ���õ���ʵ��
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // ȷ��ֻ��һ�� AudioManager ����
            return;
        }

        DontDestroyOnLoad(gameObject); // ��ֹ�ڳ����л�ʱ����

        // ��ȡ AudioSource ���
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
        AudioClip ac = Resources.Load<AudioClip>(path); // �� Resources ������Ƶ
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

