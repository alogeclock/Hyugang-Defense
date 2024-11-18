using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PoolManager pool;

    public int gold;
    // public int round;
    public int isGameOver;
    public float playTime;

    public int monsterLevel;
    public float spawnInterval;

    public Text playTimeUI;
    public Text goldUI;

    void Awake() {
        instance = this;
        monsterLevel = 1;
        gold = 300;
        spawnInterval = 15.0f;
    }

    void FixedUpdate() {
        playTime += Time.deltaTime;
        playTimeUI.text = SetTime((int)playTime);

        goldUI.text = gold.ToString();

        /* gold 관련 UI debug 코드
        goldTimer += Time.deltaTime;
        if (goldTimer > 1.0f) {
            goldTimer = 0.0f;
            gold += 10;
        }
        */
        
        // 매 1분마다 소환되는 몬스터 레벨이 높아짐
        monsterLevel =  (int)Mathf.Min(1f + (float)(playTime / 60), Spawner.instance.spawnData.Length);

        // 점점 소환 속도가 빨라짐
        spawnInterval = Mathf.Max(15.0f - (float)(playTime / 15), 3.0f);
    }

    string SetTime(int time) {
        string min = (time / 60).ToString();
        if (int.Parse(min) < 10) min = "0" + min;
        string sec = (time % 60).ToString();
        if (int.Parse(sec) < 10) sec = "0" + sec;
        return min + ":" + sec;
    }
}
