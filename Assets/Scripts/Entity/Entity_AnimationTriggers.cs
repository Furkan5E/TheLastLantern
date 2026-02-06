using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat entityCombat;
    
    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
        entityCombat = GetComponentInParent<Entity_Combat>();
    }

    private void CurrentStateAnimationTrigger()
    {
        entity.CurrentStateAnimationTrigger();
    }

    protected virtual void AttackTrigger()
    {
        entityCombat.PerformAttack();
    }
}
