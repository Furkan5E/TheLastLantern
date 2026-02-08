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
        rb.gravityScale = 2.6f;
        player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
        player.coyoteTimeCounter = 0f;
    }
    
    public override void Update()
    {
        base.Update();
        
        if (input.Player.Jump.WasReleasedThisFrame() && rb.linearVelocity.y > 0 && !jumpCut)
        {
            jumpCut = true;
            player.SetVelocity(rb.linearVelocity.x, rb.linearVelocity.y * player.jumpCutMultiplier);
        }
        
        if (rb.linearVelocity.y < 0 && stateMachine.currentState != player.jumpAttackState)
            stateMachine.ChangeState(player.fallState);
    }
}