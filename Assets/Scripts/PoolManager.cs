using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] pools;
    
    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int idx = 0; idx < pools.Length; idx++)
            pools[idx] = new List<GameObject>();
    }

    public GameObject Get(int idx)
    {
        GameObject select = null;

        // access an disabled GameObject.. if detected, allocate it to select variable
        foreach (GameObject item in pools[idx]) {
            if (!item.activeSelf) {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // if all GameObjects are enabled.. 
        // instantiate new GameObject and allocate it to select var
        if (!select) {
            select = Instantiate(prefabs[idx], transform);
            pools[idx].Add(select);
        }            
        
        return select;
    }
}
