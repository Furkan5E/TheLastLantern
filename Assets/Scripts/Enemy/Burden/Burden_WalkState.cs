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
        // base.Update();

        // Use the new waypoint-based movement system
        burden.Move();
    }

}
