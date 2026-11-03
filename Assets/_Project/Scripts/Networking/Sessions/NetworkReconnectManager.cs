using System;
using System.Collections;
using UnityEngine;

namespace Eclipse.Networking.Sessions
{
    public class NetworkReconnectManager : MonoBehaviour
    {
        [Header("Reconnection Settings")]
        [SerializeField] private int _maxAttempts = 3;
        [SerializeField] private float _delayBetweenAttempts = 4.0f;

        private int _currentAttempt = 0;
        private bool _isReconnecting = false;
        private string _cachedJoinCode;

        public event Action<int, int> OnReconnectAttempt;
        public event Action OnReconnectFailed;

        public void AttemptReconnection(string joinCode)
        {
            if (_isReconnecting) return;
            _cachedJoinCode = joinCode;
            _currentAttempt = 0;
            StartCoroutine(ReconnectRoutine());
        }

        private IEnumerator ReconnectRoutine()
        {
            _isReconnecting = true;

            while (_currentAttempt < _maxAttempts)
            {
                _currentAttempt++;
                OnReconnectAttempt?.Invoke(_currentAttempt, _maxAttempts);
                Debug.Log($"[ECLIPSE NetworkReconnectManager] Reconnection attempt {_currentAttempt}/{_maxAttempts}...");

                var task = NetworkSessionManager.Instance.JoinRoomWithCodeAsync(_cachedJoinCode);
                while (!task.IsCompleted)
                {
                    yield return null;
                }

                if (task.Result)
                {
                    Debug.Log("[ECLIPSE NetworkReconnectManager] Reconnected successfully!");
                    _isReconnecting = false;
                    yield break;
                }

                yield return new WaitForSeconds(_delayBetweenAttempts);
            }

            _isReconnecting = false;
            Debug.LogError("[ECLIPSE NetworkReconnectManager] All reconnection attempts failed.");
            OnReconnectFailed?.Invoke();
        }
    }
}
