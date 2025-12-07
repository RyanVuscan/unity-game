using UnityEngine;
using Unity.Netcode;

public class NetworkPlayerSpawner : NetworkBehaviour
{
    [SerializeField] Vector3[] spawnPositions = new Vector3[]
    {
        new Vector3(0, 1, 0),
        new Vector3(5, 1, 5),
        new Vector3(-5, 1, 5),
        new Vector3(5, 1, -5)
    };

    int spawnIndex = 0;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    void OnClientConnected(ulong clientId)
    {
        // Spawn player at next spawn position
        Vector3 spawnPos = spawnPositions[spawnIndex % spawnPositions.Length];
        spawnIndex++;

        // The player prefab will be spawned automatically by NetworkManager
        // This is just for tracking spawn positions if needed
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }
}
