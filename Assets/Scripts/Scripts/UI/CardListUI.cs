using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardListUI : MonoBehaviour
{
    public List<Card> cardList;

    private void Start()
    {
      //  DisableCardList();
       // EnableCardList();
        //ShowCardList();
    }

    public void ShowCardList()
    {
        GetComponent<RectTransform>().DOAnchorPosY(-48,1);
        EnableCardList();
    }
    
    public void DisableCardList()
    {
        foreach(Card card in cardList)
        {
            card.DisableCard();
        }
    }
    void EnableCardList()
    {
        foreach (Card card in cardList)
        {
            card.EnableCard();
        }
    }
    
}
