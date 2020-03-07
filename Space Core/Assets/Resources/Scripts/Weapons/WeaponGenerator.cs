using UnityEngine;

/// <summary>
/// This class will generate a random weapon
/// </summary>
public static class WeaponGenerator
{
    //Used for generating max ammo capacity
    public const int MIN_RNG = 3;
    public const int MAX_RNG = 5;

    /// <summary>
    /// Main function to generate a weapon
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject GenerateWeapon(Transform parent, string weaponName)
    {
        //Create empty object to load generated weapon into
        GameObject weapon = null;

        //Create empty string to load generated prefix into
        string prefix = "";

        //Generate a weaon specified by its name
        switch(weaponName)
        {
            case "Rotary Repeater":
                //Load generated RotaryRepeater stats onto weapon
                WeaponGenerationInfo newRotaryRepeater = new RotaryRepeaterInfo();
                weapon = BuildAutomatic(parent, newRotaryRepeater);
                prefix = newRotaryRepeater.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            case "Vortex Launcher":
                //Load generated Vortex Launcher stats onto weapon
                WeaponGenerationInfo newVortexLauncher = new VortexLauncherInfo();
                weapon = BuildAutomatic(parent, newVortexLauncher);
                prefix = newVortexLauncher.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            case "Pulse Projector":
                //Load generated Pulse Projector stats onto weapon
                WeaponGenerationInfo newPulseProjector = new PulseProjectorInfo();
                weapon = BuildSpread(parent, newPulseProjector);
                prefix = newPulseProjector.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            case "Gauss Cannon":
                //Load generated SnipeyBoi stats onto weapon
                WeaponGenerationInfo newGaussCannon = new GaussCannonInfo();
                weapon = BuildPrecision(parent, newGaussCannon);
                prefix = newGaussCannon.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            case "Breach Missile":
                //Load generated RawketLawnchair stats onto weapon
                WeaponGenerationInfo newBreachMissile = new BreachMissileInfo();
                weapon = BuildPrecision(parent, newBreachMissile);
                prefix = newBreachMissile.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            case "Shock Lance":
                //Load generated Vohnemet stats onto weapon
                WeaponGenerationInfo newShockLance = new ShockLanceInfo();
                weapon = BuildPrecision(parent, newShockLance);
                prefix = newShockLance.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            default:
                return weapon;
        }

        //Apply the newly generated prefix to the weapon name
        Weapon wep = weapon.GetComponent<Weapon>();
        weapon.name = wep.Name;
        return weapon;
    }

    /// <summary>
    /// Main function to generate a weapon
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject GenerateWeapon(Transform parent)
    {
        //parse into a weapon type
        int rngVal = UnityEngine.Random.Range(0, Weapon.WeaponsList.Count);

        //Load the randomly picked weapon's name
        string weaponName = Weapon.WeaponsList[rngVal];

        //Create empty object to load generated weapon into
        GameObject weapon = null;

       //Create empty string to load generated prefix into
        string prefix = "";

        //Generate a weaon specified by its name
        switch(weaponName)
        {
            case "Rotary Repeater":
                //Load generated RotaryRepeater stats onto weapon
                WeaponGenerationInfo newRotaryRepeater = new RotaryRepeaterInfo();
                weapon = BuildAutomatic(parent, newRotaryRepeater);
                prefix = newRotaryRepeater.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            case "Vortex Launcher":
                //Load generated Vortex Launcher stats onto weapon
                WeaponGenerationInfo newVortexLauncher = new VortexLauncherInfo();
                weapon = BuildBurstFire(parent, newVortexLauncher);
                prefix = newVortexLauncher.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            case "Pulse Projector":
                //Load generated Pulse Projector stats onto weapon
                WeaponGenerationInfo newPulseProjector = new PulseProjectorInfo();
                weapon = BuildSpread(parent, newPulseProjector);
                prefix = newPulseProjector.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            case "Gauss Cannon":
                //Load generated SnipeyBoi stats onto weapon
                WeaponGenerationInfo newGaussCannon = new GaussCannonInfo();
                weapon = BuildPrecision(parent, newGaussCannon);
                prefix = newGaussCannon.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            case "Breach Missile":
                //Load generated RawketLawnchair stats onto weapon
                WeaponGenerationInfo newBreachMissile = new BreachMissileInfo();
                weapon = BuildPrecision(parent, newBreachMissile);
                prefix = newBreachMissile.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            case "Shock Lance":
                //Load generated Vohnemet stats onto weapon
                WeaponGenerationInfo newShockLance = new ShockLanceInfo();
                weapon = BuildPrecision(parent, newShockLance);
                prefix = newShockLance.GenerateNewWeaponName(weapon.GetComponent<Weapon>());
                break;

            default:
                return weapon;
        }

        //Apply the newly generated prefix to the weapon name
        Weapon wep = weapon.GetComponent<Weapon>();
        wep.FullName = prefix + weaponName;
        wep.Name = weaponName;
        weapon.name = wep.Name;
        return weapon;
    }

