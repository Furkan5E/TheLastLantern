using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Jump Attack")]
    [SerializeField] private Transform downTargetCheck;
    [SerializeField] private float downCheckRadius = 1f;

    /// <summary>
    /// casts a downward overlap circle to detect and damage entities below the player
    /// returns true if a valid target is struck, signaling the animation trigger to apply a bounce effect
    /// </summary>
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

    /// <summary>
    /// checks the forward attack radius for counterable objects
    /// </summary>
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