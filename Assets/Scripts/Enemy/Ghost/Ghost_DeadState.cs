using UnityEngine;

public class Ghost_DeadState : EntityState
{
    private Ghost ghost;

    public Ghost_DeadState(Ghost ghost, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.ghost = ghost;
        rb = ghost.rb;
        anim = ghost.anim;
    }

    public override void Enter()
    {
        // Disable animator and collider
        if (anim != null)
            anim.enabled = false;

        Collider2D col = ghost.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Jump up and fall off screen
        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 23);

        // Turn off state machine
        stateMachine.SwitchOffStateMachine();
    }
}
