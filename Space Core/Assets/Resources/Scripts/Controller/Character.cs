using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is in charge of exposing elements that all characterrs share
/// </summary>
public class Character : MonoBehaviour
{
    public enum AnimationState { idle, running, hit, jump, special, runextended, falling }

    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public string Class { get; set; }
    public Weapon Weapon { get; set; }
    public int Invincible { get; set; }
    public short MoveDirection { get; set; }
    public GameObject HackArea { get; protected set; }
    public GameObject RotationPoint { get; set; }
    public GameObject MinimapIcon;
    public GameObject QTEPanel { get; protected set; }
    public Transform QTEPointLeft;
    public Transform QTEPointRight;
    public Canvas WorldspaceCanvas;
    public ActiveAbility ActiveAbility { get; set; }
    public Movement movement { get; protected set; }
    public bool IsBlinking;
    public bool IsPlayer;
    public AudioClip WalkClip;
    public AudioClip HitClip;
    public AudioSource AudioSource { get; private set; }
    public int isStunned { get; set; }
    public bool IsSelected { get; set; }
    public SkinnedMeshRenderer[] childComponents;
    public AnimationState animationState { get; set; }
    public Animator animator;
    
    public bool lookingLeft = false;
    public int facingDirection { get { return (int)Mathf.Sign(transform.localScale.x); } }

    #region delegates
    public delegate void OnAttackRecieved(AttackInfo attackInfo);
    public OnAttackRecieved AttackRecievedDeleage;
    public delegate void OnTriggerEntered2D(Collider2D collision);
    public OnTriggerEntered2D TriggerEntered2DDelegate;
    #endregion

    private GameObject DropAmmo { get; set; }

