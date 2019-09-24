using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MoveData
{

    public Vector2 moveInitial;
    public Vector2 moveDone;
    public Vector2 moveLeft { get { return moveInitial - moveDone; } }
    public Vector2 normal;
    public RaycastHit2D[] hits;
    public int hitSize;
    
    public bool wasContinous;

    public bool moveCompleted { get { return moveLeft.magnitude <= CharacterController2D.totalCollisionOffset; } }

    public MoveData(Vector2 init, Vector2 done, Vector2 _normal, RaycastHit2D[] _hits, int _hitSize, bool _wasContinous)
    {
        moveInitial = init;
        moveDone = done;
        normal = _normal;
        hits = _hits;
        hitSize = _hitSize;
        wasContinous = _wasContinous;
    }

}

public class CharacterController2D : MonoBehaviour
{

    private const float extraCollisionOffset = 0.0035f;///0.0001f;
    static public float totalCollisionOffset { get { return Physics2D.defaultContactOffset + extraCollisionOffset; } }
    private LayerMask layerMask { get { return Physics2D.GetLayerCollisionMask(gameObject.layer); } }

    private Vector2 _currentSlope;
    public Vector2 currentSlope { get { return _currentSlope; } private set { _currentSlope = value; } }

    [SerializeField] private float slopeChangeThreshold = 45;
    [SerializeField] private float stepHeightThreshold = 0.5f;

    private Rigidbody2D rb;
    private CapsuleCollider2D capCol;

    private bool initialValidation = true;
    private void OnValidate() {
        if (initialValidation) {
            rb = gameObject.AddComponent<Rigidbody2D>();
            capCol = gameObject.AddComponent<CapsuleCollider2D>();

            // Default Values

            initialValidation = false;
        }

        //Const Values
        if (rb.bodyType != RigidbodyType2D.Kinematic)
            rb.bodyType = RigidbodyType2D.Kinematic;
        if (!rb.simulated)
            rb.simulated = true;
        if (rb.useFullKinematicContacts)
            rb.useFullKinematicContacts = false;

        if (!capCol.isTrigger)
            capCol.isTrigger = true;
        }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
