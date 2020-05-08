using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : BossWeapon
{
    private const string projectileName = "Missile";

    public override bool Activate(int behavior)
    {
        GameObject missile = ObjectPooler.instance.SpawnFromPool(projectileName, transform.position, Quaternion.identity);
        return true;
    }
}
