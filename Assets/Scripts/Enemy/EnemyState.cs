using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyState : EntityState
{
    protected Enemy enemy;
    public EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;

        rb = enemy.rb;
        anim = enemy.anim;
    }

    public override void Update()
    {
        base.Update();
        if (Keyboard.current.fKey.wasPressedThisFrame)
            stateMachine.ChangeState(enemy.attackState);
        anim.SetFloat("runAnimSpeedMultiplier", enemy.runAnimSpeedMultiplier);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
