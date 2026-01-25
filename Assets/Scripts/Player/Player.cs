using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    private PlayerInputSet input;
    private StateMachine stateMachine;

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    
    public Vector2 moveInput { get; private set; }

    [Header("Movement Details")]
    public float moveSpeed;
    private bool facingRight = true;

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
        stateMachine.UpdateActiveState();
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

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
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
                if(health == maxHealth)
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
}
