using System;
using UnityEngine;
using LDB = LoadDataBaseEntries;
using LO = LoadObjectives;

public class PlayerInitializer : MonoBehaviour
{
    [Tooltip("Drag in from resources")]
    public GameObject Player;

    [Tooltip("Drag in from resources")]
    public GameObject[] Enemies;

    private string[] classes = { "small", "medium", "large" };

    private void Awake()
    {
        if (SaveSystem.loadedData == null)
        {
            GameObject player = Instantiate(Player);
            player.transform.position = transform.position;
            HUDController.instance.UpdateHUD(player.GetComponent<Player>());
        }
        else
        {
            //Update position to loaded position
            transform.position = new Vector3(SaveSystem.loadedData.Position[0], 
                                             SaveSystem.loadedData.Position[1], 
                                             SaveSystem.loadedData.Position[2]);

            //Iterate through each enum
            int classToInstantiate = Array.IndexOf(classes, SaveSystem.loadedData.Class);

            //If a class was correctly loaded
            if(classToInstantiate > -1)
            {
                GameObject player = Instantiate(Enemies[classToInstantiate]);
                player.transform.position = transform.position;

                //Remove enemy component and add a player component
                Destroy(player.GetComponent<Enemy>());
                player.AddComponent<Player>();

                Player playerComponent = player.GetComponent<Player>();
                playerComponent.Class = classes[classToInstantiate];
                playerComponent.Health = SaveSystem.loadedData.Health;
                playerComponent.MaxHealth = SaveSystem.loadedData.MaxHealth;
                playerComponent.MinimapIcon = player.transform.Find("MinimapIcon").gameObject;
                playerComponent.MinimapIcon.GetComponent<SpriteRenderer>().color = Color.cyan;
                player.name = "Player";

                Destroy(playerComponent.ActiveAbility);
                Destroy(playerComponent.PassiveAbility);
                Transform weaponParent = playerComponent.RotationPoint.transform;
                Vector2 weaponPos = playerComponent.Weapon.gameObject.transform.position;
                Destroy(playerComponent.Weapon.gameObject);
                playerComponent.Weapon = null;
                AbilityGenerator.AddAbilitiesToCharacter
                (
                    player,
                    SaveSystem.loadedData.ActiveAbilityName,
                    SaveSystem.loadedData.PassiveAbilityName
                );
                playerComponent.ActiveAbility = player.GetComponent<ActiveAbility>();
                playerComponent.PassiveAbility = player.GetComponent<PassiveAbility>();

                //Retrieve Weapon info
                string vanillaWeaponName = SaveSystem.loadedData.WeaponName;
                int indexer = vanillaWeaponName.IndexOf(" ") + 1;
                vanillaWeaponName = vanillaWeaponName.Substring(indexer);
                GameObject tempWeapon = WeaponGenerator.GenerateWeapon(weaponParent, vanillaWeaponName);
                tempWeapon.name = vanillaWeaponName;
                tempWeapon.transform.position = weaponPos;
                Weapon weaponComponent = tempWeapon.GetComponent<Weapon>();

                playerComponent.Weapon = weaponComponent;
                weaponComponent.Name = SaveSystem.loadedData.WeaponName;
                weaponComponent.Description = SaveSystem.loadedData.Description;
                weaponComponent.IsReloading = false;
                weaponComponent.AmmoInClip = SaveSystem.loadedData.AmmoInClip;
                weaponComponent.SpreadFactor = SaveSystem.loadedData.SpreadFactor;
                weaponComponent.TotalAmmo = SaveSystem.loadedData.TotalAmmo;
                weaponComponent.ClipSize = SaveSystem.loadedData.ClipSize;
                weaponComponent.MaxAmmoCapacity = SaveSystem.loadedData.MaxAmmoCapacity;
                weaponComponent.Damage = SaveSystem.loadedData.Damage;
                weaponComponent.StunTime = SaveSystem.loadedData.StunTime;
                weaponComponent.KnockbackImpulse = SaveSystem.loadedData.KnockbackImpulse;
                weaponComponent.RateOfFire = SaveSystem.loadedData.RateOfFire;
                weaponComponent.FireTimer = SaveSystem.loadedData.FireTimer;
                weaponComponent.Range = SaveSystem.loadedData.Range;
                weaponComponent.BulletVeloc = SaveSystem.loadedData.BulletVeloc;
                weaponComponent.ReloadTime = SaveSystem.loadedData.ReloadTime;
                weaponComponent.ControlledByPlayer = true;

                //Retrieve Objects/Data Entry info
                LO.PrimaryObjectives = SaveSystem.loadedData.PrimaryObjectives;
                LO.SecondaryObjectives = SaveSystem.loadedData.SecondaryObjectives;
                LO.currentPrimaryObjective = SaveSystem.loadedData.currentPrimaryObjective;
                LDB.Logs = SaveSystem.loadedData.DatabaseEntries;

                //Empty saved data cache as confirmation that data was successfully loaded
                SaveSystem.loadedData = null;

                HUDController.instance.UpdateHUD(playerComponent);
                HUDController.instance.UpdateMinimap(GameManager.Section, "Save Room");

            }
        }
    }
}
