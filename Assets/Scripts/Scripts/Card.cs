using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum CardState
{
    Disable,
    Cooling,
    Waiting,
    Ready
}

public enum UnitType
{
    unit1,
    unit2,
    unit3
}

public class Card : MonoBehaviour
{
    private CardState cardState = CardState.Ready;
    public UnitType unitType = UnitType.unit1;

    public GameObject cardLight;
    public GameObject cardGray;
    public Image cardMask;

    [SerializeField]
    private float cdTime = 2;
    private float cdTimer = 0;

    [SerializeField]
    private int gold;

    private void Awake() {
        gold = GameManager.instance.gold;
    }

    private void Update()
    {
        gold = GameManager.instance.gold;
        switch (cardState)
        {
            case CardState.Cooling:
                CoolingUpdate();
                break;
            case CardState.Waiting:
                WaitingUpdate();
                break;
            case CardState.Ready:
                ReadyUpdate();
                break;
            default:
                break;
        }
    }

    void CoolingUpdate()
    {
        cdTimer += Time.deltaTime;
        cardMask.fillAmount = (cdTime - cdTimer) / cdTime;
        if (cdTimer >= cdTime) TransitionToWaiting();
    }
    void WaitingUpdate()
    {
        if (gold >= 100) TransitionToReady();
    }
    void ReadyUpdate() {
        if (gold < 100) TransitionToWaiting();
    }

    void TransitionToWaiting()
    {
        cardState = CardState.Waiting;
        cardLight.SetActive(false);
        cardGray.SetActive(true);
        cardMask.gameObject.SetActive(false);
    }

    void TransitionToReady()
    {
        cardState = CardState.Ready;
        cardLight.SetActive(true);
        cardGray.SetActive(false);
        cardMask.gameObject.SetActive(false);
    }
    
    void TransitionToCooling()
    {
        cardState = CardState.Cooling;
        cdTimer = 0;
        cardLight.SetActive(false);
        cardGray.SetActive(true);
        cardMask.gameObject.SetActive(true);
    }

    public void OnClick()
    {
        // AudioManager.Instance.PlayClip(Config.btn_click);
        if (cardState == CardState.Disable) return;
        if (GameManager.instance.gold < 100) return;
        bool isSuccess = HandManager.instance.AddUnit((int)unitType);
        
        if (isSuccess)
        {
            // SunManager.Instance.SubSun(needSunPoint);
            GameManager.instance.gold -= 100;
            TransitionToCooling();
        }
    }

    public void DisableCard()
    {
        cardState = CardState.Disable;
    }
    
    public void EnableCard()
    {
        TransitionToCooling();
    }

}
