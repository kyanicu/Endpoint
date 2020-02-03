using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller used to control the AI with more complex behavior
/// </summary>
public class AIController : Controller
{
    //define enums for state and enemy types
    #region EnumsAndTypes
    public enum AIState { Patroling, Attacking, Searching }
    public static string[] EnemyTypes = { "light", "medium", "heavy" };
    #endregion

    //Various constants that are used within the code.
    #region Constants
    private const float PLAYER_RANGE = 15.0f;
    private const float DISTANCE_BETWEEN_ENEMIES = 2.0f;
    private const float FREEZE_TIME = 5.0f;
    private const float SEARCH_RADIUS = 10.0f;
    private const float MAX_SEARCH_MOVE_TIME = 3.0f;
    private const float MAX_PAUSE_TIME = 2.0f;
    private const int ATTACK_SPEED_MOD = 2;
    #endregion

    //Variables used within the AI Controller
    #region ClassVariables
    public List<Node> RoomGraph;
    public int CurrentNode { get; set; }
    public int PreviousNode { get; set; }
    public Node.PathType CurrentPathType { get; set; }
    public float SearchTime = 5.0f;
    public float BaseRunMax;
    private Vector2 playerPos { get { return PlayerController.instance.Character.transform.position; } }
    private Vector2 lastPlayerPosition;
    private int bulletsToFire;
    private int maxBulletsToFire;
    private short searchDirection;
    private float fireWaitTime;
    private float searchMoveTimer;
    private float pauseTimer;
    private bool finishedFiring;
    private bool disabled;
    private bool canMakeJump;
    private bool madeJump;
    private AIState currentState;
    #endregion

    #region UnityFunctions
    // Start is called before the first frame update
    public override void Start()
    {
        searchDirection = 0;
        pauseTimer = MAX_PAUSE_TIME;
        searchMoveTimer = MAX_SEARCH_MOVE_TIME;
        currentState = AIState.Patroling;
        Character.AttackRecievedDeleage = ReceiveAttack;
        Character.TriggerEntered2DDelegate = TriggerEnter2D;
        Character.Weapon.ControlledByPlayer = false;
        maxBulletsToFire = 4;
        fireWaitTime = 2f;
        bulletsToFire = maxBulletsToFire;
        disabled = false;
        SetStart();
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
                if (currentState == AIState.Searching)
                {
                    StopCoroutine(StartSearch());
                }
                currentState = AIState.Attacking;
            }
            //if we are attacking and the player leaves AI range, change state to searching
            else if (currentState == AIState.Attacking)
            {
                lastPlayerPosition = PlayerController.instance.Character.transform.position;
                currentState = AIState.Searching;
                StartCoroutine(StartSearch());
            }

