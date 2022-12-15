using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Bullet : MonoBehaviour
{
    private Vector3 shootDir;
    public float bulletDamage;
    public float bulletSpeed;

    [SerializeField]
    private int spread;

    private void Start()
    {
        bulletDamage = 30f;
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
        Monster target = collision.GetComponent<Monster>();
        if (target != null)
        {
            target.Damaged(bulletDamage);
            Destroy(gameObject);
        }

        if (collision.CompareTag("Wall"))
        {

            Destroy(gameObject);
        }
    }
}
