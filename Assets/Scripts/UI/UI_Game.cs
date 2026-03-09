using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Game : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private Entity_Health playerHealth;
    [SerializeField] private Image[] masks;
    [SerializeField] private Sprite fullMaskSprite;
    [SerializeField] private Sprite emptyMaskSprite;

    [Header("Currency Settings")]
    [SerializeField] private TextMeshProUGUI fireflyText;

    private void Start()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHearts;
            UpdateHearts(playerHealth.currentHp, playerHealth.MaxHp);
        }

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnFirefliesChanged += UpdateFirefliesUI;
            UpdateFirefliesUI(CurrencyManager.Instance.Fireflies);
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHearts;
        }

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnFirefliesChanged -= UpdateFirefliesUI;
        }
    }

    private void UpdateHearts(float current, float max)
    {
        int currentHp = Mathf.RoundToInt(current);
        int maxHp = Mathf.RoundToInt(max);

        for (int i = 0; i < masks.Length; i++)
        {
            masks[i].enabled = i < maxHp;

            if (masks[i].enabled)
            {
                masks[i].sprite = (i < currentHp) ? fullMaskSprite : emptyMaskSprite;
            }
        }
    }

    private void UpdateFirefliesUI(int amount)
    {
        if (fireflyText != null)
        {
            fireflyText.text = amount.ToString();
        }
    }
}