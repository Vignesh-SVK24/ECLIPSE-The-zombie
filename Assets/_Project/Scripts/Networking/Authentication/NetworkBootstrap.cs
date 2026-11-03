using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;

namespace Eclipse.Networking.Authentication
{
    public class NetworkBootstrap : MonoBehaviour
    {
        private static NetworkBootstrap _instance;
        public static NetworkBootstrap Instance => _instance;

        private bool _isInitialized;
        private string _playerId;

        public bool IsInitialized => _isInitialized;
        public string PlayerId => _playerId;

        public event Action<string> OnAuthenticationSuccess;
        public event Action<string> OnAuthenticationFailure;

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

        private async void Start()
        {
            await InitializeServicesAsync();
        }

        public async Task<bool> InitializeServicesAsync()
        {
            if (_isInitialized) return true;

            try
            {
                await UnityServices.InitializeAsync();

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }

                _playerId = AuthenticationService.Instance.PlayerId;
                _isInitialized = true;

                Debug.Log($"[ECLIPSE NetworkBootstrap] Authenticated successfully. PlayerId: {_playerId}");
                OnAuthenticationSuccess?.Invoke(_playerId);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[ECLIPSE NetworkBootstrap] Authentication failed: {ex.Message}");
                OnAuthenticationFailure?.Invoke(ex.Message);
                return false;
            }
        }
    }
}
