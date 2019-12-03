using System;
using UnityEngine;
using LDB = LoadDataBaseEntries;
using LO = LoadObjectives;
using LD = LoadDialogue;

public class PlayerInitializer : MonoBehaviour
{
    [Tooltip("Drag in from resources")]
    public GameObject PlayerObj;

    [Tooltip("Drag in from resources")]
    public GameObject[] Enemies;

    private string[] classes = { "small", "medium", "large" };

    private void Awake()
    {
        //Check that there's not already a player in scene [DontDestroyOnLoad]
        if (Player.instance != null)
        {
            Destroy(gameObject);
        }
        //If scene is not being loaded from a save file
        if (SaveSystem.loadedData == null)
        {
            //Spawn in the default player
            GameObject player = Instantiate(PlayerObj);
            player.transform.position = transform.position;
        }
        //Begin operation for loading a player from a save file
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
                #region Load Player
                GameObject player = Instantiate(Enemies[classToInstantiate]);
                player.transform.position = transform.position;

                //Remove enemy component and add a player component
                Destroy(player.GetComponent<Enemy>());
                player.AddComponent<Player>();
                Player playerComponent = player.GetComponent<Player>();

                #region Load Player Stats
                playerComponent.Class = classes[classToInstantiate];
                playerComponent.Health = SaveSystem.loadedData.Health;
                playerComponent.MaxHealth = SaveSystem.loadedData.MaxHealth;
                playerComponent.MinimapIcon = player.transform.Find("MinimapIcon").gameObject;
                playerComponent.MinimapIcon.GetComponent<SpriteRenderer>().color = Color.cyan;
                player.name = "Player";
                player.tag = "Player";
                #endregion

                Transform weaponParent = playerComponent.RotationPoint.transform.GetChild(0);
                Vector2 weaponPos = playerComponent.Weapon.gameObject.transform.position;
                Destroy(playerComponent.Weapon.gameObject);
                playerComponent.Weapon = null;

                //Remove player's current ability components
                Destroy(playerComponent.ActiveAbility);
                Destroy(playerComponent.PassiveAbility);

                //Load the player's saved abilities and attach components to player
                AbilityGenerator.AddAbilitiesToCharacter
                (
                    playerComponent,
                    SaveSystem.loadedData.ActiveAbilityName,
                    SaveSystem.loadedData.PassiveAbilityName
                );
                #endregion

                #region Load Weapon info
                //Retrieve orignal weapon's name (need to remove prefix for weapon load)
                string weaponName = SaveSystem.loadedData.WeaponName;
                //Generate a weapon of the same type (stats added later)
                GameObject tempWeapon = WeaponGenerator.GenerateWeapon(weaponParent, weaponName);
                tempWeapon.name = weaponName;
                tempWeapon.transform.position = weaponPos;

                //Set player's weapon to loaded weapon
                Weapon weaponComponent = tempWeapon.GetComponent<Weapon>();
                playerComponent.Weapon = weaponComponent;

                #region Load Weapon Stats
                weaponComponent.Name = SaveSystem.loadedData.WeaponName;
                weaponComponent.FullName = SaveSystem.loadedData.WeaponFullName;
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
                #endregion 

                #endregion

                //Load player's Objectives progress
                LO.PrimaryObjectives = SaveSystem.loadedData.PrimaryObjectives;
                LO.SecondaryObjectives = SaveSystem.loadedData.SecondaryObjectives;
                LO.currentPrimaryObjective = SaveSystem.loadedData.currentPrimaryObjective;

                //Load player's unlocked database entries
                LDB.Logs = SaveSystem.loadedData.DatabaseEntries;

                LD.DialogueItems = SaveSystem.loadedData.DialogueItems;
            }
        }
    }
}
