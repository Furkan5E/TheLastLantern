using UnityEngine;

public class Ghost_FlyState : EntityState
{
    private Ghost ghost;

    public Ghost_FlyState(Ghost ghost, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.ghost = ghost;
        rb = ghost.rb;
        anim = ghost.anim;
    }

    public override void Update()
    {
        base.Update();

        ghost.Fly();
    }
}
