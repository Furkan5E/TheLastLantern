using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damage, Transform damageDealer);
}