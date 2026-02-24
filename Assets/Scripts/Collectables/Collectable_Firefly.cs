using UnityEngine;

public class Collectable_Firefly : MonoBehaviour, ICollectable
{
    [SerializeField] private int amount = 1;

    public void OnCollect(Player player)
    {
        CurrencyManager currencyManager = CurrencyManager.Instance;
        if (currencyManager != null){
            currencyManager.Add(amount);
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
            Debug.LogWarning("currencyManager is null");
    }
}