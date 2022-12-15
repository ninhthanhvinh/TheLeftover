using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public float fireRate;
    public GameObject bullet;
    public float reloadTime;
    public float bulletsInAMagazine;
    public string soundName;
    public string reloadSoundName;
    public int bulletsPerTap;
}
