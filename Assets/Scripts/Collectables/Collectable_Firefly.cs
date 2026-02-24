using UnityEngine;

public class Collectable_Firefly : MonoBehaviour, ICollectable
{
    [SerializeField] private int amount = 1;

    public void OnCollect(Player player)
    {
        CurrencyManager.Instance.Add(amount);
        gameObject.SetActive(false);
    }
}