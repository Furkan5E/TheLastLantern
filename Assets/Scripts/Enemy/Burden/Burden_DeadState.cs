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
        // Disable animator and collider
        if (anim != null)
            anim.enabled = false;
        
        Collider2D col = burden.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Jump up and fall off platform (same as HunchedEnemy)
        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 23);

        // Turn off state machine
        stateMachine.SwitchOffStateMachine();
    }
}
