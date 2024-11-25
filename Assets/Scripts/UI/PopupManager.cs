using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;
    public Unit currentUnit;
    
    public Image image;
    public Text levelUI;
    public Text nameUI;
    public Text statUI;
    public Text statNumberUI;
    public Text hpUI;
    public Text costUI;

    void Awake()
    {
        instance = this;
    }

    public void UpdateData(Unit unit)
    {
        currentUnit = unit;

        if (unit.spriter != null) image.sprite = unit.spriter.sprite;
        else Debug.LogWarning("unit does not have a valid sprite");
        if (unit.isFarm) {
            nameUI.text = "부원";
            statUI.text = "EARN";
            statNumberUI.text = unit.earn.ToString();
        }
        else if (unit.isRanged) {
            nameUI.text = "타자";
            statUI.text = "ATK";
            statNumberUI.text = (10 * unit.level).ToString();
        }
        else {
            nameUI.text = "포수";
            statUI.text = "";
            statNumberUI.text = "";
        }

        levelUI.text = unit.level.ToString();
        costUI.text = (unit.price * 2).ToString();
        hpUI.text = unit.health.ToString() + "/" + unit.maxHealth.ToString();
    }

    public void SellUnit() {
        // Debug.Log("before null check in sell unit");
        if (currentUnit == null) return;
        // Debug.Log("after null check in sell unit");
        GameManager.instance.popup.SetActive(false);
        GameManager.instance.gold += currentUnit.price / 2;
        Destroy(currentUnit.gameObject);
    }
}
