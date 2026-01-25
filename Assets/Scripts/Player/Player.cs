using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private float xInput;
    public int health = 5;
    public int maxHealth = 5;

    public const int MAX_HEALTH_CAP = 7;

    [Header("Movement")]
    public float moveSpeed = 3.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
       // xInput = Input.GetAxisRaw("Horizontal");

       // rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
    }
}
