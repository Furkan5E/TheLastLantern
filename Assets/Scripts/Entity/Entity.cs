using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    protected StateMachine stateMachine;



    private bool facingRight = true;
    public int facingDir { get; private set; } = 1;


    [Header("Collision detection")]
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] private Transform primaryWallcheck;
    [SerializeField] private Transform secondaryWallcheck;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    private bool isKnocked;
    private Coroutine knockbackCo;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
    }


    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        stateMachine.UpdateActiveState();
    }

    protected virtual void FixedUpdate()
    {
        HandleCollisionDetection();
        stateMachine.PhysicsUpdateActiveState();
    }

    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }

    public virtual void EntityDeath()
    {
    }

    public void ReciveKnockback(Vector2 knockback, float duration)
    {
        if(knockbackCo != null)
            StopCoroutine(knockbackCo);

        knockbackCo = StartCoroutine(KnockbackCo(knockback, duration));
    }

    private IEnumerator KnockbackCo(Vector2 knockback, float duration)
    {
        isKnocked = true;
        rb.linearVelocity = knockback;

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        isKnocked = false;
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if(isKnocked)
            return;
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void HandleFlip(float xVelocity)
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
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        if (secondaryWallcheck != null)
        {

            wallDetected = Physics2D.Raycast(primaryWallcheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround)
                        && Physics2D.Raycast(secondaryWallcheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
        }
        else
        {
            wallDetected = Physics2D.Raycast(primaryWallcheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
        }

    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallcheck.position, primaryWallcheck.position + new Vector3(wallCheckDistance * facingDir, 0));
        if (secondaryWallcheck != null)
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
