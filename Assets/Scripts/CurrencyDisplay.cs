using UnityEngine;
using TMPro;

public class CurrencyDisplay : MonoBehaviour
{
    public TextMeshProUGUI label;

    private PlayerCurrency trackedCurrency;

    public void Bind(PlayerCurrency currency)
    {
        trackedCurrency = currency;

        trackedCurrency.gold.OnChange += OnValueChanged;
        trackedCurrency.nexusShards.OnChange += OnValueChanged;

        UpdateText();
    }

    private void OnValueChanged(int oldValue, int newValue, bool asServer)
    {
        UpdateText();
    }

    private void UpdateText()
    {
        if (trackedCurrency == null || label == null) return;

        label.text = $"Gold: {trackedCurrency.gold.Value} | Nexus Shards: {trackedCurrency.nexusShards.Value}";
    }
}