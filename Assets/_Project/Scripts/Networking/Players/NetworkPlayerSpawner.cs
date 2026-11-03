using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Eclipse.Networking.Players
{
    public class NetworkPlayerSpawner : NetworkBehaviour
    {
        [Header("Prefab Reference")]
        [SerializeField] private GameObject _networkPlayerPrefab;

        [Header("Spawn Points")]
        [SerializeField] private Transform[] _spawnPoints;

        private readonly Dictionary<ulong, NetworkObject> _spawnedPlayers = new Dictionary<ulong, NetworkObject>();

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;

                // Spawn host player
                SpawnPlayerForClient(NetworkManager.ServerClientId);
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer && NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
            }
        }

        private void HandleClientConnected(ulong clientId)
        {
            if (clientId == NetworkManager.ServerClientId) return; // Host already spawned
            SpawnPlayerForClient(clientId);
        }

        private void HandleClientDisconnected(ulong clientId)
        {
            if (_spawnedPlayers.TryGetValue(clientId, out var playerObj))
            {
                if (playerObj != null && playerObj.IsSpawned)
                {
                    playerObj.Despawn(true);
                }
                _spawnedPlayers.Remove(clientId);
            }
        }

        private void SpawnPlayerForClient(ulong clientId)
        {
            Vector3 spawnPos = Vector3.zero;
            Quaternion spawnRot = Quaternion.identity;

            if (_spawnPoints != null && _spawnPoints.Length > 0)
            {
                int index = (int)(clientId % (ulong)_spawnPoints.Length);
                spawnPos = _spawnPoints[index].position;
                spawnRot = _spawnPoints[index].rotation;
            }

            GameObject playerInstance = Instantiate(_networkPlayerPrefab, spawnPos, spawnRot);
            NetworkObject netObj = playerInstance.GetComponent<NetworkObject>();
            netObj.SpawnAsPlayerObject(clientId, true);

            _spawnedPlayers[clientId] = netObj;
        }
    }
}
