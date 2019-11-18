using System.Collections;
using UnityEngine;

public class ImmortalReloading : PassiveAbility
{
    protected override bool activationCondition => activationFlag;
    /// <summary>
    /// Boolean flag used to signify the start of player immortality
    /// </summary>
    private bool activationFlag;
    /// <summary>
    /// Boolean flag used to signify the start of a player reload
    /// </summary>
    private bool reloadStarted;

    protected override void Activate()
    {
        StartCoroutine(Immortality());
        activationFlag = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        activationFlag = false;
        reloadStarted = false;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (Player.instance.Weapon.IsReloading && !reloadStarted)
        {
            StartCoroutine(ReloadBuffer());
            reloadStarted = true;
        }
    }
    /// <summary>
    /// Timeout for the player to finish reloading
    /// </summary>
    /// <returns></returns>
    IEnumerator ReloadBuffer()
    {
        yield return new WaitForSeconds(Player.instance.Weapon.ReloadTime);
        activationFlag = true;
        reloadStarted = false;
    }

    /// <summary>
    /// Timer used to measure the player's immortality duration
    /// Set to 5 seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator Immortality()
    {
        Player.instance.isImmortal = true;
        yield return new WaitForSeconds(5);
        Player.instance.isImmortal = false;
    }
}
