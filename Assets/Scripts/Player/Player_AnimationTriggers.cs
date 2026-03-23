using UnityEngine;

public class Player_AnimationTriggers: Entity_AnimationTriggers
{
    private Player_Combat playerCombat;

    protected override void Awake()
    {
        base.Awake();
        playerCombat = GetComponentInParent<Player_Combat>();
    }

    protected override void AttackTrigger()
    {
        base.AttackTrigger();
        playerCombat.CounterAttackPerformed();
    }

    public void JumpAttackTrigger()
    {
        bool hitSomething = playerCombat.PerformDownAttack();
        
        if (hitSomething)
        {
            Player player = entity as Player;
            if (player != null)
                player.SetVelocity(player.rb.linearVelocity.x, player.jumpAttackForce); 
        }
    }
}