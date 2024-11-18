using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager instance { get; private set; }
    public List<Unit> unitPrefabList;
    private Unit currentUnit;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        FollowCursor();
    }

    public bool AddUnit(int unitType)
    {
        if (currentUnit != null) return false;
        
        Unit unitPrefab = unitPrefabList[unitType];
        if (unitPrefab == null) {
            Debug.LogError("Unit Prefab is not exist.");
            return false; 
        }

        currentUnit = GameObject.Instantiate(unitPrefab);
        
        return true;
    }

    void FollowCursor()
    {
        if (currentUnit == null) return;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0.5f;
        currentUnit.transform.position = mouseWorldPosition;

        BoxCollider2D unitColl = currentUnit.GetComponent<BoxCollider2D>();
        if (unitColl != null) unitColl.enabled = false;
    }
    

    public void OnCellClick(Cell cell)
    {
        if (currentUnit == null) return;
        bool isSuccess = cell.AddUnit(currentUnit);
        
        if (isSuccess)
        { 
            currentUnit = null;
           // AudioManager.Instance.PlayClip(Config.plant);
        }
    }
}
