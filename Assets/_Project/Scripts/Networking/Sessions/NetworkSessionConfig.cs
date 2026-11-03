using System;
using UnityEngine;

namespace Eclipse.Networking.Sessions
{
    [Serializable]
    [CreateAssetMenu(fileName = "NetworkSessionConfig", menuName = "ECLIPSE/Networking/Session Configuration")]
    public class NetworkSessionConfig : ScriptableObject
    {
        [Header("Room Capacity")]
        [Range(2, 4)]
        [SerializeField] private int _maxPlayers = 4;
        [Range(1, 2)]
        [SerializeField] private int _minPlayers = 2;

        [Header("Connection Parameters")]
        [SerializeField] private float _connectionTimeoutSeconds = 12f;
        [SerializeField] private float _heartbeatIntervalSeconds = 15f;
        [SerializeField] private float _reconnectWindowSeconds = 20f;

        [Header("Relay Settings")]
        [SerializeField] private string _preferredRelayRegion = ""; // empty = auto-best latency
        [SerializeField] private bool _enableEncryption = true;

        public int MaxPlayers => _maxPlayers;
        public int MinPlayers => _minPlayers;
        public float ConnectionTimeoutSeconds => _connectionTimeoutSeconds;
        public float HeartbeatIntervalSeconds => _heartbeatIntervalSeconds;
        public float ReconnectWindowSeconds => _reconnectWindowSeconds;
        public string PreferredRelayRegion => _preferredRelayRegion;
        public bool EnableEncryption => _enableEncryption;
    }
}
