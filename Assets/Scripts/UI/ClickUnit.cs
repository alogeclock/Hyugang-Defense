using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickUnit : MonoBehaviour
{
    private Unit unit;
    
    void Awake()
    {
        unit = GetComponentInParent<Unit>();
    }

     private void OnMouseDown()
    {
        HandManager.instance.OnUnitClick(unit);
    }
}
