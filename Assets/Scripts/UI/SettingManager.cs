using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;
    public GameObject setting;

    void Awake()
    {
        instance = this;
        setting.SetActive(false);
    }

    void FixedUpdate() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
        }
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

    public void SceneToMenu() {
        Disable();
        if (GameManager.instance != null && GameManager.instance.gameObject != null) {
            Destroy(GameManager.instance.gameObject);
        }
        SceneManager.LoadScene("Menu");
    }

    public void Enable() {
        Time.timeScale = 0;
        setting.SetActive(true);
        
        AudioManager.instance.PlaySoundEffect("Audio/buttonclick");
        if (GameManager.instance != null) GameManager.instance.isPopupped = true;
    }

    public void Disable() {
        setting.SetActive(false);                
        AudioManager.instance.PlaySoundEffect("Audio/buttonclick");
        if (GameManager.instance != null) GameManager.instance.isPopupped = false;
        Time.timeScale = 1;
    }
}
