using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    protected StateMachine stateMachine;



    private bool facingRight = true;
    public int facingDir { get; private set; } = 1;


    [Header("Collision detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform primaryWallcheck;
    [SerializeField] private Transform secondaryWallcheck;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

 

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
    }

 
    protected virtual void Start()
    {
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
        wallDetected = Physics2D.Raycast(primaryWallcheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround)
                    && Physics2D.Raycast(secondaryWallcheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallcheck.position, primaryWallcheck.position + new Vector3(wallCheckDistance * facingDir, 0));
        Gizmos.DrawLine(secondaryWallcheck.position, secondaryWallcheck.position + new Vector3(wallCheckDistance * facingDir, 0));
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
