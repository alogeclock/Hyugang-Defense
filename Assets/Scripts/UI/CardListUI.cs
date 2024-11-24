using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardListUI : MonoBehaviour
{
    public List<Card> cardList;
    
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
