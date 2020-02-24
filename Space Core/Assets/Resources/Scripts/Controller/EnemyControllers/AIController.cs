using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller used to control the AI with more complex behavior
/// </summary>
public abstract class AIController : Controller
{
    //define enums for state and enemy types
    #region EnumsAndTypes
    public enum AIState { Idle, Attacking, AttackOutOfRange}
    public static string[] EnemyTypes = { "light", "medium", "heavy" };
    #endregion

    //Various constants that are used within the code.
    #region Constants
    protected const float PLAYER_RANGE = 15.0f;
    protected const float DISTANCE_BETWEEN_ENEMIES = 2.0f;
    protected const float FREEZE_TIME = 5.0f;
    protected const float PLAYER_ESCAPE_RANGE = 22.5f;
    protected const float MAX_SEARCH_MOVE_TIME = 3.0f;
    protected const float MAX_PAUSE_TIME = 2.0f;
    protected const float MOVE_FORWARD_DISTANCE = 5f;
    protected const int ATTACK_SPEED_MOD = 2;
    #endregion

    //Variables used within the AI Controller
    #region ClassVariables

    public float BaseRunMax { get; set; }
    protected Vector2 playerPos { get { return PlayerController.instance.Character.transform.position; } }
    protected Vector2 lastPlayerPosition;
    protected int bulletsToFire;
    protected int maxBulletsToFire;
    protected short searchDirection;
    protected float fireWaitTime;
    protected float searchMoveTimer;
    protected float pauseTimer;
    protected bool finishedFiring;
    protected bool disabled;
    protected bool canMakeJump;
    protected bool madeJump;
    protected AIState currentState;
    #endregion

    #region UnityFunctions
    // Start is called before the first frame update
    public override void Start()
    {
        searchDirection = 0;
        pauseTimer = MAX_PAUSE_TIME;
        searchMoveTimer = MAX_SEARCH_MOVE_TIME;
        currentState = AIState.Idle;
        Character.AttackRecievedDeleage = ReceiveAttack;
        Character.TriggerEntered2DDelegate = TriggerEnter2D;
        Character.Weapon.ControlledByPlayer = false;
        maxBulletsToFire = 4;
        fireWaitTime = 2f;
        bulletsToFire = maxBulletsToFire;
        disabled = false;
        base.Start();
        BaseRunMax = Character.movement.runMax;
    }

    // Update is called once per frame
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
            //if the player is in range, switch to attack mode
            if (IsPlayerInRange())
            {
                currentState = AIState.Attacking;
            }
            //if we are attacking and the player leaves AI range, change state to searching
            else if (currentState == AIState.Attacking && Math.Abs(Vector3.Distance(playerPos, Character.transform.position)) < PLAYER_ESCAPE_RANGE)
            {
                currentState = AIState.AttackOutOfRange;
            }
            else if (Math.Abs(Vector3.Distance(playerPos, Character.transform.position)) > PLAYER_ESCAPE_RANGE)
            {
                currentState = AIState.Idle;
                Character.SetAnimationState(Character.AnimationState.idle);
            }

