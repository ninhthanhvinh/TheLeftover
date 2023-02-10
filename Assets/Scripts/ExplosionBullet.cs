using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet : MonoBehaviour
{
    private Vector3 shootDir;
    public float explosionDamage;
    public float bulletSpeed;
    public GameObject explosionPrefab;

    [SerializeField]
    private int spread;
    [SerializeField]
    private float explosionRadius;
    [SerializeField]
    private LayerMask whatIsEnemy;

    private void Start()
    {
        explosionDamage = 30f;
    }
    public void Setup(Vector3 shootDir)
    {
        float y = Random.Range(-spread, spread);
        this.shootDir = shootDir;
        this.shootDir.y += y;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(shootDir));
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.position += shootDir * bulletSpeed * Time.deltaTime;

        /*float hitDetectionSize = 3f;
        Target target = Target.GetClosest(transform.position, hitDetectionSize);
        if(target != null)
        {
            target.Damage(10);
            Destroy(gameObject);
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Debug.Log(collision.name);

        //Monster target = collision.GetComponent<Monster>();
        Explode();

        
    }

    void Explode()
    {
        Collider2D[] monsters =
        Physics2D.OverlapCircleAll(transform.position, explosionRadius, whatIsEnemy);
        foreach (var item in monsters)
        {
            Monster monster = item.GetComponent<Monster>();
            if (monster != null)
            {
                monster.Damaged(explosionDamage);
            }
        }

        Destroy(gameObject);
    }
}
