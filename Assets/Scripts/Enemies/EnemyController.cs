using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Monster
{
    public Transform player;
    public float moveSpeed;
    public float attackRange;
    public float checkRange;
    public float attackCoolDown;
    public LayerMask whatIsPlayer;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    private Vector2 dir;

    private bool isInChaseRange;
    private bool isInAttackRange;
    private float nextAttack;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        nextAttack = Time.time;
    }

    private void Update()
    {
        anim.SetFloat("Speed", GetMovementState(isInChaseRange));

        isInChaseRange = Physics2D.OverlapCircle(transform.position, checkRange, whatIsPlayer);
        isInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);

        dir = player.position - transform.position;
        dir.Normalize();
        movement = dir;
        float horizontal = dir.x / dir.SqrMagnitude();
        float vertical = dir.y / dir.SqrMagnitude();
        anim.SetFloat("Horizontal", horizontal);
        anim.SetFloat("Vertical", vertical);
    }

    public float GetMovementState(bool _bool)
    {
        if (_bool) return 0;
        else return 1;
    }

    private void FixedUpdate()
    {
        if(isInChaseRange && !isInAttackRange)
        {
            //MoveCharacter(movement);
        }
        if (isInAttackRange && Time.time > nextAttack)
        {
            anim.SetTrigger("isAttacking");
            nextAttack += attackCoolDown;
            Attack();
            
            //do damage
        }
    }

    public void MoveCharacter(Vector2 dir)
    {
        rb.MovePosition((Vector2)transform.position + (moveSpeed * Time.deltaTime * dir));
    }

    public override void Attack()
    {
        player.GetComponent<PlayerController>().Damage(20);
    }
}

