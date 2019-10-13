using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Weapon gun;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            gun.Fire();
            Debug.Log($"Number of shots left: {gun.AmmoInClip}");
            Debug.Log($"Total Ammo: {gun.TotalAmmo}");
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log($"Reloading!");
            StartCoroutine(gun.Reload());
            Debug.Log($"Number of shots left: {gun.AmmoInClip}");
            Debug.Log($"Total Ammo: {gun.TotalAmmo}");
        }
    }
}
