using UnityEngine;
using FishNet.Object;

public class PlayerInteraction : NetworkBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 2.5f;

    private Interactable currentTarget;

    private void Update()
    {
        if (!IsOwner) return;

        Interactable closest = Interactable.FindClosest(transform.position, interactionRange);

        if (closest != currentTarget)
        {
            currentTarget?.HidePrompt();
            currentTarget = closest;
            currentTarget?.ShowPrompt();
        }

        if (currentTarget != null && Input.GetKeyDown(KeyCode.E))
        {
            currentTarget.RequestInteract();
        }
    }
}