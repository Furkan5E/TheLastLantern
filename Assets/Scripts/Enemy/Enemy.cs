using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        // Move
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

        // Raycast origin slightly above the feet
        Vector2 origin = (Vector2)transform.position + Vector2.down * 0.5f;
        Vector2 forwardDir = Vector2.right * direction;

        // Check ground in front
        RaycastHit2D frontHit = Physics2D.Raycast(origin, forwardDir, 2.5f, groundLayer);

        // Optional: still ensure enemy is grounded before jumping
        isGrounded = Physics2D.Raycast(origin, Vector2.down, 1f, groundLayer);

        if (isGrounded && frontHit.collider != null)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 7f);
        }
    }
}
