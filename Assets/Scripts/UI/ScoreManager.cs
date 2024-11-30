using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int score;
    public Text rankUI;

    void Start()
    {   
        gameObject.SetActive(false);
        if (instance == null) instance = this;
        else {
            Destroy(gameObject); 
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance != null) score = GameManager.instance.score;
    }

    public void Enable() {
        GetRank();
        gameObject.SetActive(true);
    }

    public void Disable() {
        Destroy(gameObject);
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
