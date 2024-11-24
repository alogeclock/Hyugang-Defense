using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;

    void Awake() {
        instance = this;
    }

    public void SceneToPlay() {
        SceneManager.LoadScene("Play");
    }

    public void SceneToPrologue() {
        SceneManager.LoadScene("Prologue");
    }
    
    public void SceneToMenu() {
        SceneManager.LoadScene("Menu");
    }

    public void Exit() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else 
            Application.Quit();
        #endif
    }
}
