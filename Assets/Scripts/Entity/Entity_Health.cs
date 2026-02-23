using System;
using Unity.VisualScripting;
using UnityEngine;

public class Entity_Health : MonoBehaviour, IDamageable
{
    private Entity entity;
    private Entity_VFX entityVfx;
    [SerializeField] protected float maxHp = 5;
    public float currentHp { get; private set; }
    protected bool isDead;

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private float knockbackDuration = 0.2f;
    
    protected virtual void Awake()
    {
        currentHp = maxHp;
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<Entity_VFX>();
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if(isDead)
            return;

        Vector2 knockback = CalculateKnockback(damageDealer);

        if(entity != null)
            entity.ReciveKnockback(knockback, knockbackDuration);

        if(entityVfx != null)
            entityVfx.PlayOnDamgeVfx();
        ReduceHp(damage);
    }

    protected void ReduceHp(float damage)
    {
        currentHp -= damage;

        if(currentHp < 0)
            Die();
    }

    public void IncreaseHp(float healAmount)
    {
        if (!CanHeal())
            return;

        currentHp = Mathf.Min(currentHp + healAmount, maxHp);
    }

    public bool CanHeal()
    {
        return !isDead && currentHp < maxHp;
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    private Vector2 CalculateKnockback(Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1:-1;

        Vector2 knockback = knockbackPower;
        knockback.x *= direction;

        return knockback;
    }
}