using UnityEngine;

public class Burden_WalkState : EntityState
{
    private Burden burden;

    public Burden_WalkState(Burden burden, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.burden = burden;
        rb = burden.rb;
        anim = burden.anim;
    }

    public override void Update()
    {
        base.Update();

        // Check for edges/walls and flip
        if (burden.groundDetected == false || burden.wallDetected)
            burden.Flip();

        // Move horizontally
        burden.SetVelocity(burden.moveSpeed * burden.facingDir, rb.linearVelocity.y);
    }

}
