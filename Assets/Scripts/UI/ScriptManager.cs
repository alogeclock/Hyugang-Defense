using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptManager : MonoBehaviour
{
    public Text scriptUI;
    string[] prologueScript;
    private int idx;

    void Awake()
    {
        prologueScript = new string[] 
        {
            "좀비 바이러스가 창궐한 지도\n어느덧 3일 차···", 
            "서강고등학교에도\n좀비 바이러스의 불씨가 들이닥쳤다!", 
            "서강고등학교 최후의 방어선은\n서강고 야구 동아리.", 
            "\"어어 저기 들어온다!!\"", 
            "여기서 막지 못하면\n학교가 전부 감염된다!!"
        };

        idx = 0;
    }

    void FixedUpdate() 
    {    
        UpdateText();
    }

    public void OnScriptClick() 
    {
        if (idx < prologueScript.Length - 1) idx++;
        else SceneChanger.instance.SceneToPlay();
    }

    void UpdateText()
    {
        scriptUI.text = prologueScript[idx];
    }
}
