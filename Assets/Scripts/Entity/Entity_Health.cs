using UnityEngine;

public class Entity_Health : MonoBehaviour
{
    private Entity_VFX entityVfx;
    [SerializeField] protected float maxHp = 5;
    protected bool isDead;
    
    protected virtual void Awake()
    {
        entityVfx = GetComponent<Entity_VFX>();
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if(isDead)
            return;

        if(entityVfx != null)
            entityVfx.PlayOnDamgeVfx();
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