using UnityEngine;
using Unity.Netcode;
using Eclipse.Damage;

namespace Eclipse.Networking.Authority
{
    public class NetworkAuthorityManager : NetworkBehaviour, INetworkAuthority
    {
        private static NetworkAuthorityManager _instance;
        public static NetworkAuthorityManager Instance => _instance;

        public bool IsServerOrHost => IsServer;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
        }

        public bool ValidateDamageRequest(ulong requestingClientId, DamageInfo info, GameObject target)
        {
            if (!IsServer) return false;

            if (target == null || info.Amount <= 0f) return false;

            // Verify requesting client owns the damage source
            if (info.Source != null && info.Source.TryGetComponent<NetworkObject>(out var netSource))
            {
                if (netSource.OwnerClientId != requestingClientId)
                {
                    Debug.LogWarning($"[ECLIPSE NetworkAuthority] Damage rejected: client {requestingClientId} does not own damage source.");
                    return false;
                }
            }

            return true;
        }

        public bool ValidateLootPickup(ulong requestingClientId, string itemId, GameObject container)
        {
            if (!IsServer) return false;
            if (string.IsNullOrEmpty(itemId) || container == null) return false;

            return true;
        }

        public bool ValidateReload(ulong requestingClientId, int requestedAmmunition)
        {
            if (!IsServer) return false;
            return requestedAmmunition > 0 && requestedAmmunition <= 100;
        }

        public bool ValidateRevive(ulong rescuerClientId, ulong downedClientId)
        {
            if (!IsServer) return false;
            return rescuerClientId != downedClientId;
        }
    }
}
