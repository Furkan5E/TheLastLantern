using UnityEngine;

public class Player_LandingState : Player_GroundedState
{
    public Player_LandingState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        // Play landing dust particles
        if (player.landingDustFX != null)
            player.landingDustFX.Play();
        
        // Stop horizontal movement on landing for that "impact" feel
        player.SetVelocity(rb.linearVelocity.x * 0.2f, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();


        if(player.moveInput.x != 0)
            player.SetVelocity(player.moveInput.x * player.moveSpeed, rb.linearVelocity.y);
        // Transition to idle/move when landing animation finishes
        if (triggerCalled)
        {
            if (player.moveInput.x != 0)
                stateMachine.ChangeState(player.moveState);
            else
                stateMachine.ChangeState(player.idleState);
        }
    }
}
