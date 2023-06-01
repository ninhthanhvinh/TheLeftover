using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSupport : Monster
{

    public float timer;
    float count;

    private HealthBar healthBar;

    public override void Attack()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        count = timer;
        currentHealthPoint = maxHealthPoint;
        healthBar = transform.GetComponentInChildren<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        Move(GameObject.FindGameObjectWithTag("Player"));
        count -= Time.deltaTime;
        if(count <= 0)
        {
            Healing();
            count = timer;
        }
        healthBar.SetSize(currentHealthPoint / maxHealthPoint);
    }

    void Healing()
    {
        var monsters = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (Collider2D collider in monsters)
        {
            if (collider.GetComponent<Monster>() != null)
            {
                collider.GetComponent<Monster>().Healing(40f);
            }
        }
    }

    void Move(GameObject target)
    {
        Vector2 targetPosition = new(target.transform.position.x, target.transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void Dead()
    {
        Destroy(gameObject);
    }

}
