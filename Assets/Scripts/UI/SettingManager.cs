using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public void SceneToMenu() {
        Disable();
        if (GameManager.instance != null) Destroy(GameManager.instance);
        SceneChanger.instance.SceneToMenu();
    }

    public void Enable() {
        Time.timeScale = 0;
        setting.SetActive(true);
        if (GameManager.instance != null) GameManager.instance.isPopupped = true;
    }

    public void Disable() {
        setting.SetActive(false);
        if (GameManager.instance != null) GameManager.instance.isPopupped = false;
        Time.timeScale = 1;
    }
}