            switch (currentState)
            {
                case AIState.Patroling:
                    Patrol();
                    break;
                case AIState.Attacking:
                    Attack();
                    break;
                case AIState.Searching:
                    Search();
                    break;
            }
        }
    }
    #endregion

    #region AttackBehavior
    /// <summary>
    /// Method used to control the attack behavior of the AI
    /// </summary>
    private void Attack()
    {
        Vector3 playerPosition = PlayerController.instance.Character.transform.position;
        Vector3 myPosition = Character.transform.position;
        Vector3 diff = playerPosition - myPosition;
        AimWeapon(Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);

        switch (Character.Weapon.Type)
        {
            case Weapon.WeaponType.Automatic:
                AutomaticAttackBehavior();
                break;
            case Weapon.WeaponType.Scatter:
                ScatterAttackBehavior();
                break;
            case Weapon.WeaponType.Precision:
                PrecisionAttackBehavior();
                break;
        }
    }

    /// <summary>
    /// Movement function to control movement of the AI while the AI is attacking
    /// </summary>
    /// <param name="factor">Factor that controls the range the AI needs to be in range</param>
    /// <param name="speedMod">modifier for the AI speed while AI is moving away from the player</param>
    private void AttackMove(float factor, float speedMod)
    {
        bool previous = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = true;
        RaycastHit2D hit;

        if (!Character.lookingLeft)
        {
            hit = Physics2D.Raycast(Character.Weapon.FireLocation.transform.position, transform.right);
        }
        else
        {
            hit = Physics2D.Raycast(Character.Weapon.FireLocation.transform.position, transform.right * -1);
        }

        Physics2D.queriesHitTriggers = previous;

        short directionTowardsObject = 0;

        if (hit && hit.collider != null && hit.collider.gameObject != null && hit.collider.gameObject.tag == "Enemy")
        {
            directionTowardsObject = GetMovementDirection(hit.collider.gameObject.transform.position.x);
            if (Math.Abs(Vector3.Distance(playerPos, hit.collider.gameObject.transform.position)) > DISTANCE_BETWEEN_ENEMIES * factor)
            {
                Move(directionTowardsObject * -1);
            }
            else
            {
                Move(directionTowardsObject);
            }
        }
        else
        {
            directionTowardsObject = GetMovementDirection(PlayerController.instance.Character.transform.position.x);
            
            if (Math.Abs(Vector3.Distance(playerPos, Character.transform.position)) > PLAYER_RANGE * factor)
            {
                if (BaseRunMax - Character.movement.runMax > .5f)
                {
                    Character.movement.runMax = BaseRunMax;
                }
                Move(directionTowardsObject);
            }
            else if (Math.Abs(Vector3.Distance(playerPos, Character.transform.position)) < PLAYER_RANGE * factor)
            {
                if (BaseRunMax - Character.movement.runMax < .5f)
                {
                    Character.movement.runMax = Character.movement.runMax / speedMod;
                }
                Move(directionTowardsObject * -1);
            }
        }
    }

    /// <summary>
    /// Behavior of the AI when they have a Precision weapon
    /// </summary>
    private void PrecisionAttackBehavior()
    {
        AttackMove(.8f, ATTACK_SPEED_MOD);

        if (bulletsToFire > 0)
        { 
            bool previous = Physics2D.queriesHitTriggers;
            Physics2D.queriesHitTriggers = true;
            RaycastHit2D hit;

            if (!Character.lookingLeft)
            {
                hit = Physics2D.Raycast(Character.Weapon.FireLocation.transform.position, transform.right);
            }
            else
            {
                hit = Physics2D.Raycast(Character.Weapon.FireLocation.transform.position, transform.right * -1);
            }

            Physics2D.queriesHitTriggers = previous;

            if ((hit.collider.gameObject.tag != "Enemy" && hit.collider.gameObject.tag != "Terrain"))
            {
                if (Fire())
                {
                    bulletsToFire--;
                }
            }
        }
        else if (bulletsToFire <= 0 && !finishedFiring)
        {
            StartCoroutine(WaitToFireTimer());
            finishedFiring = true;
        }
    }

    /// <summary>
    /// Behavior of AI when they have a scatter weapon
    /// </summary>
    private void ScatterAttackBehavior()
    {
        AttackMove(.3f, 1.5f);

        if (bulletsToFire > 0)
        {
            bool previous = Physics2D.queriesHitTriggers;
            Physics2D.queriesHitTriggers = true;
            RaycastHit2D hit;

            if (!Character.lookingLeft)
            {
                hit = Physics2D.Raycast(Character.Weapon.FireLocation.transform.position, transform.right);
            }
            else
            {
                hit = Physics2D.Raycast(Character.Weapon.FireLocation.transform.position, transform.right * -1);
            }

            Physics2D.queriesHitTriggers = previous;

            if ((hit.collider.gameObject.tag != "Enemy" && hit.collider.gameObject.tag != "Terrain"))
            {
                if (Fire())
                {
                    bulletsToFire--;
                }
            }
        }
        else if (bulletsToFire <= 0 && !finishedFiring)
        {
            StartCoroutine(WaitToFireTimer());
            finishedFiring = true;
        }
    }

    /// <summary>
    /// Method to control the behavior of the AI when they have an automatic weapon.
    /// </summary>
    private void AutomaticAttackBehavior()
    {
        AttackMove(.5f, ATTACK_SPEED_MOD);

        if (bulletsToFire > 0)
        {
            bool previous = Physics2D.queriesHitTriggers;
            Physics2D.queriesHitTriggers = true;
            RaycastHit2D hit;

            if (!Character.lookingLeft)
            {
                hit = Physics2D.Raycast(Character.Weapon.FireLocation.transform.position, transform.right);
            }
            else
            {
                hit = Physics2D.Raycast(Character.Weapon.FireLocation.transform.position, transform.right * -1);
            }

            Physics2D.queriesHitTriggers = previous;

            if (hit.collider.gameObject.tag == "Player" && Fire())
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

    #region SearchBehavior
    /// <summary>
    /// Method to control the behavior of the AI when they are searching for the player
    /// </summary>
    private void Search()
    {
        float distance = Vector2.Distance(Character.transform.position, lastPlayerPosition);
        if (Math.Abs(distance) > SEARCH_RADIUS)
        {
            short direction = GetMovementDirection(lastPlayerPosition.x);
            Move(direction);
        }
        else
        {
            if (searchDirection == 0)
            {
                int direction = UnityEngine.Random.Range(0, 2);
                if (direction == 1)
                {
                    searchDirection = 1;
                }
                else
                {
                    searchDirection = -1;
                }
            }

            if (searchMoveTimer > 0)
            {
                searchMoveTimer -= Time.deltaTime;
                Move(searchDirection);
            }
            else if (pauseTimer > 0)
            {
                pauseTimer -= Time.deltaTime;
            }
            else
            {
                searchMoveTimer = MAX_SEARCH_MOVE_TIME;
                pauseTimer = MAX_PAUSE_TIME;

                int direction = UnityEngine.Random.Range(0, 2);
                if (direction == 1)
                {
                    searchDirection = 1;
                }
                else
                {
                    searchDirection = -1;
                }
            }
        }
    }
    #endregion

    #region PatrolBehavior
    /// <summary>
    /// Method to control the behavior of the AI when they are patroling an area
    /// </summary>
    private void Patrol()
    {
        if (Mathf.Abs(RoomGraph[CurrentNode].transform.position.x - Character.transform.position.x) > .8)
        {
            switch (CurrentPathType)
            {
                case Node.PathType.Walking:
                    PatrolWalkBehavior();
                    break;
                case Node.PathType.Jumping:
                    PatrolJumpBehavior();
                    break;
                case Node.PathType.Variable:
                    PatrolVaraibleBehavior();
                    break;
            }
        }
        else if (Character.movement.charCont.isGrounded)
        {
            PreviousNode = CurrentNode;
            GetNewNode();
            canMakeJump = false;
            madeJump = false;
        }
    }

    /// <summary>
    /// e
    /// </summary>
    private void PatrolVaraibleBehavior()
    {
        //e
    }

    /// <summary>
    /// Method to control how the AI jumps to a node
    /// </summary>
    private void PatrolJumpBehavior()
    {
        short direction = GetMovementDirection(RoomGraph[CurrentNode].transform.position.x);
        if (!canMakeJump)
        {
            Vector2 initialVelocity = GetInitialVelocity(direction);
            float time = JumpTAtX(RoomGraph[CurrentNode].transform.position.x, initialVelocity);
            Vector2 PositionAtT = MaxJumpPositionAtT(time, initialVelocity);
            canMakeJump = true;
        }

        if (canMakeJump)
        {
            if (Mathf.Abs(RoomGraph[CurrentNode].transform.position.x - Character.transform.position.x) > 1.0f && !madeJump)
            {
                Jump();
                Move(direction);
            }
            else if (!Character.movement.charCont.isGrounded)
            {
                madeJump = true;
                Character.movement.velocity = new Vector2(0, Character.movement.velocity.y);
                JumpCancel();
            }
            else if (Character.movement.charCont.isGrounded)
            {
                Move(direction);
            }
        }
        else if (PreviousNode != -1)
        {
            GetNewNode();
        }
    }

    /// <summary>
    /// Behavior to control how an AI walks to a node
    /// </summary>
    private void PatrolWalkBehavior()
    {
        Move(GetMovementDirection(RoomGraph[CurrentNode].transform.position.x));
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Gets the direction to move towards another X location
    /// </summary>
    /// <param name="otherX">The x we are comparing the character's position to</param>
    /// <returns></returns>
    private short GetMovementDirection(float otherX)
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
    private bool isFacingLeft
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
    private void UpdateQTEManagerPosition()
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

    /// <summary>
    /// Finds a new random node adjacent to the current node in the graph
    /// </summary>
    private void GetNewNode()
    {
        int newCurrentNode = UnityEngine.Random.Range(0, RoomGraph[CurrentNode].AdjacentNodes.Count);
        CurrentPathType = RoomGraph[CurrentNode].AdjacentNodes[newCurrentNode].Item2;
        CurrentNode = RoomGraph[CurrentNode].AdjacentNodes[newCurrentNode].Item1.NodeIndex;
    }


    /// <summary>
    /// Gets the initial velocity of the character
    /// </summary>
    /// <param name="direction">Direction the character is moving in</param>
    /// <returns>Returns the inital velocity vector</returns>
    private Vector2 GetInitialVelocity(float direction)
    {
        return Character.movement.jumpVelocity * Vector2.up + Character.movement.runMax * direction * Vector2.right;
    }

    /// <summary>
    /// Gets the position at which the enemy will jump to at time t with the initial velocity
    /// </summary>
    /// <param name="t">the time in the equation</param>
    /// <param name="initialVelocity">initial velocity of the character</param>
    /// <returns>Returns the X Y coordinate where the character will be at when attempting the jump</returns>
    private Vector2 MaxJumpPositionAtT(float t, Vector2 initialVelocity)
    {
        return (Vector2)Character.transform.position + (initialVelocity * t + 0.5f * Physics2D.gravity * t * t);
    }

    /// <summary>
    /// Finds the time it will take for us to get to X during a jump
    /// </summary>
    /// <param name="x">the target x value</param>
    /// <param name="initialVelocity">initial velocity of the character</param>
    /// <returns>returns the time at which the character will get to x</returns>
    private float JumpTAtX(float x, Vector2 initialVelocity)
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
        if (PlayerController.instance.Character != null)
        {
            return (Vector3.Distance(playerPos, Character.transform.position) < PLAYER_RANGE);
        }
        return false;
    }

    /// <summary>
    /// Selects the closest node in the graph for the AI to start moving towards
    /// </summary>
    private void SetStart()
    {
        float shortestDistance = float.MaxValue;
        int shortestDistanceIndex = 0;
        for (int i = 0; i < RoomGraph.Count; i++)
        {
            float distance = Mathf.Abs(Vector2.Distance(Character.transform.position, RoomGraph[i].transform.position));
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                shortestDistanceIndex = i;
            }
        }

        CurrentNode = shortestDistanceIndex;
        CurrentPathType = Node.PathType.Walking;
        PreviousNode = -1;
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

    /// <summary>
    /// Coroutine that simulates the action of being frozen by an EMP
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartSearch()
    {
        yield return new WaitForSeconds(SearchTime);
        currentState = AIState.Patroling;
        SetStart();
    }
    #endregion
}