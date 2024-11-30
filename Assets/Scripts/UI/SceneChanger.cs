using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;
    
    void Awake() {
        /*
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        */
        instance = this;
        // DontDestroyOnLoad(instance);
    }

    public void LoadScene(string sceneName) {
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    public void SceneToPlay() {
        ScoreManager.instance.Disable();
        SceneManager.LoadScene("Play");
        AudioManager.instance.PlayBGM(Config.BGM1);
        // **************************************************
        // [재생되는 BGM을 플레이 중 BGM으로 변경]
        // 1. change to gameplay BGM for Play
        // **************************************************
    }

    public void SceneToPrologue() {
        // gameObject.SetActive(false);
        
        ScoreManager.instance.Disable();
        AudioManager.instance.PlaySoundEffect("Audio/buttonclick");
        SceneManager.LoadScene("Prologue");
    }
    
    public void SceneToMenu() {
        if (GameManager.instance != null && GameManager.instance.gameObject != null) {
            Destroy(GameManager.instance.gameObject);
        }
        ScoreManager.instance.Disable();
        AudioManager.instance.PlaySoundEffect("Audio/buttonclick");
        SceneManager.LoadScene("Menu");
        AudioManager.instance.PlayBGM(Config.MenuBGM);
        // **************************************************
        // [재생되는 BGM을 메뉴 BGM으로 변경]
        // 1. change to gameplay BGM for menu
        // **************************************************
    }

    public void SceneToWin() {
        ScoreManager.instance.score = (int)1e7;
        ScoreManager.instance.Enable();
        SceneManager.LoadScene("Win");
        
        // **************************************************
        // [재생되는 BGM을 승리 시 BGM으로 변경 or 승리 시 효과음을 재생, 둘 중 택1]
        // 1. change to gameplay BGM for win or 2. play SFX sound of win
        // **************************************************
    }

    public void SceneToLose() {
        ScoreManager.instance.Enable();
        SceneManager.LoadScene("Lose");

        // **************************************************
        // [재생되는 BGM을 패배 시 BGM으로 변경 or 패배 시 효과음을 재생, 둘 중 택1]
        // 1. change to gameplay BGM for defeat or 2. play SFX sound of defeat
        // **************************************************
    }

    public void Exit() {
        AudioManager.instance.PlaySoundEffect("Audio/buttonclick");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else 
            Application.Quit();
        #endif
    }
}
