using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Spell : MonoBehaviour
{
    private Vector3 direction;
    public float damage;
    public float speed;
    Transform player;
    [SerializeField]
    GameObject boom;

    private void Start()
    {
        damage = 30f;
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        direction = -transform.position + player.position;
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController target = collision.GetComponent<PlayerController>();
        if (target != null)
        {
            target.Damage(damage);
            Instantiate(boom, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (collision.CompareTag("Wall"))
        {

            Destroy(gameObject);
        }
    }
}
