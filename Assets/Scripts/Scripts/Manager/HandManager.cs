using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance { get; private set; }

    public List<Unit> plantPrefabList;

    private Unit currentPlant;

    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        FollowCursor();
    }

    public bool AddPlant(PlantType plantType)
    {
        if (currentPlant != null) return false;
        Debug.Log(plantType);
        Unit plantPrefab = GetPlantPrefab(plantType);
        if (plantPrefab == null)
        {
            print("要种植的植物不存在");return false;
        }
        currentPlant = GameObject.Instantiate(plantPrefab);
        return true;
    }

    private Unit GetPlantPrefab(PlantType plantType)
    {
        foreach(Unit plant in plantPrefabList)
        {
            if (plant.plantType == plantType)
            {
                return plant;
            }
        }
        return null;
    }
    void FollowCursor()
    {
        if (currentPlant == null) return;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        currentPlant.transform.position = mouseWorldPosition;

    }

    public void OnCellClick(Cell cell)
    {
        if (currentPlant == null) return;

        bool isSuccess = cell.AddPlant(currentPlant);

        if (isSuccess)
        {
            currentPlant = null;
           // AudioManager.Instance.PlayClip(Config.plant);
        }
    }

}
