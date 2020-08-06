using UnityEngine;

/// <summary>
/// Class to generate abilities on characters
/// </summary>
public static class AbilityGenerator
{
    /// <summary>
    /// Method to add abilities to a character game object
    /// </summary>
    /// <param name="character">The character getting the abilities</param>
    public static void AddAbilitiesToCharacter(GameObject character)
    {
        int activeAbilityIndex = Random.Range(0, ActiveAbility.ActiveAbilityList.Count);

        switch (ActiveAbility.ActiveAbilityList[activeAbilityIndex])
        {
            case "Homing Bullet":
                character.AddComponent<HomingBulletAbility>();
                break;
            case "EMP Grenade":
                character.AddComponent<EMPGrenadeAbility>();
                break;
            case "DashAttack":
                character.AddComponent<DashAttack>();
                break;
        }

        character.GetComponent<Character>().ActiveAbility = character.GetComponent<ActiveAbility>();
        //character.GetComponent<Character>().PassiveAbility = character.GetComponent<PassiveAbility>();
    }
    /// <summary>
    /// Method to add abilities to a character game object
    /// </summary>
    /// <param name="character">The character getting the abilities</param>
    public static void AddAbilitiesToCharacter(Character player, string activeAbility, string passiveAbility)
    {
        switch (activeAbility)
        {
            case "Homing Bullet":
                player.gameObject.AddComponent<HomingBulletAbility>();
                break;
            case "EMP Grenade":
                player.gameObject.AddComponent<EMPGrenadeAbility>();
                break;
        }

        player.ActiveAbility = player.GetComponent<ActiveAbility>();
        //player.PassiveAbility = player.GetComponent<PassiveAbility>();
        player.ActiveAbility.resetOwner(player);
        //player.PassiveAbility.resetOwner(player);
    }
}
