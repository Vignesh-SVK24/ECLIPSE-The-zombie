using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;

namespace Eclipse.Networking.Sessions
{
    public class NetworkSessionManager : MonoBehaviour
    {
        private static NetworkSessionManager _instance;
        public static NetworkSessionManager Instance => _instance;

        [Header("Configuration")]
        [SerializeField] private NetworkSessionConfig _config;

        private string _activeJoinCode;
        private string _activeLobbyId;
        private bool _isHosting;

        public string ActiveJoinCode => _activeJoinCode;
        public string ActiveLobbyId => _activeLobbyId;
        public bool IsHosting => _isHosting;
        public bool IsInSession => NetworkManager.Singleton != null && (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer);

        public event Action<string> OnRoomCreated;
        public event Action<string> OnRoomJoined;
        public event Action<string> OnSessionError;
        public event Action OnSessionLeft;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public async Task<string> CreatePrivateRoomAsync()
        {
            try
            {
                int maxConnections = _config != null ? _config.MaxPlayers - 1 : 3;

                // 1. Allocate Relay
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections, _config?.PreferredRelayRegion);
                _activeJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

                // 2. Configure Unity Transport
                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetHostRelayData(
                    allocation.RelayServer.IpV4,
                    (ushort)allocation.RelayServer.Port,
                    allocation.AllocationIdBytes,
                    allocation.Key,
                    allocation.ConnectionData
                );

                // 3. Register Lobby
                var lobbyOptions = new CreateLobbyOptions
                {
                    IsPrivate = true,
                    Data = new Dictionary<string, DataObject>
                    {
                        { "JoinCode", new DataObject(DataObject.VisibilityOptions.Member, _activeJoinCode) }
                    }
                };

                Lobby lobby = await LobbyService.Instance.CreateLobbyAsync("GreyfallSafehouse", _config != null ? _config.MaxPlayers : 4, lobbyOptions);
                _activeLobbyId = lobby.Id;

                // 4. Start Host
                NetworkManager.Singleton.StartHost();
                _isHosting = true;

                Debug.Log($"[ECLIPSE NetworkSessionManager] Private room created. JoinCode: {_activeJoinCode}");
                OnRoomCreated?.Invoke(_activeJoinCode);
                return _activeJoinCode;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[ECLIPSE NetworkSessionManager] Failed to create room: {ex.Message}");
                OnSessionError?.Invoke(ex.Message);
                return null;
            }
        }

        public async Task<bool> JoinRoomWithCodeAsync(string joinCode)
        {
            if (string.IsNullOrWhiteSpace(joinCode))
            {
                OnSessionError?.Invoke("Invalid Join Code.");
                return false;
            }

            try
            {
                string cleanCode = joinCode.Trim().ToUpper();

                // 1. Join Relay Allocation
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(cleanCode);

                // 2. Configure Unity Transport for Client
                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                transport.SetClientRelayData(
                    joinAllocation.RelayServer.IpV4,
                    (ushort)joinAllocation.RelayServer.Port,
                    joinAllocation.AllocationIdBytes,
                    joinAllocation.Key,
                    joinAllocation.ConnectionData,
                    joinAllocation.HostConnectionData
                );

                // 3. Start Client
                NetworkManager.Singleton.StartClient();
                _activeJoinCode = cleanCode;
                _isHosting = false;

                Debug.Log($"[ECLIPSE NetworkSessionManager] Joined room with code: {cleanCode}");
                OnRoomJoined?.Invoke(cleanCode);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[ECLIPSE NetworkSessionManager] Failed to join room: {ex.Message}");
                OnSessionError?.Invoke(ex.Message);
                return false;
            }
        }

        public void LeaveSession()
        {
            if (NetworkManager.Singleton != null && NetworkManager.Singleton.isActiveAndEnabled)
            {
                NetworkManager.Singleton.Shutdown();
            }

            _activeJoinCode = string.Empty;
            _activeLobbyId = string.Empty;
            _isHosting = false;

            OnSessionLeft?.Invoke();
        }
    }
}
