using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class ShootFlash : MonoBehaviour
{
    [SerializeField] private PlayerAimWeapon aimWeapon;

    [SerializeField] private Sprite shootFlashSprite;

    public void Create(Vector3 spawnPosition)
    {
        World_Sprite worldSprite = World_Sprite.Create(spawnPosition, shootFlashSprite);
        FunctionTimer.Create(worldSprite.DestroySelf, .1f);
    }
}