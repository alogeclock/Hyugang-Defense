using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    public bool isActive;

    void Awake()
    {
        instance = this;
        isActive = false;
    }

    void FixedUpdate() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
        }
        
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
            statNumberUI.text = unit.damage.ToString();
        }
        else {
            nameUI.text = "포수";
            statUI.text = "";
            statNumberUI.text = "";
        }

        levelUI.text = unit.level.ToString();
        int upgradePrice = (currentUnit.level + 1) * currentUnit.price;
        if (unit.level < 3) costUI.text = "다음 학년까지 " + upgradePrice.ToString() + "원 필요합니다!";
        else costUI.text = "더 이상 진급할 수 없습니다.";
        hpUI.text = unit.health.ToString() + "/" + unit.maxHealth.ToString();
    }

    public void SellUnit() {
        // Debug.Log("before null check in sell unit");
        if (currentUnit == null) return;
        // Debug.Log("after null check in sell unit");
        Disable();
        GameManager.instance.gold += currentUnit.price / 2;
        Destroy(currentUnit.gameObject);
    }

    public void Enable() {
        GameManager.instance.popup.SetActive(true);
        GameManager.instance.isPopupped = true;
    }

    public void Disable() {
        GameManager.instance.popup.SetActive(false);
        GameManager.instance.isPopupped = false;
    }

    public void UpgradeUnit() 
    {
        Unit unit = currentUnit;

        Debug.Log("before null check in upgrade unit");
        if (unit == null || unit.level >= 3) return;
        Debug.Log("after null check in upgrade unit");

        int upgradePrice = (currentUnit.level + unit.level) * unit.price;

        if (GameManager.instance.gold >= upgradePrice) { // 업그레이드가 가능할 경우
            Disable(); // 팝업 비활성화
            GameManager.instance.gold -= upgradePrice;   // 가격만큼 돈 소모
            unit.level++;                         // 유닛 레벨 상승
            
            if (unit.isFarm) { // 파밍 유닛
                unit.maxHealth = unit.maxHealth += 100;
                unit.health = unit.maxHealth;
                unit.earn += 5;
            } 
            
            else if (unit.isRanged) { // 공격 유닛
                unit.maxHealth = unit.maxHealth += 100;
                unit.health = unit.maxHealth;
                unit.damage += 10;
            }

            else { // 방어 유닛
                unit.maxHealth = unit.maxHealth += 250;
                unit.health = unit.maxHealth;
            }
        }
    }
}
