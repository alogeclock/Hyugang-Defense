using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Method to load menu scene and play MenuBGM
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");  // 加载菜单场景
    }

    // Method to load game scene and play BGM1
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");  // 加载游戏场景
    }
}
