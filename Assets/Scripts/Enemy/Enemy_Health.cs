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
        if(damageDealer.GetComponent<Player>() != null)
            enemy.EnterBattleState(damageDealer);


        base.TakeDamage(damage, damageDealer);
    }
}