using UnityEngine;

public class Ghost : Entity
{
    public Ghost_FlyState flyState;
    public Ghost_DeadState deadState;

    [Header("Movement Details")]
    public float moveSpeed = 2f;
    public Vector2 moveDirection = new Vector2(1f, 0.25f);

    [Header("Contact Damage")]
    public float contactDamage = 1f;
    public float contactDamageCooldown = 1f;
    private float lastContactDamageTime = -999f;

    public bool isDead { get; private set; }

    [Header("Fly Detection")]
    [SerializeField] private GameObject flyRightCheck;
    [SerializeField] private GameObject flyRoofCheck;
    [SerializeField] private GameObject flyGroundCheck;
    [SerializeField] private Vector2 flyRightCheckSize;
    [SerializeField] private Vector2 flyRoofCheckSize;
    [SerializeField] private Vector2 flyGroundCheckSize;
    [SerializeField] private LayerMask flyDetectionLayer;

    [SerializeField] private bool goingUp = true;

    private bool touchedRight;
    private bool touchedRoof;
    private bool touchedGround;

    protected override void Awake()
    {
        base.Awake();

        flyState = new Ghost_FlyState(this, stateMachine, "ghostFly");
        deadState = new Ghost_DeadState(this, stateMachine, "ghostFly");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(flyState);
    }

    protected override void Update()
    {
        // Don't call base.Update() - we handle state machine manually
        // This skips HandleCollisionDetection() from Entity base class
        // since Ghost uses its own overlap box detection for flying
        stateMachine.UpdateActiveState();
    }

    public void Fly()
    {
        HandleFlyDetection();
        rb.linearVelocity = moveDirection.normalized * moveSpeed;
    }

    private void HandleFlyDetection()
    {
        touchedRight = DetectOverlap(flyRightCheck, flyRightCheckSize);
        touchedRoof = DetectOverlap(flyRoofCheck, flyRoofCheckSize);
        touchedGround = DetectOverlap(flyGroundCheck, flyGroundCheckSize);

        if (touchedRight)
        {
            Flip();
            moveDirection.x = -moveDirection.x;
        }

        if (touchedRoof && goingUp)
        {
            ChangeYDirection();
        }

        if (touchedGround && !goingUp)
        {
            ChangeYDirection();
        }
    }

    private bool DetectOverlap(GameObject checkObject, Vector2 size)
    {
        return Physics2D.OverlapBox(checkObject.transform.position, size, 0f, flyDetectionLayer);
    }

    private void ChangeYDirection()
    {
        moveDirection.y = -moveDirection.y;
        goingUp = !goingUp;
    }

    // Called by the child DamageTrigger
    public void OnPlayerEnterDamageTrigger(GameObject playerObject)
    {
        DamagePlayer(playerObject);
    }

    private void DamagePlayer(GameObject playerObject)
    {
        if (Time.time < lastContactDamageTime + contactDamageCooldown)
            return;

        Entity_Health playerHealth = playerObject.GetComponent<Entity_Health>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(contactDamage, transform);
            lastContactDamageTime = Time.time;
        }
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.ChangeState(deadState);
        isDead = true;
    }

    // Override without calling base - Entity's ground/wall check transforms
    // are not used by Ghost, so drawing them would cause null references
    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (flyGroundCheck != null)
            Gizmos.DrawWireCube(flyGroundCheck.transform.position, flyGroundCheckSize);
        if (flyRoofCheck != null)
            Gizmos.DrawWireCube(flyRoofCheck.transform.position, flyRoofCheckSize);
        if (flyRightCheck != null)
            Gizmos.DrawWireCube(flyRightCheck.transform.position, flyRightCheckSize);
    }
}
