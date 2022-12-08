using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageEnemy : Monster
{
    [SerializeField]
    private Transform player;
    private Transform mage;
    public float aoeDamage;
    public float meleeDamage;
    private Vector3 dir;

    #region Public Variables
    public Transform rayCast;
    public LayerMask raycastMask;
    public float raycastLength;
    public float attackDistance;
    public float timer;
    #endregion

    #region Private Variables
    private RaycastHit2D hit;
    private GameObject target;
    private Animator anim;
    private float distance;
    private bool attackMode;
    private bool inRange;
    private bool cooling;
    private float intTimer;
    #endregion

    private HealthBar healthBar;

    private void Awake()
    {
        intTimer = timer;
    }
    public override void Attack()
    {
        if(currentHealthPoint > 0.4 * maxHealthPoint)
        {
            MeleeAttack();
        } else
        {
            StopAttack();
            RangeAttack();
        }
    }

    void RangeAttack()
    {
        if (Vector3.Distance(player.position, mage.position) < 2f)
            anim.SetBool("RangeAttack", true);
    }

    void MeleeAttack()
    {
        if (inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, raycastLength, raycastMask);
            RaycastDebugger();
        }
        if (hit.collider == null)
        {
            EnemyLogic();
        }
        else inRange = false;
        if (inRange == false)
        {
            anim.SetBool("MeleeAttack", false);
            StopAttack();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = 1f;
        maxHealthPoint = 100;
        currentHealthPoint = maxHealthPoint;
        mage = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        healthBar = transform.GetComponentInChildren<HealthBar>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {   
        dir = (player.position - mage.position);
        //if (Vector3.Distance(player.position, mage.position) < 6f) RangeAttack();
        if (dir.x > 0.01f)
        {
            mage.GetComponent<SpriteRenderer>().flipX = true;
        }
        else mage.GetComponent<SpriteRenderer>().flipX = false;


        Attack();


        if(currentHealthPoint < 0f)
        {
            anim.SetBool("RangeAttack", false);
            anim.SetBool("isDead", true);
        }

        healthBar.SetSize(currentHealthPoint / maxHealthPoint);
    }

    void EnemyLogic()
    {
        if (target == null)
        {
            var tempTarget = GameObject.FindGameObjectWithTag("Player");
            Move(tempTarget);
        }
        else
        {
            distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance < attackDistance && Mathf.Abs(transform.position.y - target.transform.position.y) < 0.25f)
            {
                Attacking();
            }

            else
            {
                Move(target);
                StopAttack();
            }

            if (cooling)
            {
                Cooldown();
                anim.SetBool("MeleeAttack", false);
            }
        }
    }

    void Move(GameObject target)
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("MeleeAttack"))
        {
            Vector2 targetPosition = new(target.transform.position.x, target.transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    void Attacking()
    {
        timer = intTimer;
        anim.SetBool("MeleeAttack", true);
    }

    void StopAttack()
    {
        cooling = false;
        anim.SetBool("MeleeAttack", false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            target = collision.gameObject;
            inRange = true;
        }
    }

    void RaycastDebugger()
    {
        if (distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * raycastLength, Color.red);

        }
        else if (attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * raycastLength, Color.green);
        }
    }

    public void DealMeleeDamage()
    {
        player.GetComponent<PlayerController>().Damage(meleeDamage);
    }

    public void DealAOEDamage()
    {
        if (Vector3.Distance(player.position, mage.position) < 2f)
        {
            player.GetComponent<PlayerController>().Damage(aoeDamage);
        }

        anim.SetBool("RangeAttack", false);
    }

    public void Dead()
    {
        Destroy(gameObject);
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;
        
        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }
}
