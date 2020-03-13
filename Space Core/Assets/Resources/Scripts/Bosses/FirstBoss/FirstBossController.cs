using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstBossController : Controller
{
    public enum BossStage { Stage1, State2, Stage3 }

    #region PublicVariables
    public BossStage Stage { get; set; }
    public CentralLaserCannon CentralLaserCannon;
    public LaserCannons LaserCannonRight;
    public LaserCannons LaserCannonLeft;
    public Transform LaserCannonRightAttachPoint;
    public Transform LaserCannonLeftAttachPoint;
    public RocketArms RocketArmRight;
    public RocketArms RocketArmLeft;
    public Transform RocketArmRightAttachedPoint;
    public Transform RocketArmLeftAttachedPoint;
    public MissileLauncher[] MissileLaunchers;
    #endregion

    #region PrivateVariables
    private float fireWaitTime;
    private float moveWaitTime;
    private float moveTimer;
    private int bulletsToFire;
    private int maxBulletsToFire;
    private short moveDirection;
    private bool finishedFiring;
    private bool finishedMoving;
    private Vector2 playerPos { get { return PlayerController.instance.Character.transform.position; } }
    #endregion

    #region Constants
    protected const float MAX_PAUSE_TIME = 1.5f;
    protected const float MAX_MOVE_TIME = 1.0f;
    private const float PLAYER_RANGE = 25.0f;
    #endregion

    public override void Start()
    {
        moveDirection = 1;
        maxBulletsToFire = 4;
        bulletsToFire = maxBulletsToFire;
        Character.AttackRecievedDeleage = ReceiveAttack;
        Character.TriggerEntered2DDelegate = TriggerEnter2D;
        fireWaitTime = 1.5f;
        moveWaitTime = .75f;
        moveTimer = moveWaitTime;
        base.Start();
    }

    /// <summary>
    /// Empty because player will not be able to hack the boss
    /// </summary>
    public override void DeselectHackTarget()
    {
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

    // Update is called once per frame
    protected override void Update()
    {
        if (InputManager.instance.currentState != InputManager.InputState.GAMEPLAY)
            return;

        if (IsPlayerInRange())
        {
            FireLaserCannons();
            MoveAroundPlayer();
        }        
    }

    private void MoveAroundPlayer()
    {
        if(moveTimer > 0 && !finishedMoving)
        {
            moveTimer -= Time.deltaTime;
            Move(moveDirection * Vector2.right);
        }
        else
        {
            finishedMoving = true;
            StartCoroutine(WaitToMoveTimer());
        }
    }

    private void FireLaserCannons()
    {
        Vector2 myPosition = Character.transform.position;
        Vector2 diff = playerPos - myPosition;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        bool pointLeft = Mathf.Abs(angle) > 90;
        if (pointLeft ^ Character.lookingLeft)
        {
            Vector3 newScale = Character.gameObject.transform.localScale;
            newScale.x *= -1;
            Character.gameObject.transform.localScale = newScale;
            Character.lookingLeft = !Character.lookingLeft;
        }
        else
        {
            LaserCannonRight.AimWeapon(angle);
        }

        if (bulletsToFire > 0 && IsPlayerInRange())
        {
            if (LaserCannonRight.Activate(0))
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

    private void FireMissiles()
    {

    }

    private void FireRocketArms()
    {

    }

    private void FireCentralCannon()
    {

    }

    private void AttachlaserCannonsToCeiling()
    {

    }

    public override void Die()
    {
        InputManager.instance.currentState = InputManager.InputState.GAME_OVER;
        SceneManager.LoadScene(3);
    }

    public override void TriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Rocket"))
        {
            //need to heal player if vampire bullets are active
            AttackInfo info = collision.gameObject.GetComponentInChildren<ExplosionInformation>().Info;
            if (Bullet.VampireBullet && info.damageSource == DamageSource.Player)
            {
                float healthToHeal = info.damage * 0.25f;
                if (healthToHeal < 0)
                {
                    PlayerController.instance.HealCharacter(1);
                }
                else
                {
                    PlayerController.instance.HealCharacter((int)healthToHeal);
                }
            }
            ReceiveAttack(info);
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
    /// Coroutine that will wait to reset the enemy's ability to shoot
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitToMoveTimer()
    {
        yield return new WaitForSeconds(moveWaitTime);
        moveDirection *= -1;
        moveTimer = moveWaitTime;
        finishedMoving = false;
    }
}
