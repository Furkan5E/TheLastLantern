using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if(player.groundDetected)
            stateMachine.ChangeState(player.landingState);
        
        if(player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
        
        // Transition to fall loop when start animation finishes
        if(triggerCalled)
            stateMachine.ChangeState(player.fallLoopState);
    }
}