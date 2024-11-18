using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Unit currentUnit;
    public int line;

    private void OnMouseDown()
    {
        HandManager.instance.OnCellClick(this);
    }

    public bool AddUnit(Unit unit)
    {
        if (currentUnit != null) return false;
        
        currentUnit = unit;
        currentUnit.transform.position = transform.position;
        
        SpriteRenderer spriter = currentUnit.GetComponent<SpriteRenderer>();
        BoxCollider2D unitColl = currentUnit.GetComponent<BoxCollider2D>();

        spriter.sortingOrder = line;
        if (unitColl != null) unitColl.enabled = true;
    
        // Unit.TransitionToEnable();
        return true;
    }
}
