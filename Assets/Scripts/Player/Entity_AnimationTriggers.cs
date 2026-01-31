using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity entity;
    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    public void CurrentStateAnimationTrigger()
    {
        entity.CurrentStateAnimationTrigger();
    }
}
