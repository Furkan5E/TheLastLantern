using UnityEngine;

/// <summary>
/// Attach this to a child GameObject with a trigger collider for damage detection.
/// </summary>
public class Ghost_DamageTrigger : MonoBehaviour
{
    private Ghost ghost;

    private void Awake()
    {
        ghost = GetComponentInParent<Ghost>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            ghost.OnPlayerEnterDamageTrigger(collision.gameObject);
        }
    }
}
