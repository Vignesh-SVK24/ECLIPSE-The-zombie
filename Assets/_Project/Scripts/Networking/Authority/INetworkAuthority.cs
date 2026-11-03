using UnityEngine;
using Eclipse.Damage;

namespace Eclipse.Networking.Authority
{
    public interface INetworkAuthority
    {
        bool IsServerOrHost { get; }
        bool ValidateDamageRequest(ulong requestingClientId, DamageInfo info, GameObject target);
        bool ValidateLootPickup(ulong requestingClientId, string itemId, GameObject container);
        bool ValidateReload(ulong requestingClientId, int requestedAmmunition);
        bool ValidateRevive(ulong rescuerClientId, ulong downedClientId);
    }
}
