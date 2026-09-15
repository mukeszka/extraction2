using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance;

    [Header("UI References")]
    public GameObject promptRoot;
    public TextMeshProUGUI promptLabel;

    private void Awake()
    {
        Instance = this;

        if (promptRoot != null)
            promptRoot.SetActive(false);
    }

    public void ShowPrompt(string text)
    {
        if (promptRoot != null)
            promptRoot.SetActive(true);

        if (promptLabel != null)
            promptLabel.text = text;
    }

    public void HidePrompt()
    {
        if (promptRoot != null)
            promptRoot.SetActive(false);
    }
}