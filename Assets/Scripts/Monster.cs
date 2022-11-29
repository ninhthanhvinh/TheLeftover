using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    public float maxHealthPoint = 100;
    public float currentHealthPoint;
    public float speed;

    private static List<Monster> targetList;

    private void Start()
    {
        currentHealthPoint = maxHealthPoint;
    }


    public void Damaged(float damage)
    {
        if (gameObject != null)
        {
            currentHealthPoint -= damage;
        }
    }

    private void Awake()
    {
        if (targetList == null) targetList = new List<Monster>();
        targetList.Add(this);
    }
    private void FixedUpdate()
    {

        if (GetComponent<Monster>().currentHealthPoint <= 0) Destroy(gameObject);
    }

    public abstract void Attack();
}
