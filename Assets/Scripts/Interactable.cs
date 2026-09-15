using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;

public abstract class Interactable : NetworkBehaviour
{
    [Header("Interaction Settings")]
    public string promptText = "Press E";
    public float interactionRange = 2.5f;

    [Header("World Space Prompt")]
    public GameObject promptVisual; 

    private static readonly List<Interactable> all = new List<Interactable>();

    protected virtual void OnEnable()
    {
        all.Add(this);
        HidePrompt();
    }

    protected virtual void OnDisable()
    {
        all.Remove(this);
    }

    public void ShowPrompt()
    {
        if (promptVisual != null)
            promptVisual.SetActive(true);
    }

    public void HidePrompt()
    {
        if (promptVisual != null)
            promptVisual.SetActive(false);
    }

    public static Interactable FindClosest(Vector3 fromPosition, float maxRange)
    {
        Interactable closest = null;
        float closestDist = maxRange;

        foreach (var interactable in all)
        {
            float dist = Vector3.Distance(fromPosition, interactable.transform.position);
            if (dist <= interactable.interactionRange && dist <= closestDist)
            {
                closest = interactable;
                closestDist = dist;
            }
        }

        return closest;
    }

    public void RequestInteract()
    {
        ServerInteract();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ServerInteract(NetworkConnection sender = null)
    {
        OnInteract(sender);
    }

    protected abstract void OnInteract(NetworkConnection interactor);
}