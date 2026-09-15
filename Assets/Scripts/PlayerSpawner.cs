using UnityEngine;
using FishNet.Connection;
using FishNet.Object;

public class PlayerSpawner : NetworkBehaviour
{
    [Header("Player Prefab")]
    public NetworkObject playerPrefab;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    public override void OnStartServer()
    {
        base.OnStartServer();

        // Amikor egy új kliens csatlakozik, ezt az eseményt hívja meg FishNet
        base.ServerManager.OnRemoteConnectionState += ServerManager_OnRemoteConnectionState;
    }

    private void ServerManager_OnRemoteConnectionState(NetworkConnection conn, FishNet.Transporting.RemoteConnectionStateArgs args)
    {
        if (args.ConnectionState == FishNet.Transporting.RemoteConnectionState.Started)
        {
            SpawnPlayer(conn);
        }
    }

    private void SpawnPlayer(NetworkConnection conn)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Nincs beállítva spawn pont a PlayerSpawner-en!");
            return;
        }

        Transform chosenSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        NetworkObject nob = Instantiate(playerPrefab, chosenSpawn.position, chosenSpawn.rotation);
        base.Spawn(nob, conn);
    }
}