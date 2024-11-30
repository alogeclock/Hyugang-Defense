using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptManager : MonoBehaviour
{
    public Text scriptUI;
    string[] prologueScript;
    public Image[] prologueImage;
    private int scriptIdx;

    void Awake()
    {
        for (int i = 1; i < prologueImage.Length; i++) {
            prologueImage[i].gameObject.SetActive(false);
        }

        prologueScript = new string[] 
        {
            "좀비 바이러스가 창궐한 지도\n어느덧 3일 차···", 
            "서강고등학교에도\n좀비 바이러스의 불씨가 들이닥쳤다!", 
            "서강고등학교 최후의 방어선은\n서강고 야구 동아리.", 
            "\"어어 저기 들어온다!!\"", 
            "여기서 막지 못하면\n학교가 전부 감염된다!!"
        };

        scriptIdx = 0;
    }

    public void OnScriptClick() 
    {
        if (scriptIdx < prologueScript.Length - 1) {
            StartCoroutine(UpdateText());
        }
        else SceneChanger.instance.SceneToPlay();
    }

    IEnumerator UpdateText()
    {
        Color textColor = scriptUI.color;
        Color blankColor = new Color(1f, 1f, 1f, 0f);
        float fadeDuration = 0.2f; // 페이드 아웃 지속 시간
        float timer = 0f;

        // fade-out
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            scriptUI.color = Color.Lerp(textColor, blankColor, timer / fadeDuration);
            yield return null;
        }

        scriptIdx = Mathf.Min(scriptIdx + 1, prologueScript.Length - 1);
        scriptUI.text = prologueScript[scriptIdx];

        // fade-in
        timer = 0f;
        if (scriptIdx == 2) prologueImage[1].gameObject.SetActive(true);
        else if (scriptIdx == 3) prologueImage[2].gameObject.SetActive(true);
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            scriptUI.color = Color.Lerp(blankColor, textColor, timer / fadeDuration);
            yield return null;
        }
    }
}
