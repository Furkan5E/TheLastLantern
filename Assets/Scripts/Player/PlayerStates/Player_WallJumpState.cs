using UnityEngine;

public class Player_WallJumpState : PlayerState
{
    private float inputLockTime = 0.25f;

    public Player_WallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = inputLockTime;
     
        player.SetVelocity(player.wallJumpForce.x * -player.facingDir, player.wallJumpForce.y);
    }

    public override void Update()
    {
        base.Update();

        stateTimer -= Time.deltaTime;

        if (rb.linearVelocity.y < 0)
            stateMachine.ChangeState(player.fallState);

        if (player.wallDetected && stateTimer <= 0)
            stateMachine.ChangeState(player.wallSlideState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (stateTimer <= 0)
        {
            if (player.moveInput.x != 0)
                player.SetVelocity(player.moveInput.x * (player.moveSpeed * player.inAirMoveMultiplier),rb.linearVelocity.y);
        }
    }
}