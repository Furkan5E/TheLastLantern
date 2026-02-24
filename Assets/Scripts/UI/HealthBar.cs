using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Entity_Health playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;

    private void Start()
    {
        playerHealth.OnHealthChanged += UpdateHearts;
        UpdateHearts(playerHealth.currentHp, playerHealth.MaxHp);
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateHearts;
    }

    private void UpdateHearts(float current, float max)
    {
        currenthealthBar.fillAmount = current/10;
        totalhealthBar.fillAmount = max/10;
    }
}