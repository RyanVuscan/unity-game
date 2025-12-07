using UnityEngine;
using Unity.Netcode;

public class NetworkPrefabRegistrar : MonoBehaviour
{
    [SerializeField] GameObject[] prefabsToRegister;

    void Awake()
    {
        // Register prefabs before NetworkManager starts
        if (NetworkManager.Singleton != null)
        {
            foreach (var prefab in prefabsToRegister)
            {
                if (prefab == null) continue;
                
                var netObj = prefab.GetComponent<NetworkObject>();
                if (netObj == null)
                {
                    Debug.LogWarning($"Prefab {prefab.name} has no NetworkObject - skipping");
                    continue;
                }
                
                // Check if already registered
                bool alreadyRegistered = false;
                foreach (var registered in NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs)
                {
                    if (registered.Prefab == prefab)
                    {
                        alreadyRegistered = true;
                        break;
                    }
                }
                
                if (!alreadyRegistered)
                {
                    NetworkManager.Singleton.AddNetworkPrefab(prefab);
                    Debug.Log($"Registered network prefab: {prefab.name}");
                }
            }
        }
    }
}
