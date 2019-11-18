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
        int passiveAbilityIndex = Random.Range(0, PassiveAbility.PassiveAbilityList.Count);
        int activeAbilityIndex = Random.Range(0, ActiveAbility.ActiveAbilityList.Count);

        switch (PassiveAbility.PassiveAbilityList[passiveAbilityIndex])
        {
            case "Overclock":
                character.AddComponent<Overclock>();
                break;
            case "ImmortalReloading":
                character.AddComponent<ImmortalReloading>();
                break;
            case "PiercingShotAbility":
                character.AddComponent<PiercingShotAbility>();
                break;
        }

        switch (ActiveAbility.ActiveAbilityList[activeAbilityIndex])
        {
            case "HomingBulletAbility":
                character.AddComponent<HomingBulletAbility>();
                break;
            case "EMPGrenadeAbility":
                character.AddComponent<EMPGrenadeAbility>();
                break;
        }

        character.GetComponent<Character>().ActiveAbility = character.GetComponent<ActiveAbility>();
        character.GetComponent<Character>().PassiveAbility = character.GetComponent<PassiveAbility>();
    }
}
