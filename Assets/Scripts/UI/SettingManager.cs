using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;

    void Awake()
    {
        instance = this;
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
        Destroy(GameManager.instance);
        SceneChanger.instance.SceneToMenu();
        
    }

    public void Enable() {
        Time.timeScale = 0;
        GameManager.instance.setting.SetActive(true);
        GameManager.instance.isPopupped = true;
    }

    public void Disable() {
        GameManager.instance.setting.SetActive(false);
        GameManager.instance.isPopupped = false;
        Time.timeScale = 1;
    }
}
