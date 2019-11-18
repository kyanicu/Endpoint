using System.Collections;
using UnityEngine;

public class EMPGrenadeAbility : ActiveAbility
{
    protected override bool activationCondition => activationTimer <= 0f;
    private GameObject EMPGrenade;
    public float activationTimer { get; set; }
    private float force;

    void Start()
    {
        force = 17500f;
        EMPGrenade = Resources.Load<GameObject>("Prefabs/Abilities/EMPGrenade");
    }

    protected override void Activate()
    {
        GameObject empGrenadeObject = Instantiate(EMPGrenade, Player.instance.Weapon.FireLocation.transform.position, Quaternion.identity);
        empGrenadeObject.GetComponent<Rigidbody2D>().AddForce(Player.instance.Weapon.FireLocation.transform.right * force);
        activationTimer = 15f;
    }

    // Update is called once per frame
    void Update()
    {
        if (activationTimer > 0f)
        {
            activationTimer -= Time.deltaTime;
        }
    }
}
