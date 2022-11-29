using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using CodeMonkey.Utils;

public class PlayerAimWeapon : MonoBehaviour
{
    private Transform aimTransform;
    private Transform aimGunEndPointPosition;
    private GameObject inventoryInterface;

    private float fireRate;
    private float nextFire = 0f;
    private bool allowFire = true;
    private int count = 0;
    public event EventHandler<OnShootEventArgs> OnShoot;
    private GunController gun;

    public class OnShootEventArgs : EventArgs
    {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
    }

    //private Animator aimAnimator;
    private void Awake()
    {
        inventoryInterface = GameObject.Find("InventoryScreen");

        aimTransform = GameObject.FindGameObjectWithTag("Weapon").transform;
        aimGunEndPointPosition = aimTransform.Find("GunEndPointPosition");
        Debug.Log(aimTransform);
        gun = aimTransform.GetComponent<GunController>();
        fireRate = gun.fireRate;
        //aimAnimator = aimTransform.GetComponent<Animator>();
    }

    private void Update()
    {
        HandleAiming();
        HandleShooting();
    }

    private void HandleAiming()
    {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        
        Vector3 aimDirection = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (aimTransform != null)
            aimTransform.eulerAngles = new Vector3(0, 0, angle);
    }

    private void HandleShooting()
    {

        if (Input.GetMouseButton(0) && Time.time > nextFire && !inventoryInterface.activeSelf && allowFire)
        {
            //aimAnimator.SetTrigger("Shoot");

            nextFire = Time.time + fireRate;
            if (count < gun.bulletsInAMagazine)
            {
                Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();


                OnShoot?.Invoke(this, new OnShootEventArgs
                {
                    gunEndPointPosition = aimGunEndPointPosition.position,
                    shootPosition = mousePosition
                });
                count++;
            }
            else
            {
                allowFire = false;
                count = 0;
                StartCoroutine(FireCooldown(gun.reloadTime));
                
            }

        }

    }

    IEnumerator FireCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Reloading");
        allowFire = true;
    }

    public Transform FindWeapon()
    {
        if (GameObject.FindGameObjectWithTag("Weapon") == null) return null;
        return GameObject.FindGameObjectWithTag("Weapon").transform;
    }
}

