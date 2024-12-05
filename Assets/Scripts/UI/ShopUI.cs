using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;

    int atkLevel;
    int healthLevel;
    int goldLevel;

    public Text[] LevelUI;
    public Text[] costUI;
    public Text goldUI;
    public GameObject goldGameObj;

    void Start() {
        UpdateUI();
    }

    void UpdateUI() {
        atkLevel = ScoreManager.instance.atkLevel;
        healthLevel = ScoreManager.instance.healthLevel;
        goldLevel = ScoreManager.instance.goldLevel;

        LevelUI[0].text = "LV." + atkLevel.ToString();
        LevelUI[1].text = "LV." + healthLevel.ToString();
        LevelUI[2].text = "LV." + goldLevel.ToString();

        costUI[0].text = (50 * (atkLevel - 1) + 100).ToString();
        costUI[1].text = (50 * (healthLevel - 1) + 100).ToString();
        costUI[2].text = (50 * (goldLevel - 1) + 100).ToString();

        goldUI.text = ScoreManager.instance.globalGold.ToString();
    }

    public void PressUpgradeAtk() {
        ScoreManager.instance.upgradeAtk();
        UpdateUI();
    }

    public void PressUpgradeHealth() {
        ScoreManager.instance.upgradeHealth();
        UpdateUI();
    }

    public void PressUpgradeGold() {
        ScoreManager.instance.upgradeGold();
        UpdateUI();
    }
}
