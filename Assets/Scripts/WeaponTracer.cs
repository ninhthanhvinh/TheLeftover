using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class WeaponTracer : MonoBehaviour
{
    [SerializeField] private PlayerAimWeapon aimWeapon;

    [SerializeField] private Material weaponTracerMaterial;

    public void Create(Vector3 fromPosition, Vector3 targetPosition)
    {
        Vector3 dir = (targetPosition - fromPosition).normalized;
        float eulerZ = UtilsClass.GetAngleFromVector(dir) - 90;
        float distance = Vector3.Distance(fromPosition, targetPosition);
        Vector3 tracerSpawnPosition = fromPosition + dir * distance * .5f;
        //Material tmpWeaponTracerMaterial = new Material(weaponTracerMaterial);
        //tmpWeaponTracerMaterial.SetTextureScale("_MainTex", new Vector2(1f, distance / 256f));
        World_Mesh world_Mesh = World_Mesh.Create(tracerSpawnPosition, eulerZ, 1f, distance, weaponTracerMaterial, null, 10000);

        int frame = 0;
        float framerate = .016f;
        float timer = framerate;
        world_Mesh.SetUVCoords(new World_Mesh.UVCoords(0, 0, 16, 256));
        FunctionUpdater.Create(() =>
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                frame++;
                timer += framerate;
                if (frame >= 4)
                {
                    world_Mesh.DestroySelf();
                    return true;
                }
                else
                {
                    world_Mesh.SetUVCoords(new World_Mesh.UVCoords(16 * frame, 0, 16, 256));
                }

            }
            return false;
        });
    }
}
