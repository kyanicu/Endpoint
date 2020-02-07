using System.Collections;
using UnityEngine;

/// <summary>
/// Controller class that controls enemy behavior
/// </summary>
public class EnemyController : Controller
{
    public static string[] EnemyTypes =
        { "light", "medium", "heavy" };

    protected int moveDirection = +1;
    public float PatrolRange { get; set; }
    public bool PlayerInRange { get; set; }
    public float Speed { get; set; }
    public GameObject[] MovePoints;
    private int bulletsToFire;
    private int maxBulletsToFire;
    private float fireWaitTime;
    private bool finishedFiring;
    private bool disabled { get; set; }

    private static float FREEZE_TIME = 5.0f;

    /// <summary>
    /// Start function for setting delegates and setting the movement points
    /// </summary>
    public override void Start()
    {
        Character.AttackRecievedDeleage = ReceiveAttack;
        Character.TriggerEntered2DDelegate = TriggerEnter2D;
        Character.Weapon.ControlledByPlayer = false;
        maxBulletsToFire = 4;
        fireWaitTime = 2f;
        bulletsToFire = maxBulletsToFire;
        disabled = false;

        // Instantiate left, right movement boundaries
        GameObject left = new GameObject();
        left.name = "Left";
        left.transform.SetParent(transform);
        GameObject right = new GameObject();
        right.name = "right";
        right.transform.SetParent(transform);
        left.transform.position = new Vector3(Character.transform.position.x - PatrolRange, 0, 0);
        right.transform.position = new Vector3(Character.transform.position.x + PatrolRange, 0, 0);
        MovePoints = new GameObject[2];
        MovePoints[0] = left;
        MovePoints[1] = right;
        base.Start();
    }

    /// <summary>
    /// Update function that controls the behavior of the enemy
    /// </summary>
    void Update()
    {
        //Check that player is not in a menu
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY)
            return;

        if (Character.IsSelected)
        {
            UpdateQTEManagerPosition();
        }

        if (!disabled)
        {
            if (IsPlayerInRange())
            {
                PlayerInRange = true;
                Vector3 playerPosition = PlayerController.instance.Character.transform.position;
                Vector3 myPosition = Character.transform.position;
                Vector3 diff = playerPosition - myPosition;
                AimWeapon(Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
                if (bulletsToFire > 0)
                {
                    if (Fire())
                    {
                        bulletsToFire--;
                    }
                }
                else if (bulletsToFire <= 0 && !finishedFiring)
                {
                    StartCoroutine(WaitToFireTimer());
                    finishedFiring = true;
                }

                Move(Vector2.zero);
            }
            else
            {
                PlayerInRange = false;
                Vector2 pos = new Vector2(Character.transform.position.x, 0);
                float Dist0 = Vector2.Distance(pos, MovePoints[0].transform.position);
                float Dist1 = Vector2.Distance(pos, MovePoints[1].transform.position);
                if (Dist1 < .5 || Character.movement.charCont.isTouchingLeftWall)
                    moveDirection = +1;
                else if (Dist0 < .5 || Character.movement.charCont.isTouchingRightWall)
                    moveDirection = -1;

                Move(moveDirection * Vector2.right );
            }
        }
    }

    /// <summary>
    /// Method that finds whether or not the player is in shooting range of the current enemy
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerInRange()
    {
        if (PlayerController.instance.Character != null)
        {
            Vector3 playerPos = PlayerController.instance.Character.transform.position;
            return (Vector3.Distance(playerPos, Character.transform.position) < 20);
        }
        return false;
    }

    /// <summary>
    /// Updates which side of the enemy the QTE panel is relative to the player's position from the enemy
    /// </summary>
    protected void UpdateQTEManagerPosition()
    {
        //Retrieve enemy local scale
        Vector2 EnemyScale = transform.localScale;

        //If enemy is facing to the left, x local scale is negative
        //so swap panel left and right positions
        if (isFacingLeft)
        {
            Character.QTEPanel.transform.localScale = new Vector3(-1, 1, 1);
        }
        //Else reset scale and positions
        else
        {
            Character.QTEPanel.transform.localScale = new Vector3(1, 1, 1);
        }

        //Grab player Position
        Vector2 pos = PlayerController.instance.Character.transform.position;

        //Check player distance from right and left side of enemy
        float playerDistanceToLeftSide = Vector2.Distance(pos, Character.QTEPointLeft.position);
        float playerDistanceToRightSide = Vector2.Distance(pos, Character.QTEPointRight.position);

        //If player is closer to left side of enemy
        if (playerDistanceToLeftSide < playerDistanceToRightSide)
        {
            Character.QTEPanel.transform.position = Character.QTEPointRight.position;

        }
        else //Player is closer to the right side of enemy
        {
            Character.QTEPanel.transform.position = Character.QTEPointLeft.position;
        }
    }

    //Returns whether or not the enemy is facing left
    //Used in UpdateQTEManagerPosition()
    protected bool isFacingLeft
    {
        get
        {
            Vector2 enemyScale = Character.transform.localScale;
            return enemyScale.x < 0;
        }
    }

    /// <summary>
    /// Function that starts the FreezeTimer co-routine when hit by an emp
    /// </summary>
    public void Freeze()
    {
        StartCoroutine(FreezeTimer());
    }

    /// <summary>
    /// function that is called when the enemy character is no longer the hack target
    /// </summary>
    public override void DeselectHackTarget()
    {
        Character.HackArea.GetComponent<RangeFinder>().CancelHack();
        Character.IsSelected = false;
        Character.QTEPanel.SetActive(Character.IsSelected);
        Character.HackArea.SetActive(Character.IsSelected);
    }

    /// <summary>
    /// Function called when the enemy character's health reaches zero
    /// </summary>
    public override void Die()
    {
        // Drop ammo upon death
        if (Character.Weapon.TotalAmmo > 0)
        {
            if (Random.Range(0, 10) % 2 == 0)
            {
                GameObject instantiatedDroppedAmmo = ObjectPooler.instance.SpawnFromPool("DroppedAmmo", Character.transform.position, Quaternion.identity);
                DroppedAmmo droppedAmmo = instantiatedDroppedAmmo.GetComponent<DroppedAmmo>();
                droppedAmmo.Ammo = (Character.Weapon.TotalAmmo < 25) ? 25 : Character.Weapon.TotalAmmo;
            }
        }
        Destroy(Character.gameObject);
        Destroy(this);
    }

    /// <summary>
    /// Delegate that will be called when the enemy character enters a 2d trigger
    /// </summary>
    /// <param name="collision"></param>
    public override void TriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HackProjectile"))
        {
            if (PlayerController.instance.Enemy == null)
            {
                Character.IsSelected = true;
                Character.QTEPanel.SetActive(Character.IsSelected);
                Character.HackArea.SetActive(Character.IsSelected);
                collision.gameObject.SetActive(false);
            }
        }
        else if (collision.CompareTag("EMP"))
        {
            Freeze();
        }
    }

    /// <summary>
    /// Coroutine that will wait to reset the enemy's ability to shoot
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitToFireTimer()
    {
        yield return new WaitForSeconds(fireWaitTime);
        bulletsToFire = maxBulletsToFire;
        finishedFiring = false;
    }

    /// <summary>
    /// Coroutine that simulates the action of being frozen by an EMP
    /// </summary>
    /// <returns></returns>
    private IEnumerator FreezeTimer()
    {
        disabled = true;
        yield return new WaitForSeconds(FREEZE_TIME);
        disabled = false;
    }
}
