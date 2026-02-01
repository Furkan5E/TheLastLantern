using UnityEngine;

public class Entity_Health : MonoBehaviour
{
    [SerializeField] protected float maxHp = 5;
    protected bool isDead;

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if(isDead)
            return;

        ReduceHp(damage);
    }

    protected void ReduceHp(float damage)
    {
        maxHp -= damage;

        if(maxHp < 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Dead");
    }
}