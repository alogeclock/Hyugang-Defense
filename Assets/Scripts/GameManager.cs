using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PoolManager pool;
    public GameObject popup;

    public int gold;
    public int round;
    public int score;
    public int isGameOver;
    public float playTime;
    public float scoreTimer;

    public int monsterLevel;
    public float spawnInterval;

    public Text playTimeUI;
    public Text goldUI;
    public Text scoreUI;

    public bool isPopupped;
    private bool isBossSummoned;
    
    void Awake() {
        instance = this;
        monsterLevel = 1;
        isPopupped = false;
        gold = 450;
        playTime = 0.0f;
        score = 0;
        scoreTimer = 0.0f;
        spawnInterval = 10.0f;
        popup.SetActive(false);
    }

    void FixedUpdate() {
        playTime += Time.deltaTime;
        scoreTimer += Time.deltaTime;
        
        if (scoreTimer > 1.0f) {
            scoreTimer = 0.0f;
            score += 10;
        }

        playTimeUI.text = SetTime((int)playTime);
        goldUI.text = gold.ToString();
        scoreUI.text = score.ToString();

        /* gold 관련 UI debug 코드
        goldTimer += Time.deltaTime;
        if (goldTimer > 1.0f) {
            goldTimer = 0.0f;
            gold += 10;
        }
        */
        
        // 2분마다 1라운드가 진행됨, 최대 3라운드
        round = (int)Mathf.Min(1f + (float)(playTime / 120), 5);

        // Boss Round가 종료되면 Spawner가 멈추고, 보스 몬스터를 소환함
        int bossRound = Spawner.instance.spawnData.Length - 1;
        if (round >= bossRound + 1 && !isBossSummoned) {
            Spawner.instance.SpawnSpecific(0, 1);
            Spawner.instance.SpawnSpecific(0, 2);
            Spawner.instance.SpawnSpecific(0, 3);
            Spawner.instance.SpawnBoss(bossRound, 3); // boss(type 4)를 line 3(가장 아랫줄)에 소환
            isBossSummoned = true;
        }

        if (isBossSummoned && !PoolManager.FindObjectOfType<Enemy>()) Win();

        // 매 라운드마다 소환되는 몬스터 레벨이 높아짐
        monsterLevel = (int)Mathf.Min(round, Spawner.instance.spawnData.Length);

        // 점점 소환 속도가 빨라짐
        if (!isBossSummoned)
            spawnInterval = Mathf.Max(10.0f - (float)(playTime / 60), 3.0f);
    }

    public void Win() {
        Destroy(gameObject);
        SceneChanger.instance.SceneToWin();
    }

    public void Lose() {
        Destroy(gameObject);
        SceneChanger.instance.SceneToLose();
    }

    string SetTime(int time) {
        string min = (time / 60).ToString();
        if (int.Parse(min) < 10) min = "0" + min;
        string sec = (time % 60).ToString();
        if (int.Parse(sec) < 10) sec = "0" + sec;
        return min + ":" + sec;
    }
}
