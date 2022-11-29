using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class BulletPhysics : MonoBehaviour
{
    public void Setup(Vector3 shootDir)
    {
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        float moveSpeed = 150f;
        rigidbody2D.AddForce(shootDir * moveSpeed, ForceMode2D.Impulse);

        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(shootDir));
        Destroy(gameObject, 5f);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Monster target = collision.GetComponent<Monster>();
        if (target != null)
        {
            target.Damaged(10);
            Destroy(gameObject);
        }
    }
}
