using System.Collections; // ��Ӵ���
using UnityEngine;

public class ClickSoundController : MonoBehaviour
{
    public AudioClip clickSound; // ���õ����Ч��AudioClip��
    public float playInterval = 1.0f; // ���ż��ʱ��
    private bool isPlaying = false; // ��ǵ�ǰ�Ƿ����ڲ���
    private AudioSource audioSource; // ��ƵԴ���

    void Start()
    {
        // ��ȡ�� GameObject �ϵ� AudioSource ���
        audioSource = GetComponent<AudioSource>();
        // ���û�� AudioSource ����������һ��
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isPlaying) // �������δ�ڲ���
        {
            StartCoroutine(PlaySoundWithInterval());
        }
    }

    private IEnumerator PlaySoundWithInterval()
    {
        isPlaying = true; // ��ֹ�ٴβ���
        audioSource.PlayOneShot(clickSound); // ʹ�� AudioSource ������Ч
        yield return new WaitForSeconds(playInterval); // �ȴ�ָ��ʱ��
        isPlaying = false; // �����ٴβ���
    }
}
