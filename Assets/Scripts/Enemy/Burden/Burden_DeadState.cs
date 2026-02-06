using UnityEngine;

public class Burden_DeadState : EntityState
{
    private Burden burden;

    public Burden_DeadState(Burden burden, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.burden = burden;
        rb = burden.rb;
        anim = burden.anim;
    }

    public override void Enter()
    {
        base.Enter();

        // Stop all movement
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;

        // Optionally disable collider to prevent further interactions
        burden.GetComponent<Collider2D>().enabled = false;
    }

    public override void Update()
    {
        base.Update();

        // Could add fade out logic here if needed
        // For now, just keep the entity stopped
    }

    public override void Exit()
    {
        base.Exit();
    }
}
