using UnityEngine;

public class Collectable_Health : MonoBehaviour, ICollectable
{
    [SerializeField] private int amount = 1;

    public void OnCollect(Player player)
    {
        Entity_Health health = player.GetComponent<Entity_Health>();

        if (!health.CanHeal())
            return;

        health.IncreaseHp(amount);
        Destroy(gameObject);
    }
}