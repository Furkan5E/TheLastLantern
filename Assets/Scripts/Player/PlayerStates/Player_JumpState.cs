using UnityEngine;

public class Player_JumpState : Player_AiredState
{
    private bool jumpCut;
    
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        jumpCut = false;
        player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
        player.coyoteTimeCounter = 0f;
    }
    
    public override void Update()
    {
        base.Update();
        
        if (input.Player.Jump.WasReleasedThisFrame() && rb.linearVelocity.y > 0 && !jumpCut)
        {
            jumpCut = true;
        }
        
        if (rb.linearVelocity.y < 0 && stateMachine.currentState != player.jumpAttackState)
            stateMachine.ChangeState(player.fallState);
    }
    
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (jumpCut && rb.linearVelocity.y > 0)
        {
            player.SetVelocity(
                rb.linearVelocity.x,
                rb.linearVelocity.y * player.jumpCutMultiplier
            );

            jumpCut = false;
        }
    }
}