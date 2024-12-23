using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private HashSet<GameObject> enemiesInArea = new HashSet<GameObject>();
    public Unit currentUnit;
    public int line;

    private bool isInEnemy;

    BoxCollider2D coll;

    void Start() {
        coll = GetComponent<BoxCollider2D>();
    }

    void Update() {
        if (currentUnit == null) coll.enabled = true;
    }

    private void OnMouseDown()
    {
        HandManager.instance.OnCellClick(this);
        isInEnemy = false;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // "Enemy" 태그를 가진 오브젝트가 들어오면 리스트에 추가
        if (collision.CompareTag("Enemy")) {
            enemiesInArea.Add(collision.gameObject);
            isInEnemy = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // "Enemy" 태그를 가진 오브젝트가 나가면 리스트에서 제거
        if (collision.CompareTag("Enemy")) {
            enemiesInArea.Remove(collision.gameObject);
            if (enemiesInArea.Count == 0) isInEnemy = false;
        }
    }

    public bool AddUnit(Unit unit)
    {
        if (currentUnit != null) return false;
        if (isInEnemy) {
            HandManager.instance.CancelUnit();
            return false;
        }
        
        currentUnit = unit;

        // global level에 따른 수치 변경
        int atkLevel = ScoreManager.instance.atkLevel;
        int healthLevel = ScoreManager.instance.healthLevel;

        currentUnit.maxHealth += 5 * (healthLevel - 1);
        currentUnit.health = currentUnit.maxHealth;
        if (currentUnit.isRanged) currentUnit.damage += (atkLevel - 1);

        currentUnit.transform.position = transform.position;
        GameManager.instance.gold -= currentUnit.price;
        
        SpriteRenderer spriter = currentUnit.GetComponent<SpriteRenderer>();
        BoxCollider2D unitColl = currentUnit.GetComponent<BoxCollider2D>();

        spriter.sortingOrder = line;
        if (unitColl != null) unitColl.enabled = true;
        coll.enabled = false; // 현재 콜라이더 비활성화
        // Unit.TransitionToEnable();
        return true;
    }
}
