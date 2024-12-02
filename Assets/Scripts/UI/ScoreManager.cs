using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int score;
    public Text rankDialogue;
    public Text rankUI;

    void Start()
    {   
        if (instance == null) instance = this;
        else {
            Destroy(gameObject); 
            return;
        }
        rankDialogue.text = "당신의 등급은··· ";
        Disable();
        DontDestroyOnLoad(gameObject);
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
    }
}