    /// <summary>
    /// This function will generate a specified Automatic Weapon with random stats
    /// </summary>
    /// <param name="parent">Transform of the weapons parent object</param>
    /// <returns>new weapon gameobject</returns>
    private static GameObject BuildAutomatic(Transform parent, WeaponGenerationInfo wgi)
    {
        return SetCommonAttributes(parent, wgi);
    }

    /// <summary>
    /// This function will generate a specified Burst Fire Weapon with random stats
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="wgi"></param>
    /// <returns></returns>
    private static GameObject BuildBurstFire(Transform parent, WeaponGenerationInfo wgi)
    {
        return SetCommonAttributes(parent, wgi);
    }

    /// <summary>
    /// This function will generate a specified Spread Weapon with random stats
    /// </summary>
    /// <param name="parent">Transform of the weapons parent object</param>
    /// <returns>new weapon gameobject</returns>
    private static GameObject BuildSpread(Transform parent, WeaponGenerationInfo wgi)
    {

        return SetCommonAttributes(parent, wgi);
    }

    private static GameObject BuildHeavy(Transform parent, WeaponGenerationInfo wgi)
    {
        return SetCommonAttributes(parent, wgi);
    }

    private static GameObject BuildEnergy(Transform parent, WeaponGenerationInfo wgi)
    {
        return SetCommonAttributes(parent, wgi);
    }

    /// <summary>
    /// This function will generate a specified Precision Weapon with random stats
    /// </summary>
    /// <param name="parent">Transform of the weapons parent object</param>
    /// <returns>new weapon gameobject</returns>
    private static GameObject BuildPrecision(Transform parent, WeaponGenerationInfo wgi)
    {
        return SetCommonAttributes(parent, wgi);
    }

    /// <summary>
    /// This function will generate a specific weapon with random stats
    /// </summary>
    /// <param name="parent">Transform of the weapons parent object</param>
    /// <param name="wgi">Weapon generation information</param>
    /// <returns>new weapon gameobject</returns>
    private static GameObject SetCommonAttributes(Transform parent, WeaponGenerationInfo wgi)
    {
        GameObject weaponResource = Resources.Load<GameObject>("Prefabs/Weapons/" + wgi.name);
        GameObject weaponObject = GameObject.Instantiate(weaponResource, parent);
        Weapon weapon = weaponObject.GetComponent<Weapon>();
        weapon.BulletTag = wgi.BulletTag;
        weapon.SpreadFactor = UnityEngine.Random.Range(wgi.MinSpread, wgi.MaxSpread);
        weapon.Damage = UnityEngine.Random.Range(wgi.MinDamage, wgi.MaxDamage);
        weapon.StunTime = wgi.StunTime;
        weapon.KnockbackImpulse = wgi.MaxKnockbackImpulse;
        weapon.KnockbackTime = wgi.MaxKnockbackTime; ;
        weapon.ClipSize = UnityEngine.Random.Range(wgi.MinClipSize, wgi.MaxClipSize);
        weapon.AmmoInClip = weapon.ClipSize;
        weapon.MaxAmmoCapacity = UnityEngine.Random.Range(weapon.ClipSize * MIN_RNG, weapon.ClipSize * MAX_RNG);
        weapon.RateOfFire = UnityEngine.Random.Range(wgi.MinRateOfFire, wgi.MaxRateOfFire);
        weapon.ReloadTime = UnityEngine.Random.Range(wgi.MinReloadTime, wgi.MaxReloadTime);
        weapon.Range = UnityEngine.Random.Range(wgi.MinRange, wgi.MaxRange);
        weapon.BulletVeloc = UnityEngine.Random.Range(wgi.MinBulletVeloc, wgi.MaxBulletVeloc);
        weapon.TotalAmmo = weapon.MaxAmmoCapacity;
        weapon.Description = wgi.description;
        return weaponObject;
    }
}
