using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Unit currentPlant;

    private void OnMouseDown()
    {
        HandManager.Instance.OnCellClick(this);
    }

    public bool AddPlant(Unit plant)
    {
        if (currentPlant != null) return false;

        currentPlant = plant;
        currentPlant.transform.position = transform.position;
      //  plant.TransitionToEnable();
        return true;
    }

}
