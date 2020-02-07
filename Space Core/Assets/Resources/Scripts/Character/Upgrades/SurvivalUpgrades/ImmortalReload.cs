using System.Collections;
using UnityEngine;

public class ImmortalReload : WheelUpgrade
{
    private bool reloadStarted;
    private bool invincibilityAdded;
    protected override bool activationCondition => PlayerController.instance.Character.Weapon.IsReloading;

    /// <summary>
    /// Time that the player stays immortal after reloading
    /// </summary>
    public float ImmortalTime = 2.0f;

    private void Start()
    {
        invincibilityAdded = false;
        reloadStarted = false;
    }

    public override void DeactivateAbility()
    {
        if (invincibilityAdded)
        {
            StopCoroutine(ResetImmortality());
            PlayerController.instance.Character.Invincible--;
        }
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(activationCondition && !reloadStarted)
        {
            StartCoroutine(ResetImmortality());
        }
    }

    private IEnumerator ResetImmortality()
    {
        yield return new WaitForSeconds(PlayerController.instance.Character.Weapon.ReloadTime);
        PlayerController.instance.Character.Invincible++;
        invincibilityAdded = true;
        yield return new WaitForSeconds(ImmortalTime);
        PlayerController.instance.Character.Invincible--;
        invincibilityAdded = false;
    }

    public override void EnableAbility()
    {
        invincibilityAdded = false;
        reloadStarted = false;
    }
}
