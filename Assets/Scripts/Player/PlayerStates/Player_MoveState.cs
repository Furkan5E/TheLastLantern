using UnityEngine;

public class Player_MoveState : Player_GroundedState
{
    public Player_MoveState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x == 0 || player.wallDetected)
            stateMachine.ChangeState(player.idleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        float targetSpeed = player.moveInput.x * player.moveSpeed;
        float currentSpeed = rb.linearVelocity.x;

        float newSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            player.groundAcceleration * Time.fixedDeltaTime
        );

        player.SetVelocity(newSpeed, rb.linearVelocity.y);
    }
}