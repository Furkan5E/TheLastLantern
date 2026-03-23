using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Jump Attack")]
    [SerializeField] private Transform downTargetCheck;
    [SerializeField] private float downCheckRadius = 1f;

    public bool PerformDownAttack()
    {
        if (downTargetCheck == null)
            return false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(downTargetCheck.position, downCheckRadius, whatIsTarget);
        foreach (var target in colliders)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable == null)
                continue;

            damageable.TakeDamage(damage, transform);
            
            if (vfx != null)
                vfx.CreateOnHitVfx(target.transform);
        }
        return colliders.Length > 0;
    }

    public bool CounterAttackPerformed()
    {
        bool hasCountered = false;

        foreach (var target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();
            if(counterable != null)
            {
                counterable.HandleCounter();
                hasCountered = true;
            }
        }
        return hasCountered;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(downTargetCheck.position, downCheckRadius);
    }
}