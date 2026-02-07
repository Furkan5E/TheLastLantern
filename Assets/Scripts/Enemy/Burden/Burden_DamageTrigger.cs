using UnityEngine;

/// <summary>
/// Attach this to a child GameObject with a trigger collider for damage detection
/// </summary>
public class Burden_DamageTrigger : MonoBehaviour
{
    private Burden burden;

    private void Awake()
    {
        burden = GetComponentInParent<Burden>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            burden.OnPlayerEnterDamageTrigger(collision.gameObject);
        }
    }
}
