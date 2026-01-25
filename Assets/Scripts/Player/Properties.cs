using UnityEngine;

public class Properties : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player component not found");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Item item = other.gameObject.GetComponent<Item>();
        if (item == null)
        {
            Debug.LogError("Item component not found");
            return;
        }

        Debug.Log("Trigger with " + item.itemType);

       switch (item.itemType)
        {
            case ItemType.HealthPotion:
                // Check if already at max health cap
                if (player.maxHealth >= Player.MAX_HEALTH_CAP)
                {
                    Debug.Log("Max health already at cap (" + Player.MAX_HEALTH_CAP + "), cannot collect");
                    return; // Don't collect the item, leave it in the scene
                }
                
                // Increase health but clamp to cap
                player.maxHealth = Mathf.Min(player.maxHealth + item.healthIncrease, Player.MAX_HEALTH_CAP);
                Debug.Log("Max Health: " + player.maxHealth);
                break;
                
            case ItemType.SpeedPotion:
                player.moveSpeed += item.speedIncrease;
                Debug.Log("Move Speed: " + player.moveSpeed);
                break;
        }


        // Remove the collected item
        Destroy(other.gameObject);
    }
}
