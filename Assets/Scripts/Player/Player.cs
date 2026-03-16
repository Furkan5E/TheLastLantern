using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public static event Action OnPlayerDeath;
    public PlayerInputSet input { get; private set; }
    public UI ui { get; private set; }
    public Entity_VFX vfx { get; private set; }
    public Entity_Health health { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_DeadState deadState { get; private set; }

    [Header("Attack Details")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = 0.1f;
    public float comboResetTime = 1f;
    private Coroutine queuedAttackCoroutine;

    [Header("Movement Details")]
    public float moveSpeed;
    public float groundAcceleration = 55f;
    public float jumpForce = 5f;
    public Vector2 wallJumpForce;
    
    public float coyoteTime = 0.1f;
    public float coyoteTimeCounter;

    [Range(0, 1)]
    public float inAirMoveMultiplier = 0.7f;
    [Range(0, 1)]
    public float jumpCutMultiplier = 0.5f;
    [Range(0, 1)]
    public float wallSlideSlowMultiplier = 0.7f;
    public Vector2 moveInput { get; private set; }

    [Header("Dashing")]
    public float dashCooldown = 1f;
    public float dashCooldownTimer;
    public float dashDuration = 0.25f;
    public float dashSpeed = 20;

    protected override void Awake()
    {
        base.Awake();
        ui = FindAnyObjectByType<UI>();
        vfx = GetComponent<Entity_VFX>();
        health = GetComponent<Entity_Health>();
        
        input = new PlayerInputSet();
        ui.SetupControlsUI(input);
        
        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        deadState = new Player_DeadState(this, stateMachine, "dead");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public override void EntityDeath()
    {
        base.EntityDeath();

        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deadState);
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
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        ICollectable collectable = other.GetComponent<ICollectable>();
        collectable?.OnCollect(this);
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
}