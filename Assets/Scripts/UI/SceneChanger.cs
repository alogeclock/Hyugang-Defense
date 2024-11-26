using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;
    
    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(instance);
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
        SceneManager.LoadScene("Play");
    }

    public void SceneToPrologue() {
        gameObject.SetActive(false);
        SceneManager.LoadScene("Prologue");
    }
    
    public void SceneToMenu() {
        SceneManager.LoadScene("Menu");
        gameObject.SetActive(true);
    }

    public void SceneToWin() {
        SceneManager.LoadScene("Win");
    }

    public void SceneToLose() {
        SceneManager.LoadScene("Lose");
    }

    public void Exit() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else 
            Application.Quit();
        #endif
    }
}
