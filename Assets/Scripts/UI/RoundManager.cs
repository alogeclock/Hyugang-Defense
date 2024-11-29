using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    GameManager g;
    public int round;

    public GameObject[] roundUI;

    private void Awake() 
    {
        g = FindObjectOfType<GameManager>();
        round = 0;

        for (int i = 0; i < roundUI.Length; i++) {
            roundUI[i].SetActive(false);
        }
    }

    private void Update() 
    {
        if (round < g.round) {
            round = g.round;
            StartCoroutine(PrintRoundUI());
        }
    }

    IEnumerator PrintRoundUI()
    {
        if (round > roundUI.Length) {
            yield break;
        }

        GameObject currentUI = roundUI[round - 1];
        Image image = currentUI.GetComponent<Image>();

        if (image == null) {
            Debug.LogError("there is not image components in roundUI");
            yield break;
        }
        
        // 초기값 설정
        Color color = image.color;
        color.a = 0f;
        image.color = color;
        currentUI.SetActive(true);

        yield return StartCoroutine(FadeImage(image, 0f, 1f, 1f));    
        yield return new WaitForSeconds(3f); // 3초 대기
        yield return StartCoroutine(FadeImage(image, 1f, 0f, 1f));

        currentUI.SetActive(false);
    }

    IEnumerator FadeImage(Image image, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = image.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, t); // 알파 값 변경
            image.color = color;
            yield return null;
        }

        color.a = endAlpha; // 최종 알파 값 설정
        image.color = color;
    }
}
