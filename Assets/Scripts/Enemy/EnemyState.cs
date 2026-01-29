using UnityEngine;

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
        anim.SetFloat("runAnimSpeedMultiplier", enemy.runAnimSpeedMultiplier);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
