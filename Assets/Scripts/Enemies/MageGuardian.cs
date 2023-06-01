using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageGuardian : Monster
{
    [SerializeField]
    private Transform mage;
    public float damage;
    private Vector3 dir;

    #region Public Variables
    public float attackDistance;
    public float timer;
    public GameObject projectile;
    #endregion

    #region Private Variables
    private GameObject target;
    private float distance;
    #endregion

    private HealthBar healthBar;

    public override void Attack()
    {
        Instantiate(projectile, transform.position, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealthPoint = maxHealthPoint;
        mage = GetComponent<Transform>();
        healthBar = transform.GetComponentInChildren<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        dir = (target.transform.position - mage.position);
        //if (Vector3.Distance(player.position, mage.position) < 6f) RangeAttack();
        if (dir.x > 0.01f)
        {
            mage.GetComponent<SpriteRenderer>().flipX = true;
        }
        else mage.GetComponent<SpriteRenderer>().flipX = false;

        distance = Vector2.Distance(transform.position, target.transform.position);
        if (distance < attackDistance && timer <= 0)
        {
            target = transform.gameObject;
            Attack();
            timer = 2;
        }

        else
        {
            target = GameObject.FindGameObjectWithTag("Player");
            Move(target);
        }

        timer -= Time.deltaTime;
        healthBar.SetSize(currentHealthPoint / maxHealthPoint);
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
