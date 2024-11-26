using System.Collections; // 添加此行
using UnityEngine;

public class ClickSoundController : MonoBehaviour
{
    public AudioClip clickSound; // 引用点击音效（AudioClip）
    public float playInterval = 1.0f; // 播放间隔时间
    private bool isPlaying = false; // 标记当前是否正在播放
    private AudioSource audioSource; // 音频源组件

    void Start()
    {
        // 获取该 GameObject 上的 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        // 如果没有 AudioSource 组件，则添加一个
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isPlaying) // 检测点击且未在播放
        {
            StartCoroutine(PlaySoundWithInterval());
        }
    }

    private IEnumerator PlaySoundWithInterval()
    {
        isPlaying = true; // 禁止再次播放
        audioSource.PlayOneShot(clickSound); // 使用 AudioSource 播放音效
        yield return new WaitForSeconds(playInterval); // 等待指定时间
        isPlaying = false; // 允许再次播放
    }
}
