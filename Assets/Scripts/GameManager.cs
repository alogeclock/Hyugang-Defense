using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager g;
    public int gold;
    public int round;
    public int isGameOver;
    public float playTime;
    public Text playTimeUI;
    public Text goldUI;
    
    public float goldTimer;

    void Update() {
        playTime += Time.deltaTime;
        playTimeUI.text = SetTime((int)playTime);

        // gold 관련 UI debug 코드
        goldUI.text = gold.ToString();
        goldTimer += Time.deltaTime;
        if (goldTimer > 1.0f) {
            goldTimer = 0.0f;
            gold += 10;
        }
    }

    string SetTime(int time) {
        string min = (time / 60).ToString();
        if (int.Parse(min) < 10) min = "0" + min;
        string sec = (time % 60).ToString();
        if (int.Parse(sec) < 10) sec = "0" + sec;
        return min + ":" + sec;
    }
}
