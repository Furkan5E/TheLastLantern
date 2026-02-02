using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();    
    }

    public override void TakeDamage(float damage, Transform damageDealer)
    {
        base.TakeDamage(damage, damageDealer);
        if(isDead)
            return;

        if(damageDealer.GetComponent<Player>() != null)
            enemy.EnterBattleState(damageDealer);
    }
}