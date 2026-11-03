using System;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

namespace Eclipse.Networking.Players
{
    public class NetworkPlayer : NetworkBehaviour, IPlayerIdentity
    {
        [Header("Local Components to Disable on Remote Proxies")]
        [SerializeField] private Behaviour[] _localOnlyComponents;
        [SerializeField] private GameObject _localCameraRig;

        private readonly NetworkVariable<FixedString64Bytes> _networkPlayerName = new NetworkVariable<FixedString64Bytes>(
            new FixedString64Bytes("Survivor"),
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        private readonly NetworkVariable<bool> _isReady = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        private readonly NetworkVariable<float> _networkHealth = new NetworkVariable<float>(
            100f,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server);

        public ulong ClientId => OwnerClientId;
        public string PlayerName => _networkPlayerName.Value.ToString();
        public bool IsReady => _isReady.Value;
        public bool IsHost => IsServer && IsOwner;
        public int PingMilliseconds => (int)(NetworkManager.Singleton != null ? NetworkManager.Singleton.NetworkTimeSystem.ServerTime : 0);
        public GameObject PlayerGameObject => gameObject;
        public float NetworkHealth => _networkHealth.Value;

        public event Action<string> OnPlayerNameUpdated;
        public event Action<bool> OnReadyStatusUpdated;
        public event Action<float> OnHealthUpdated;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            _networkPlayerName.OnValueChanged += HandleNameChanged;
            _isReady.OnValueChanged += HandleReadyChanged;
            _networkHealth.OnValueChanged += HandleHealthChanged;

            if (!IsOwner)
            {
                // Remote proxy: Disable local cameras, listeners, and input routers
                DisableLocalOnlyComponents();
            }
            else
            {
                // Local owner: Set initial player name
                string localName = PlayerPrefs.GetString("ECLIPSE_PlayerName", "Survivor_" + UnityEngine.Random.Range(100, 999));
                _networkPlayerName.Value = new FixedString64Bytes(localName);
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            _networkPlayerName.OnValueChanged -= HandleNameChanged;
            _isReady.OnValueChanged -= HandleReadyChanged;
            _networkHealth.OnValueChanged -= HandleHealthChanged;
        }

        private void HandleNameChanged(FixedString64Bytes oldVal, FixedString64Bytes newVal)
        {
            OnPlayerNameUpdated?.Invoke(newVal.ToString());
        }

        private void HandleReadyChanged(bool oldVal, bool newVal)
        {
            OnReadyStatusUpdated?.Invoke(newVal);
        }

        private void HandleHealthChanged(float oldVal, float newVal)
        {
            OnHealthUpdated?.Invoke(newVal);
        }

        private void DisableLocalOnlyComponents()
        {
            if (_localCameraRig != null)
            {
                _localCameraRig.SetActive(false);
            }

            if (_localOnlyComponents != null)
            {
                foreach (var comp in _localOnlyComponents)
                {
                    if (comp != null) comp.enabled = false;
                }
            }
        }

        public void SetReadyStatus(bool ready)
        {
            if (IsOwner)
            {
                _isReady.Value = ready;
            }
        }

        [ServerRpc]
        public void RequestDamageServerRpc(float amount, ulong targetNetworkObjectId)
        {
            // Server validates and applies damage authoritatively
            if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(targetNetworkObjectId, out var targetObj))
            {
                if (targetObj.TryGetComponent<Damage.IDamageable>(out var damageable))
                {
                    Damage.DamageInfo info = new Damage.DamageInfo(
                        amount: amount,
                        type: Damage.DamageType.Bullet,
                        source: gameObject);
                    damageable.TakeDamage(info);
                }
            }
        }
    }
}
