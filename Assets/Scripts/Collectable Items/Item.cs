using UnityEngine;

public enum ItemType
{
    HealthPotion,
    SpeedPotion
}

public class Item : MonoBehaviour
{
    [Header("Item Settings")]
    public ItemType itemType;

    [Header("Item Effects")]
    public int healthIncrease = 1;
    public float speedIncrease = 1.0f;
}