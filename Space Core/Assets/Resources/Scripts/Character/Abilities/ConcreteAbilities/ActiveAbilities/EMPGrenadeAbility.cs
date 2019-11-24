using System.Collections;
using UnityEngine;

public class EMPGrenadeAbility : ActiveAbility
{
    protected override bool activationCondition => activationTimer <= 0f;
    private GameObject EMPGrenade;
    private float force;

    void Start()
    {
        force = 17500f;
        EMPGrenade = Resources.Load<GameObject>("Prefabs/Abilities/EMPGrenade");
        Cooldown = 15f;
    }

    protected override void Activate()
    {
        GameObject empGrenadeObject = Instantiate(EMPGrenade, owner.Weapon.FireLocation.transform.position, Quaternion.identity);
        empGrenadeObject.GetComponent<Rigidbody2D>().AddForce(owner.Weapon.FireLocation.transform.right * force);
        activationTimer = Cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY) 
            return;

        if (activationTimer > 0f)
        {
            activationTimer -= Time.deltaTime;
        }
    }
}
