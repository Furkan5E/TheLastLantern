using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public PlayerInputSet input { get; private set; }

    private StateMachine stateMachine;

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }

    [Header("Attack Details")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = 0.1f;
    public float comboResetTime = 1f;
    private Coroutine queuedAttackCoroutine;

    [Header("Movement Details")]
    public float moveSpeed;
    public float jumpForce = 5f;
    public Vector2 wallJumpForce;
    [Range(0, 1)]
    public float inAirMoveMultiplier = 0.7f;

    [Range(0, 1)]
    public float wallSlideSlowMultiplier = 0.7f;
    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;

    public Vector2 moveInput { get; private set; }

    [Header("Collision detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    [Header("Health")]
    public int health = 5;
    public int maxHealth = 5;
    public const int MAX_HEALTH_CAP = 7;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        input = new PlayerInputSet();
        stateMachine = new StateMachine();

        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }

    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    private void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && facingRight == false)
            Flip();
        else if (xVelocity < 0 && facingRight == true)
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir *= -1;
    }

    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(wallCheckDistance * facingDir, 0));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Item item = other.gameObject.GetComponent<Item>();
        if (item == null)
        {
            return;
        }

        Debug.Log("Trigger with " + item.itemType);

        bool canCollect = true;

        switch (item.itemType)
        {
            case ItemType.HealthPotion:
                // Check if already at max health cap
                if (maxHealth >= MAX_HEALTH_CAP)
                {
                    Debug.Log("Max health already at cap (" + MAX_HEALTH_CAP + "), cannot collect");
                    canCollect = false;
                    break;
                }

                // Increase health but clamp to cap
                maxHealth = Mathf.Min(maxHealth + item.healthIncrease, MAX_HEALTH_CAP);
                Debug.Log("Max Health: " + maxHealth);
                break;

            case ItemType.SpeedPotion:
                moveSpeed += item.speedIncrease;
                Debug.Log("Move Speed: " + moveSpeed);
                break;

            case ItemType.HealingPotion:
                if (health == maxHealth)
                {
                    Debug.Log("Health is already at max (" + health + "), cannot collect");
                    canCollect = false;
                    break;
                }
                health = Mathf.Min(health + item.healingAmount, maxHealth);
                Debug.Log("Health: " + health);
                break;
        }

        if (canCollect)
        {
            // Remove the collected item
            Destroy(other.gameObject);
        }
        else
        {
            // Disable the collider to prevent repeated trigger events
            // This prevents the visual glitch when the item can't be collected
            Collider2D itemCollider = other.GetComponent<Collider2D>();
            if (itemCollider != null)
            {
                itemCollider.enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Re-enable the collider when player exits, so item can be collected later if conditions change
        Item item = other.gameObject.GetComponent<Item>();
        if (item != null)
        {
            Collider2D itemCollider = other.GetComponent<Collider2D>();
            if (itemCollider != null && !itemCollider.enabled)
            {
                itemCollider.enabled = true;
            }
        }
    }

    public void EnterAttackStateWithDelay()
    {
        if (queuedAttackCoroutine != null)
            StopCoroutine(queuedAttackCoroutine);

        queuedAttackCoroutine = StartCoroutine(EnterAttackStateWithDelayCoroutine());
    }
    private IEnumerator EnterAttackStateWithDelayCoroutine()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }
}