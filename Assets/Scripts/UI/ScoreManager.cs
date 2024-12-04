using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int globalGold;
    public int score;

    public Text rankDialogue;
    public Text rankUI;
    public Text goldUI;
    public GameObject goldGameObj;

    public int atkLevel;
    public int healthLevel;
    public int goldLevel;

    void Start()
    {   
        if (instance == null) instance = this;
        else {
            Destroy(gameObject); 
            return;
        }
        rankDialogue.text = "당신의 등급은···";
        Disable();
        DontDestroyOnLoad(gameObject);

        // 글로벌 골드 초기화
        globalGold = 0;

        atkLevel = 1;
        healthLevel = 1;
        goldLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance != null) score = GameManager.instance.score;
    }

    public void Enable() {
        GetRank();
        rankDialogue.gameObject.SetActive(true);
        rankUI.gameObject.SetActive(true);
    }

    public void Disable() {
        rankDialogue.gameObject.SetActive(false);
        rankUI.gameObject.SetActive(false);
    }

    void GetRank() {
        if (score < 2500) rankUI.text = "F";
        else if (score < 5000) rankUI.text = "D";
        else if (score < 7500) rankUI.text = "C";
        else if (score < 10000) rankUI.text = "B";
        else if (score < 12500) rankUI.text = "A";
        else if (score < (int)1e7) rankUI.text = "A+";
        else rankUI.text = "S";
        globalGold += Mathf.Min((int)(score * 0.02), 300);
    }

    public void upgradeAtk() {
        AudioManager.instance.PlaySoundEffect("Audio/buttonclick");
        if (globalGold < atkLevel * 100) return;
        globalGold -= atkLevel * 100;
        atkLevel++;
    }
    
    public void upgradeHealth() {
        AudioManager.instance.PlaySoundEffect("Audio/buttonclick");
        if (globalGold < healthLevel * 100) return;
        globalGold -= healthLevel * 100;
        healthLevel++;
    }

    public void upgradeGold() {
        AudioManager.instance.PlaySoundEffect("Audio/buttonclick");
        if (globalGold < goldLevel * 100) return;
        globalGold -= goldLevel * 100;
        goldLevel++;
    }
}
