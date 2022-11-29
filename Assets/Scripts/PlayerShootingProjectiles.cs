using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using System;

public class PlayerShootingProjectiles : MonoBehaviour
{

    private Transform pfBullet;
    private GunController gun;
    [SerializeField] private Transform pfBulletPhysics;

    private void Start()
    {
        gun = GameObject.FindGameObjectWithTag("Weapon").GetComponent<GunController>();
        Debug.Log(GameObject.FindGameObjectWithTag("Weapon"));
        pfBullet = gun.bullet.transform;
    }

    private void Awake()
    {
        GetComponent<PlayerAimWeapon>().OnShoot += PlayerShootingProjectiles_OnShoot;
    }

    private void PlayerShootingProjectiles_OnShoot(object sender, PlayerAimWeapon.OnShootEventArgs e)
    {
        /*Vector3 shootDir = (e.shootPosition - e.gunEndPointPosition).normalized;
        Raycast2DShooting.Shoot(e.gunEndPointPosition, shootDir);*/


        Transform bulletTransform = Instantiate(pfBullet, Vector3.zero, Quaternion.identity);
        bulletTransform.position = e.gunEndPointPosition;
        Vector3 shootDir = (e.shootPosition - e.gunEndPointPosition).normalized;
        bulletTransform.GetComponent<Bullet>().Setup(shootDir);


        /*Transform bulletTransform = Instantiate(pfBulletPhysics, e.gunEndPointPosition, Quaternion.identity);

        Vector3 shootDir = (e.shootPosition - e.gunEndPointPosition).normalized;
        bulletTransform.GetComponent<BulletPhysics>().Setup(shootDir);*/

    }

}