    /// <summary>
    /// Start function that finds the game objects associated with the character before first frame of gameplay
    /// </summary>
    void Start()
    {
        MoveDirection = 0;
        AudioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Awake function that finds main game objects associated with the character
    /// </summary>
    void Awake()
    {
        if (!(movement = GetComponent<Movement>()))
            movement = gameObject.AddComponent<BasicMovement>();
        else
            movement = GetComponent<Movement>();

        RotationPoint = transform.Find("RotationPoint").gameObject;
        Weapon = WeaponGenerator.GenerateWeapon(RotationPoint.transform.Find("WeaponLocation")).GetComponent<Weapon>();
        AbilityGenerator.AddAbilitiesToCharacter(gameObject);
        Weapon.BulletSource = DamageSource.Enemy;
        DropAmmo = Resources.Load<GameObject>("Prefabs/Enemy/DroppedAmmo/DroppedAmmo");
        QTEPointLeft = transform.Find("QTEPointLeft");
        QTEPointRight = transform.Find("QTEPointRight");
        HackArea = transform.Find("PS_Hack Sphere").gameObject;
        QTEPanel = transform.Find("QTE_Canvas").gameObject;
        QTEPanel.SetActive(false);
        animator = transform.Find("AnimatedCharacter").gameObject.GetComponent<Animator>();
        if (MinimapIcon == null)
        {
            MinimapIcon = transform.Find("MinimapIcon").gameObject;
            MinimapIcon.layer = LayerMask.NameToLayer("Minimap/Mapscreen");
        }
        childComponents = GetComponentsInChildren<SkinnedMeshRenderer>();
        SetMeshEmissionColor(Color.blue);
    }

    /// <summary>
    /// Attempts activation of the current active ability
    /// </summary>
    /// <returns>Was the ability activated</returns>
    public bool ActivateActiveAbility()
    {
        return ActiveAbility.AttemptActivation();
    }

    /// <summary>
    /// Function that moves the character along an direction based on its character controller values
    /// </summary>
    /// <param name="direction">Direction to move the character</param>
    public virtual void Move(Vector2 direction)
    {
        if (isStunned > 0)
        {
            direction.x = 0;
            return;
        }

        if (direction != Vector2.zero && direction.x != 0)
        {
            MoveDirection = (short)Mathf.Sign(direction.x);
        }

        //If we are moving, and are not in a running or falling state and the player is grounded, move to the running state
        if (direction.x != 0 
            && (animationState != AnimationState.running || animationState == AnimationState.falling)
            && movement.charCont.isGrounded)
        {
            animationState = AnimationState.running;
            animator.SetInteger("AnimationState", (int)animationState);
        }
        //If we are running, jumping or falling, the character is grounded and not moving, move to the idle state
        else if (direction.x == 0 
            && (animationState == AnimationState.running || animationState == AnimationState.jump || animationState == AnimationState.falling)
            && movement.charCont.isGrounded)
        {
            animationState = AnimationState.idle;
            animator.SetInteger("AnimationState", (int)animationState);
        }
        //If the player is running or is idle, but we are not grounded. Move to the jump state
        else if (
            (animationState == AnimationState.running || animationState == AnimationState.idle)
            && !movement.charCont.isGrounded)
        {
            animationState = AnimationState.jump;
            animator.SetInteger("AnimationState", (int)animationState);
        }

        //if we are grounded and moving forward, play the walking audi clip
        if (direction.x != 0 && movement.charCont.isGrounded)
        {
            if (!AudioSource.isPlaying)
            {
                AudioSource.clip = WalkClip;
                AudioSource.Play();
            }
        }

        movement.Move(direction);
    }

    /// <summary>
    /// Function that has the player jump based on its movement values
    /// </summary>
    public virtual void Jump()
    {
        if (isStunned <= 0)
        {
            if(animationState != AnimationState.jump)
            {
                animationState = AnimationState.jump;
                animator.SetInteger("AnimationState", (int)animationState);
            }
            movement.Jump();
        }
    }

    /// <summary>
    /// Function that makes the character stop in midair
    /// </summary>
    public virtual void JumpCancel()
    {
        if (isStunned <= 0)
            movement.JumpCancel();
    }


    /// <summary>
    /// Fucntion that invokes the AttackRecieved delegate with the attack info
    /// </summary>
    /// <param name="attackInfo">information on the attack the character is receiving</param>
    public virtual void ReceiveAttack(AttackInfo attackInfo)
    {
        AttackRecievedDeleage?.Invoke(attackInfo);
    }

    /// <summary>
    /// Coroutine to make characters temporarily blink when hit 
    /// </summary>
    public IEnumerator beginFlashing()
    {
        //Loop through the skinned mesh renderer of each body part
        foreach (SkinnedMeshRenderer smr in childComponents)
        {
            //Only continue operation if character is blinking
            if (IsBlinking)
            {
                Material m = smr.material;
                Color32 c = smr.material.color;
                smr.material = null;
                smr.material.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                smr.material = m;
                smr.material.color = c;
            }
        }
        //reset blinking flag
        IsBlinking = false;
    }

    /// <summary>
    /// Sets red for player mesh emission and blue for enemies
    /// </summary>
    /// <param name="newColor"></param>
    public void SetMeshEmissionColor(Color newColor)
    {
        //Loop through the skinned mesh renderer of each body part
        foreach (SkinnedMeshRenderer smr in childComponents)
        {
            Material m = smr.material;
            m.SetColor("_EmissionColor", newColor);
            smr.material = m;
        }
    }

    /// <summary>
    /// Coroutine that stuns the player for x amount of time
    /// </summary>
    /// <param name="time">the time that the player is stunned for</param>
    /// <returns></returns>
    public IEnumerator Stun(float time)
    {
        isStunned++;
        animationState = AnimationState.hit;
        animator.SetInteger("AnimationState", (int)animationState);
        yield return new WaitForSeconds(time);
        animationState = AnimationState.idle;
        animator.SetInteger("AnimationState", (int)animationState);
        isStunned--;
    }

    /// <summary>
    /// function that will fire the weapon the character is currnetly holding
    /// </summary>
    /// <returns>whether or not the weapon fired correctly</returns>
    public bool Fire()
    {
        //reload if out of ammo
        if (Weapon.AmmoInClip <= 0 && !Weapon.IsReloading)
        {
            Weapon.EndFire();
            Reload();
            return false;
        }
        else
        {
            return Weapon.Fire();
        }
    }

    /// <summary>
    /// function that will call the FireEnd function of the weapon class
    /// </summary>
    /// <returns>true if successful, flase otherwise</returns>
    public bool EndFire()
    {
        //reload if out of ammo
        if (Weapon.AmmoInClip <= 0 || Weapon.IsReloading)
        {
            return false;
        }
        else
        {
            return Weapon.EndFire();
        }
    }

    /// <summary>
    /// function that exposes the weapon's reload function
    /// </summary>
    public virtual void Reload()
    {
        Weapon.Reload(this);
    }

    /// <summary>
    /// Function that changes the angle that the character is aiming at. If given a camera, it will update
    /// the camera's scale if it needs to
    /// </summary>
    /// <param name="angle">New angle that character is aiming at</param>
    /// <param name="camera">Camera to update</param>
    public void AimWeapon(float angle, Camera camera)
    {
        bool pointLeft = Mathf.Abs(angle) > 90;
        if (pointLeft ^ lookingLeft)
        {
            Vector3 newScale = gameObject.transform.localScale;
            newScale.x *= -1;
            gameObject.transform.localScale = newScale;
            if (camera != null && Camera.main.transform.localScale.x != 1)
            {
                Camera.main.transform.localScale = newScale;
            }
            newScale = RotationPoint.transform.localScale;
            newScale.x *= -1;
            newScale.y *= -1;
            RotationPoint.transform.localScale = newScale;
            lookingLeft = !lookingLeft;
        }
        if (lookingLeft)
        {
            MoveDirection = -1;
            angle *= -1;
        }
        else
        {
            MoveDirection = 1;
        }

        RotationPoint.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    public void HealCharacter(int health)
    {
        if (health + Health > MaxHealth)
        {
            Health = MaxHealth;
        }
        else
        {
            Health += health;
        }
    }

    /// <summary>
    /// Method used to update the character's animation state
    /// </summary>
    /// <param name="animationState">The new animation state</param>
    public void SetAnimationState(AnimationState animationState)
    {
        this.animationState = animationState;
        animator.SetInteger("AnimationState", (int)animationState);
    }

    /// <summary>
    /// Function to call the TriggerEntered2DDelegate
    /// </summary>
    /// <param name="collision">collision information</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerEntered2DDelegate?.Invoke(collision);
    }
}
