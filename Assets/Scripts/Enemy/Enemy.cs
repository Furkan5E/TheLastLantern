using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;

    [Header("Battle Details")]
    public float battleMoveSpeed = 3;
    public float attackDistance = 2;
    public float battleTimeDuration = 5;
    public float minRetreatDistance = 1;
    public Vector2 retreatVelocity;

    [Header("Movement Details")]
    public float idleTime = 2;
    public float moveSpeed = 1.4f;
    [Range(0, 2)]
    public float runAnimSpeedMultiplier = 1.4f;


    [Header("Player Detection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10;


    public RaycastHit2D PlayerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer | whatIsGround);
        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;
        return hit;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * playerCheckDistance), playerCheck.position.y));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * attackDistance), playerCheck.position.y));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDir * minRetreatDistance), playerCheck.position.y));

    }


    // public Transform player;
    // public float speed = 3f;
    // public LayerMask groundLayer;

    // [Header("Detection")]
    // public float initialDetectionRange = 5f;
    // public float aggroDetectionRange = 12f;
    // public bool requireLineOfSight = true;
    // public LayerMask obstacleLayer;

    // private Rigidbody2D rb;
    // private bool isGrounded;
    // private bool isAggro;

    // void Start()
    // {
    //     rb = GetComponent<Rigidbody2D>();
    // }

    // void Update()
    // {
    //     if (!CanSeePlayer())
    //     {
    //         rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    //         return;
    //     }

    //     float direction = Mathf.Sign(player.position.x - transform.position.x);

    //     // Raycast origin slightly above the feet
    //     Vector2 origin = (Vector2)transform.position + Vector2.down * 0.5f;
    //     Vector2 forwardDir = Vector2.right * direction;

    //     // Check ground in front
    //     RaycastHit2D frontHit = Physics2D.Raycast(origin, forwardDir, 1f, groundLayer);

    //     // Optional: still ensure enemy is grounded before jumping
    //     isGrounded = Physics2D.Raycast(origin, Vector2.down, 1f, groundLayer);

    //     if (isGrounded && frontHit.collider == null)
    //     {   // Move
    //         rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    //     }
    //     else
    //     {   // Stop
    //         rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    //     }
    // }

    // bool CanSeePlayer()
    // {
    //     if (player == null)
    //         return false;

    //     float distance = Vector2.Distance(transform.position, player.position);
    //     float currentRange = isAggro ? aggroDetectionRange : initialDetectionRange;

    //     if (distance > currentRange)
    //     {
    //         isAggro = false;
    //         return false;
    //     }

    //     if (requireLineOfSight)
    //     {
    //         Vector2 directionToPlayer = (player.position - transform.position).normalized;
    //         RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distance, obstacleLayer);

    //         // If we hit something, line of sight is blocked
    //         if (hit.collider != null)
    //         {
    //             isAggro = false;
    //             return false;
    //         }
    //     }

    //     isAggro = true;
    //     return true;
    // }

    // void OnDrawGizmosSelected()
    // {
    //     // Visualize detection ranges in editor
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, initialDetectionRange);

    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, aggroDetectionRange);
    // }
}
