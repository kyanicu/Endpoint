using oteTag = GameManager.OneTimeEventTags;

public class HazardSwitch : InteractableEnv
{
    //list of shock floors from the scene. Added manually.
    public ShockFloor[] shockFloors;
    public bool AlreadyPressed { private get; set; }

    private void Awake()
    {
        functionalityText = "disable nearby Shock Floors";
    }

    public override void ActivateFunctionality()
    {
        if (!AlreadyPressed)
        {
            foreach (ShockFloor shockFloor in shockFloors)
            {
                shockFloor.GetComponent<ShockFloor>().enabled = false;
            }
            AlreadyPressed = false;
            //Add this object to one time events that get used on scene load
            GameManager.OneTimeEvents.Add(name, oteTag.HazardSwitch);
        }
    }
}