            switch (currentState)
            {
                case AIState.Attacking:
                case AIState.AttackOutOfRange:
                    Attack();
                    break;
                
            }
        }
    }
    #endregion

    #region AttackBehavior
    /// <summary>
    /// Method used to control the attack behavior of the AI
    /// </summary>
    protected abstract void Attack();

    /// <summary>
    /// Default method for aiming at the weapon
    /// </summary>
    protected void AimWeapon()
    {
        Vector3 playerPosition = playerPos;
        Vector3 myPosition = Character.transform.position;
        Vector3 diff = playerPosition - myPosition;
        AimWeapon(Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
    }

    /// <summary>
    /// Movement function to control movement of the AI while the AI is attacking
    /// </summary>
    /// <param name="factor">Factor that controls the range the AI needs to be in range</param>
    /// <param name="speedMod">modifier for the AI speed while AI is moving away from the player</param>
    protected void AttackMove(float factor, float speedMod)
    {
        short directionTowardsObject = GetMovementDirection(playerPos.x);
            
        if (currentState == AIState.Attacking && Math.Abs(Vector3.Distance(playerPos, Character.transform.position)) < PLAYER_RANGE * factor)
        {
            if (BaseRunMax - Character.movement.runMax < .5f)
            {
                Character.movement.runMax = Character.movement.runMax / speedMod;
            }
            Move(directionTowardsObject * -1 * Vector2.right);
        }
        else if (currentState == AIState.AttackOutOfRange ||
                Math.Abs(Vector3.Distance(playerPos, Character.transform.position)) > (PLAYER_RANGE * factor) + MOVE_FORWARD_DISTANCE)
        {
            if (BaseRunMax != Character.movement.runMax)
            {
                Character.movement.runMax = Character.movement.runMax = BaseRunMax;
            }
            Move(directionTowardsObject * Vector2.right);
        }
        else
        {
            Move(Vector2.zero);
            Character.movement.runMax = BaseRunMax;
        }
        
    }

    /// <summary>
    /// Behavior for enemies shooting at the player
    /// </summary>
    protected void FireAtPlayer()
    {
        AimWeapon();

        if (bulletsToFire > 0 && IsPlayerInRange())
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
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Gets the direction to move towards another X location
    /// </summary>
    /// <param name="otherX">The x we are comparing the character's position to</param>
    /// <returns></returns>
    protected short GetMovementDirection(float otherX)
    {
        if (Character.transform.position.x > otherX)
        {
            return -1;
        }
        else
        {
            return 1;
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

        //Check player distance from right and left side of enemy
        float playerDistanceToLeftSide = Vector2.Distance(playerPos, Character.QTEPointLeft.position);
        float playerDistanceToRightSide = Vector2.Distance(playerPos, Character.QTEPointRight.position);

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


    /// <summary>
    /// Gets the initial velocity of the character
    /// </summary>
    /// <param name="direction">Direction the character is moving in</param>
    /// <returns>Returns the inital velocity vector</returns>
    protected Vector2 GetInitialVelocity(float direction)
    {
        return Character.movement.jumpVelocity * Vector2.up + Character.movement.runMax * direction * Vector2.right;
    }

    /// <summary>
    /// Gets the position at which the enemy will jump to at time t with the initial velocity
    /// </summary>
    /// <param name="t">the time in the equation</param>
    /// <param name="initialVelocity">initial velocity of the character</param>
    /// <returns>Returns the X Y coordinate where the character will be at when attempting the jump</returns>
    protected Vector2 MaxJumpPositionAtT(float t, Vector2 initialVelocity)
    {
        return (Vector2)Character.transform.position + (initialVelocity * t + 0.5f * Physics2D.gravity * t * t);
    }

    /// <summary>
    /// Finds the time it will take for us to get to X during a jump
    /// </summary>
    /// <param name="x">the target x value</param>
    /// <param name="initialVelocity">initial velocity of the character</param>
    /// <returns>returns the time at which the character will get to x</returns>
    protected float JumpTAtX(float x, Vector2 initialVelocity)
    {
        return (x / initialVelocity.x);
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
    /// Method that finds whether or not the player is in shooting range of the current enemy
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerInRange()
    {
        if (PlayerController.instance.Character != null && !PlayerController.instance.Undetectable)
        {
            return (Vector3.Distance(playerPos, Character.transform.position) < PLAYER_RANGE);
        }
        return false;
    }
    #endregion

    #region DelegatesAndCoroutines
    /// <summary>
    /// Function called when the enemy character's health reaches zero
    /// </summary>
    public override void Die()
    {
        // Drop ammo upon death
        if (Character.Weapon.TotalAmmo > 0)
        {
            if (UnityEngine.Random.Range(0, 10) % 2 == 0)
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
                PlayerController.instance.Enemy = this;

                if (PlayerController.instance.HealOnHack)
                {
                    HealCharacter(FortificationChipset.HealValue);
                }

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
    protected IEnumerator WaitToFireTimer()
    {
        yield return new WaitForSeconds(fireWaitTime);
        bulletsToFire = maxBulletsToFire;
        finishedFiring = false;
    }

    /// <summary>
    /// Coroutine that simulates the action of being frozen by an EMP
    /// </summary>
    /// <returns></returns>
    protected IEnumerator FreezeTimer()
    {
        disabled = true;
        yield return new WaitForSeconds(FREEZE_TIME);
        disabled = false;
    }
    #endregion
}