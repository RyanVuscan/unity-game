using Unity.Netcode;
using UnityEngine;

public class NetworkedEnemy : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        // Only the server controls the enemy AI
        if (!IsServer)
        {
            // Disable AI on clients, just show the visuals
            var ai = GetComponent<ElementalEnemyAI>();
            if (ai != null)
                ai.enabled = false;
        }
    }
}
