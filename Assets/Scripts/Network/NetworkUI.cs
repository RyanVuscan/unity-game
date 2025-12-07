using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour
{
    [SerializeField] Button hostButton;
    [SerializeField] Button clientButton;
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject playerPrefab;

    void Start()
    {
        if (hostButton != null)
            hostButton.onClick.AddListener(OnHostClicked);
        
        if (clientButton != null)
            clientButton.onClick.AddListener(OnClientClicked);
    }

    void OnHostClicked()
    {
        Debug.Log("Starting Host...");
        
        // Subscribe before starting so we catch the host's own connection
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        
        bool success = NetworkManager.Singleton.StartHost();
        
        if (success)
        {
            Debug.Log("Host started successfully!");
            if (menuPanel != null) menuPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Failed to start host!");
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    void OnClientClicked()
    {
        Debug.Log("Connecting to server...");
        bool success = NetworkManager.Singleton.StartClient();
        
        if (success)
        {
            Debug.Log("Client connecting...");
            if (menuPanel != null) menuPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Failed to start client!");
        }
    }
    
    void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} connected - spawning player");
        
        // Only server spawns players
        if (!NetworkManager.Singleton.IsServer) return;
        
        // Load prefab if not assigned
        if (playerPrefab == null)
        {
            playerPrefab = Resources.Load<GameObject>("Player_New");
        }
        
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab not found!");
            return;
        }
        
        // Spawn at random position
        Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(-3f, 3f), 1f, UnityEngine.Random.Range(-3f, 3f));
        GameObject player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        
        NetworkObject netObj = player.GetComponent<NetworkObject>();
        if (netObj != null)
        {
            // THIS IS THE KEY - spawn with ownership to the connecting client
            netObj.SpawnAsPlayerObject(clientId);
            Debug.Log($"Spawned player for client {clientId} with ownership");
        }
    }
    
    void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }
}
