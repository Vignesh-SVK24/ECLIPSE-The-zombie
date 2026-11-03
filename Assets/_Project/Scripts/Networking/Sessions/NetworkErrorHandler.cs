using System;
using UnityEngine;
using Unity.Netcode;

namespace Eclipse.Networking.Sessions
{
    public class NetworkErrorHandler : MonoBehaviour
    {
        public static event Action<string> OnCriticalNetworkError;

        private void OnEnable()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback += HandleDisconnect;
            }
        }

        private void OnDisable()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleDisconnect;
            }
        }

        private void HandleDisconnect(ulong clientId)
        {
            if (NetworkManager.Singleton != null && clientId == NetworkManager.Singleton.LocalClientId)
            {
                string reason = NetworkManager.Singleton.DisconnectReason;
                if (string.IsNullOrEmpty(reason))
                {
                    reason = "Connection lost to host or server.";
                }

                Debug.LogError($"[ECLIPSE NetworkErrorHandler] Disconnected: {reason}");
                OnCriticalNetworkError?.Invoke(reason);
            }
        }
    }
}
