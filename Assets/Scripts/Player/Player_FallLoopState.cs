using UnityEngine;

public class Player_FallLoopState : Player_AiredState
{
    public Player_FallLoopState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if(player.groundDetected)
            stateMachine.ChangeState(player.landingState);
        
        if(player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}
