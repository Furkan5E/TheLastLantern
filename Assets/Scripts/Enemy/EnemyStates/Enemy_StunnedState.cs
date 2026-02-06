using UnityEngine;

public class Enemy_StunnedState : EnemyState
{
    public Enemy_StunnedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        stateTimer = enemy.stunnedDuration;
        rb.linearVelocity = new Vector2(enemy.stunnnedVelocity.x * -enemy.facingDir, enemy.stunnnedVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
