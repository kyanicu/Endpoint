using System;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [Tooltip("Drag in from resources")]
    public GameObject PlayerObj;

    [Tooltip("Drag in from resources")]
    public GameObject PlayerControllerObj;

    [Tooltip("Drag in from resources")]
    public GameObject[] Enemies;

    private string[] classes = { "light", "medium", "heavy" };

    private void Awake()
    {
        //Check that there's not already a player in scene [DontDestroyOnLoad]
        if (PlayerController.instance != null)
        {
            Destroy(gameObject);
        }
        GameObject playerController = Instantiate(PlayerControllerObj);
        //If scene is not being loaded from a save file
        if (SaveSystem.loadedData == null)
        {
            //Spawn in the default player
            GameObject player = Instantiate(PlayerObj);
            player.transform.position = transform.position;
            player.name = "Player";
            player.tag = "Player";
            PlayerController.instance.Character = player.GetComponent<Character>();
            ExperienceSystem.instance.StartExperienceSystem();
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
                PlayerController.instance.Character = player.GetComponent<Character>();
                PlayerController.instance.Character.WorldspaceCanvas.gameObject.SetActive(true);
                WorldspaceCanvasManager WSCanvasManager = 
                    PlayerController.instance.Character.WorldspaceCanvas.GetComponent<WorldspaceCanvasManager>();

                WSCanvasManager.InitializeAsPlayerCanvas(PlayerController.instance.Character);
                WSCanvasManager.UpdatePlayerAmmo();

                #region Load Player Stats
                PlayerController.instance.Character.Class = classes[classToInstantiate];
                PlayerController.instance.Character.Health = SaveSystem.loadedData.Health;
                PlayerController.instance.Character.MaxHealth = SaveSystem.loadedData.MaxHealth;
                PlayerController.instance.Character.MinimapIcon = player.transform.Find("MinimapIcon").gameObject;
                PlayerController.instance.Character.MinimapIcon.GetComponent<SpriteRenderer>().color = Color.cyan;
                player.name = "Player";
                player.tag = "Player";

                ExperienceSystem.instance.level = SaveSystem.loadedData.level;
                ExperienceSystem.instance.experience = SaveSystem.loadedData.experience;
                ExperienceSystem.instance.totalExperience = SaveSystem.loadedData.totalExperience;
                ExperienceSystem.instance.nextLevelExperience = SaveSystem.loadedData.nextLevelExperience;
                ExperienceSystem.instance.totalNextLevelExperience = SaveSystem.loadedData.totalNextLevelExperience;
                #endregion

                Transform weaponParent = PlayerController.instance.Character.RotationPoint.transform.GetChild(0);
                Vector2 weaponPos = PlayerController.instance.Character.Weapon.gameObject.transform.position;
                Destroy(PlayerController.instance.Character.Weapon.gameObject);
                PlayerController.instance.Character.Weapon = null;

                //Remove player's current ability components
                Destroy(PlayerController.instance.Character.ActiveAbility);
                //Destroy(PlayerController.instance.Character.PassiveAbility);

                //Load the player's saved abilities and attach components to player
                AbilityGenerator.AddAbilitiesToCharacter
                (
                    PlayerController.instance.Character,
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
                PlayerController.instance.Character.Weapon = weaponComponent;

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
                weaponComponent.KnockbackTime = SaveSystem.loadedData.KnockbackTime;
                weaponComponent.RateOfFire = SaveSystem.loadedData.RateOfFire;
                weaponComponent.FireTimer = SaveSystem.loadedData.FireTimer;
                weaponComponent.Range = SaveSystem.loadedData.Range;
                weaponComponent.BulletVeloc = SaveSystem.loadedData.BulletVeloc;
                weaponComponent.ReloadTime = SaveSystem.loadedData.ReloadTime;
                weaponComponent.ControlledByPlayer = true;
                #endregion 

                #endregion
            }

            //Empty saved data cache as confirmation that data was successfully loaded
            SaveSystem.loadedData = null;

            HUDController.instance.UpdateHUD(PlayerController.instance.Character);
        }
    }
}
