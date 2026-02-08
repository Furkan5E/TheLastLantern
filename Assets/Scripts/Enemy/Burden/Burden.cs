using UnityEngine;

public class Burden : Entity
{
    public Burden_WalkState walkState;
    public Burden_DeadState deadState;

    [Header("Movement Details")]
    public float moveSpeed = 1.2f;

    [Header("Contact Damage")]
    public float contactDamage = 1f;
    public float contactDamageCooldown = 1f;
    private float lastContactDamageTime = -999f;

    public bool isDead { get; private set; }

    [Header("Edge Detection")]
    public GameObject[] wayPoints;
    public int nextPoint;
    public float distToPoint;

    protected override void Awake()
    {
        base.Awake();

        walkState = new Burden_WalkState(this, stateMachine, "burdenMove");
        deadState = new Burden_DeadState(this, stateMachine, "burdenMove");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(walkState);
    }

    protected override void Update()
    {
        // Don't call base.Update() - we handle state machine manually
        // This skips HandleCollisionDetection() from Entity base class
        stateMachine.UpdateActiveState();
    }

    public void Move()
    {
        distToPoint = Vector2.Distance(transform.position, wayPoints[nextPoint].transform.position);
        transform.position = Vector2.MoveTowards(transform.position, wayPoints[nextPoint].transform.position, moveSpeed * Time.deltaTime);
        if (distToPoint < 0.2f)
        {
            TakeTurn();
        }
    }

    private void TakeTurn()
    {
        Vector3 currRotation = transform.eulerAngles;
        currRotation.z += wayPoints[nextPoint].transform.eulerAngles.z;
        transform.eulerAngles = currRotation;
        ChooseNextPoint();
    }

    private void ChooseNextPoint()
    {
        nextPoint++;

        if (nextPoint == wayPoints.Length)
        {
            nextPoint = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Main collider handles physics - no damage here
        // Damage is handled by child trigger collider
    }

    // This is called by the child DamageTrigger
    public void OnPlayerEnterDamageTrigger(GameObject playerObject)
    {
        DamagePlayer(playerObject);
    }

    private void DamagePlayer(GameObject playerObject)
    {
        // Check cooldown to prevent damage spam
        if (Time.time < lastContactDamageTime + contactDamageCooldown)
            return;

        Entity_Health playerHealth = playerObject.GetComponent<Entity_Health>();

        if (playerHealth != null)
        {
            // Apply damage to player
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
}

