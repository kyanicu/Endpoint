using System;
using System.Collections.Generic;
using LDB = LoadDataBaseEntries;
using LO = LoadObjectives;
using LD = LoadDialogue;
using oteTag = GameManager.OneTimeEventTags;

[System.Serializable]
public class PlayerData
{
    #region GameManager info
    public string Sector;
    public int Scene;
    public int SaveFileID;
    public float PlayerTimer;
    public int PlayerLevel;
    public Dictionary<string, oteTag> OneTimeEventsList;
    #endregion

    #region Player Info
    public float Health;
    public float MaxHealth;
    public string Class;
    public float[] Position;
    public string ActiveAbilityName;
    public string PassiveAbilityName;
    #endregion

    #region Weapon Info
    public string WeaponName;
    public string WeaponFullName;
    public string Description;
    public bool IsReloading;
    public int AmmoInClip;
    public float SpreadFactor;
    public int TotalAmmo;
    public int ClipSize;
    public int MaxAmmoCapacity;
    public int Damage;
    public float StunTime;
    public float KnockbackImpulse;
    public float KnockbackTime;
    public float RateOfFire;
    public float FireTimer;
    public float Range;
    public float BulletVeloc;
    public float ReloadTime;
    public bool ControlledByPlayer;
    #endregion

    #region Static Loaded Items
    public List<Tuple<bool, LO.Objective>> PrimaryObjectives;
    public Dictionary<string, List<LO.Objective>> SecondaryObjectives;
    public int currentPrimaryObjective;
    public Dictionary<string, LDB.DataEntry> DatabaseEntries;
    public Dictionary<string, LD.DialogueItem> DialogueItems;
    #endregion

    /// <summary>
    /// Constructs datamass to be serialized in save/load system
    /// </summary>
    public PlayerData(Character p)
    {
        //Retrieve GameManager info
        Sector = GameManager.Sector;
        Scene = (int)GameManager.currentScene;
        SaveFileID = GameManager.SaveFileID;
        OneTimeEventsList = GameManager.OneTimeEvents;
        PlayerTimer = GameManager.Timer;
        PlayerLevel = GameManager.PlayerLevel;

        //Retrieve Objects/Data Entry info
        PrimaryObjectives = LO.PrimaryObjectives;
        SecondaryObjectives = LO.SecondaryObjectives;
        currentPrimaryObjective = LO.currentPrimaryObjective;
        DatabaseEntries = LDB.Logs;
        DialogueItems = LD.DialogueItems;

        //Retrieve Player position
        Position = new float[3];
        Position[0] = p.transform.position.x;
        Position[1] = p.transform.position.y;
        Position[2] = p.transform.position.z;        

        //Retrieve Player info
        Health = p.Health;
        MaxHealth = p.MaxHealth;
        Class = p.Class;
        ActiveAbilityName = p.ActiveAbility.AbilityName;
        //PassiveAbilityName = p.PassiveAbility.AbilityName;

        //Retrieve Weapon info
        Weapon w = p.Weapon;
        WeaponName = w.Name;
        WeaponFullName = w.FullName;
        Description = w.Description;
        IsReloading = w.IsReloading;
        AmmoInClip = w.AmmoInClip;
        SpreadFactor = w.SpreadFactor;
        TotalAmmo = w.TotalAmmo;
        ClipSize = w.ClipSize;
        MaxAmmoCapacity = w.MaxAmmoCapacity;
        Damage = w.Damage;
        StunTime = w.StunTime;
        KnockbackImpulse = w.KnockbackImpulse;
        KnockbackTime = w.KnockbackTime;
        RateOfFire = w.RateOfFire;
        FireTimer = w.FireTimer;
        Range = w.Range;
        BulletVeloc = w.BulletVeloc;
        ReloadTime = w.ReloadTime;
        ControlledByPlayer = true;
    }

}
