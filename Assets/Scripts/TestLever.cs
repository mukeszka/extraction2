using UnityEngine;
using FishNet.Connection;

public class TestLever : Interactable
{
    protected override void OnInteract(NetworkConnection interactor)
    {
        Debug.Log($"Pulled by: {interactor?.ClientId}");
    }
}