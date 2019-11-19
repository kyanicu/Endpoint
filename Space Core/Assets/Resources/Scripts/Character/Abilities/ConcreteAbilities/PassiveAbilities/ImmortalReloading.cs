﻿using System.Collections;
using UnityEngine;

public class ImmortalReloading : PassiveAbility
{
    protected override bool activationCondition => activationFlag;
    /// <summary>
    /// Boolean flag used to signify the start of owner's immortality
    /// </summary>
    private bool activationFlag;
    /// <summary>
    /// Boolean flag used to signify the start of the owner's reload
    /// </summary>
    private bool reloadStarted;

    protected override void Activate()
    {
        StartCoroutine(Immortality());
        activationFlag = false;
    }

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        activationFlag = false;
        reloadStarted = false;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (owner.Weapon.IsReloading && !reloadStarted)
        {
            StartCoroutine(ReloadBuffer());
            reloadStarted = true;
        }
    }
    /// <summary>
    /// Timeout for the owner to finish reloading
    /// </summary>
    /// <returns></returns>
    IEnumerator ReloadBuffer()
    {
        yield return new WaitForSeconds(owner.Weapon.ReloadTime);
        activationFlag = true;
        reloadStarted = false;
    }

    /// <summary>
    /// Timer used to measure the owner's immortality duration
    /// Set to 5 seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator Immortality()
    {
        owner.isImmortal = true;
        yield return new WaitForSeconds(5);
        owner.isImmortal = false;
    }
}