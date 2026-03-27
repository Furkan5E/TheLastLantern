using UnityEngine;

public class Player_FallState : Player_AiredState
{
    private float defaultGravity = 3.8f;
    private float maxGravity = 8.0f;
    private float gravityIncreaseRate = 3.0f;

    private float maxFallSpeed = -20f;

    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.gravityScale = defaultGravity;
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravity;
    }

    public override void Update()
    {
        base.Update();

        //handle coyote time and jump input
        if (player.coyoteTimeCounter > 0)
            player.coyoteTimeCounter -= Time.deltaTime;

        if (input.Player.Jump.WasPressedThisFrame() && player.coyoteTimeCounter > 0)
            stateMachine.ChangeState(player.jumpState);

        if (player.groundDetected)
        {
            player.vfx.PlayLandingVfx();

            if (player.moveInput.x != 0)
                stateMachine.ChangeState(player.moveState);
            else
                stateMachine.ChangeState(player.idleState);
        }
        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        rb.gravityScale = Mathf.Min(rb.gravityScale + gravityIncreaseRate *Time.fixedDeltaTime, maxGravity);
        //clamp fall speed
        rb.linearVelocity = new Vector2(rb.linearVelocity.x,Mathf.Max(rb.linearVelocity.y, maxFallSpeed));
    }
}