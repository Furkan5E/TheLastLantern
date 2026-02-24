using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    private TextMeshProUGUI fireflyText;

    private void Start()
    {
        fireflyText = GetComponent<TextMeshProUGUI>();
        CurrencyManager.Instance.OnFirefliesChanged += UpdateUI;
        UpdateUI(CurrencyManager.Instance.Fireflies);
    }

    private void UpdateUI(int amount)
    {
        fireflyText.text = $"Fireflies: {amount}";
    }

    private void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnFirefliesChanged -= UpdateUI;
    }
}
