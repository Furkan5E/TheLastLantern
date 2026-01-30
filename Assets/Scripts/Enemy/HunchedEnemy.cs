using UnityEngine;

public class HunchedEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "run");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
}
