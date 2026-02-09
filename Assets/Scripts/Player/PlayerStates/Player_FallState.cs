using UnityEngine;
using UnityEngine.VFX;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.gravityScale = 3.8f; //faster fall
    }

    public override void Update()
    {
        base.Update();

        if (player.coyoteTimeCounter > 0)
            player.coyoteTimeCounter -= Time.deltaTime;

        //allow jump during coyote time
        if (input.Player.Jump.WasPressedThisFrame() && player.coyoteTimeCounter > 0)
            stateMachine.ChangeState(player.jumpState);

        if (player.groundDetected)
        {
            // Create more dynamic particle spread based on fall velocity
            var main = player.landingDustFX.main;
            float fallSpeed = Mathf.Abs(player.rb.linearVelocity.y);
            
            // Scale particle properties based on impact
            main.startSpeed = Mathf.Lerp(2f, 8f, fallSpeed / 20f); // Adjust 20f based on max fall speed
            
            player.landingDustFX.Play();
            stateMachine.ChangeState(player.idleState);
        }

        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}
