using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Raycast2DShooting 
{
    public static void Shoot(Vector3 shootPosition, Vector3 shootDirection)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(shootPosition, shootDirection);
        
        if (raycastHit2D.collider != null)
        {
            Monster target = raycastHit2D.collider.GetComponent<Monster>();
            if(target != null)
            {
                target.Damaged(10);
            }
        }
    }
}
