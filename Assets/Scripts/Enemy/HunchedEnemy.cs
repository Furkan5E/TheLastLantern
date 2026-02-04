using UnityEngine;

public class HunchedEnemy : Enemy, ICounterable
{
    protected override void Awake()
    {
        base.Awake();
        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "run");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
        deadState = new Enemy_DeadState(this, stateMachine, "idle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "idle");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public void HandleCounter()
    {
        if(canBeStunned == false)
            return;
            
        stateMachine.ChangeState(stunnedState);
    }
}
