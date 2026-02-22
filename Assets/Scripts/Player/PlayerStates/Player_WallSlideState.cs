using UnityEngine;

public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if(input.Player.Jump.WasPressedThisFrame())
            stateMachine.ChangeState(player.wallJumpState);

        if(player.wallDetected == false)
            stateMachine.ChangeState(player.fallState);

        if (player.groundDetected){
            stateMachine.ChangeState(player.idleState);
            player.Flip();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        HandleWallSlide();
    }

    private void HandleWallSlide()
    {
        if(player.moveInput.y < 0)
            player.SetVelocity(player.moveInput.x, rb.linearVelocity.y);
        else
            player.SetVelocity(player.moveInput.x, rb.linearVelocity.y * player.wallSlideSlowMultiplier);
    }
}
