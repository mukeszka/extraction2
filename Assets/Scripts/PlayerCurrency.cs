using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PlayerCurrency : NetworkBehaviour
{
    public readonly SyncVar<int> gold = new SyncVar<int>();
    public readonly SyncVar<int> nexusShards = new SyncVar<int>();

    private string playerId;

    private void Awake()
    {
        gold.OnChange += OnGoldChanged;
        nexusShards.OnChange += OnNexusShardsChanged;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (IsOwner)
        {
            string localId = LocalPlayerId.GetOrCreate();
            ServerLoadData(localId);

            CurrencyDisplay display = FindFirstObjectByType<CurrencyDisplay>();
            if (display != null)
            {
                display.Bind(this);
            }
        }
    }

    [ServerRpc]
    private void ServerLoadData(string requestedPlayerId)
    {
        playerId = requestedPlayerId;

        PlayerData data = PlayerDataService.Load(playerId);
        gold.Value = data.gold;
        nexusShards.Value = data.nexusShards;
    }

    public void ServerAddGold(int amount)
    {
        if (!IsServerInitialized) return;

        gold.Value += amount;
        SaveToDisk();
    }

    public bool ServerTrySpendGold(int amount)
    {
        if (!IsServerInitialized) return false;

        if (gold.Value < amount) return false;

        gold.Value -= amount;
        SaveToDisk();
        return true;
    }

    public void ServerAddNexusShards(int amount)
    {
        if (!IsServerInitialized) return;

        nexusShards.Value += amount;
        SaveToDisk();
    }

    private void SaveToDisk()
    {
        if (string.IsNullOrEmpty(playerId)) return;

        PlayerData data = new PlayerData
        {
            playerId = playerId,
            gold = gold.Value,
            nexusShards = nexusShards.Value
        };

        PlayerDataService.Save(data);
    }

    private void OnGoldChanged(int oldValue, int newValue, bool asServer)
    {
        Debug.Log($"Gold changed: {oldValue} -> {newValue}");
    }

    private void OnNexusShardsChanged(int oldValue, int newValue, bool asServer)
    {
        Debug.Log($"Nexus Shards changed: {oldValue} -> {newValue}");
    }
}