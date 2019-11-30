using System;
using System.Collections.Generic;
using LDB = LoadDataBaseEntries;
using LO = LoadObjectives;

[System.Serializable]
public class PlayerData
{
    #region GameManager info
    public string Location;
    public int Scene;
    public int SaveFileID;
    #endregion

    #region Player Info
    public int Health;
    public int MaxHealth;
    public string Class;
    public float[] Position;
    public string ActiveAbilityName;
    public string PassiveAbilityName;
    #endregion

    #region Weapon Info
    public string WeaponName;
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
    public float RateOfFire;
    public float FireTimer;
    public float Range;
    public float BulletVeloc;
    public float ReloadTime;
    public bool ControlledByPlayer;
    #endregion

    #region Objectives & Data Entries
    public List<Tuple<bool, LO.Objective>> PrimaryObjectives;
    public Dictionary<string, List<LO.Objective>> SecondaryObjectives;
    public int currentPrimaryObjective;
    public Dictionary<string, LDB.DataEntry> DatabaseEntries;
    #endregion

    /// <summary>
    /// Constructs datamass to be serialized in save/load system
    /// </summary>
    public PlayerData(Player p)
    {
        //Retrieve GameManager info
        Location = GameManager.Section;
        Scene = (int)GameManager.currentScene;
        SaveFileID = GameManager.SaveFileID;

        //Retrieve Objects/Data Entry info
        PrimaryObjectives = LO.PrimaryObjectives;
        SecondaryObjectives = LO.SecondaryObjectives;
        currentPrimaryObjective = LO.currentPrimaryObjective;
        DatabaseEntries = LDB.Logs;


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
        PassiveAbilityName = p.PassiveAbility.AbilityName;

        //Retrieve Weapon info
        Weapon w = p.Weapon;
        WeaponName = w.Name;
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
        RateOfFire = w.RateOfFire;
        FireTimer = w.FireTimer;
        Range = w.Range;
        BulletVeloc = w.BulletVeloc;
        ReloadTime = w.ReloadTime;
        ControlledByPlayer = true;
    }

}
