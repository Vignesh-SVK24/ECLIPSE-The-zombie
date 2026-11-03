using System;
using UnityEngine;
using UnityEngine.UI;
using Eclipse.Networking.Sessions;
using Eclipse.Networking.Players;
using Unity.Netcode;

namespace Eclipse.Networking.UI
{
    public class NetworkRoomUI : MonoBehaviour
    {
        [Header("Menu Panels")]
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _lobbyRoomPanel;

        [Header("Inputs & Displays")]
        [SerializeField] private InputField _joinCodeInputField;
        [SerializeField] private Text _roomJoinCodeDisplay;
        [SerializeField] private Text _connectionStatusDisplay;
        [SerializeField] private Text _playerListDisplay;

        [Header("Action Buttons")]
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private Button _joinRoomButton;
        [SerializeField] private Button _copyCodeButton;
        [SerializeField] private Button _readyButton;
        [SerializeField] private Button _startMissionButton;
        [SerializeField] private Button _leaveRoomButton;

        private bool _isLocalPlayerReady;

        private void OnEnable()
        {
            if (_createRoomButton != null) _createRoomButton.onClick.AddListener(HandleCreateRoomClicked);
            if (_joinRoomButton != null) _joinRoomButton.onClick.AddListener(HandleJoinRoomClicked);
            if (_copyCodeButton != null) _copyCodeButton.onClick.AddListener(HandleCopyCodeClicked);
            if (_readyButton != null) _readyButton.onClick.AddListener(HandleReadyClicked);
            if (_startMissionButton != null) _startMissionButton.onClick.AddListener(HandleStartMissionClicked);
            if (_leaveRoomButton != null) _leaveRoomButton.onClick.AddListener(HandleLeaveRoomClicked);

            if (NetworkSessionManager.Instance != null)
            {
                NetworkSessionManager.Instance.OnRoomCreated += HandleRoomCreated;
                NetworkSessionManager.Instance.OnRoomJoined += HandleRoomJoined;
                NetworkSessionManager.Instance.OnSessionError += HandleSessionError;
                NetworkSessionManager.Instance.OnSessionLeft += HandleSessionLeft;
            }
        }

        private void OnDisable()
        {
            if (_createRoomButton != null) _createRoomButton.onClick.RemoveListener(HandleCreateRoomClicked);
            if (_joinRoomButton != null) _joinRoomButton.onClick.RemoveListener(HandleJoinRoomClicked);
            if (_copyCodeButton != null) _copyCodeButton.onClick.RemoveListener(HandleCopyCodeClicked);
            if (_readyButton != null) _readyButton.onClick.RemoveListener(HandleReadyClicked);
            if (_startMissionButton != null) _startMissionButton.onClick.RemoveListener(HandleStartMissionClicked);
            if (_leaveRoomButton != null) _leaveRoomButton.onClick.RemoveListener(HandleLeaveRoomClicked);

            if (NetworkSessionManager.Instance != null)
            {
                NetworkSessionManager.Instance.OnRoomCreated -= HandleRoomCreated;
                NetworkSessionManager.Instance.OnRoomJoined -= HandleRoomJoined;
                NetworkSessionManager.Instance.OnSessionError -= HandleSessionError;
                NetworkSessionManager.Instance.OnSessionLeft -= HandleSessionLeft;
            }
        }

        private async void HandleCreateRoomClicked()
        {
            SetStatus("Allocating Relay and creating private room...");
            await NetworkSessionManager.Instance.CreatePrivateRoomAsync();
        }

        private async void HandleJoinRoomClicked()
        {
            if (_joinCodeInputField == null || string.IsNullOrWhiteSpace(_joinCodeInputField.text))
            {
                SetStatus("Please enter a valid 6-character room code.");
                return;
            }

            SetStatus("Connecting to Relay session...");
            await NetworkSessionManager.Instance.JoinRoomWithCodeAsync(_joinCodeInputField.text);
        }

        private void HandleCopyCodeClicked()
        {
            if (!string.IsNullOrEmpty(NetworkSessionManager.Instance.ActiveJoinCode))
            {
                GUIUtility.systemCopyBuffer = NetworkSessionManager.Instance.ActiveJoinCode;
                SetStatus("Room invitation code copied to clipboard!");
            }
        }

        private void HandleReadyClicked()
        {
            _isLocalPlayerReady = !_isLocalPlayerReady;

            var localPlayer = NetworkManager.Singleton?.LocalClient?.PlayerObject?.GetComponent<NetworkPlayer>();
            if (localPlayer != null)
            {
                localPlayer.SetReadyStatus(_isLocalPlayerReady);
            }

            SetStatus(_isLocalPlayerReady ? "You are READY." : "Readiness cleared.");
        }

        private void HandleStartMissionClicked()
        {
            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
            {
                SetStatus("Starting Co-Op Mission: Loading Greyfall Sector...");
                NetworkManager.Singleton.SceneManager.LoadScene("03_Greyfall_Prototype", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }

        private void HandleLeaveRoomClicked()
        {
            NetworkSessionManager.Instance.LeaveSession();
        }

        private void HandleRoomCreated(string joinCode)
        {
            SwitchToLobbyPanel(joinCode, isHost: true);
            SetStatus($"Room active. Share code: {joinCode}");
        }

        private void HandleRoomJoined(string joinCode)
        {
            SwitchToLobbyPanel(joinCode, isHost: false);
            SetStatus($"Joined room. Waiting for host to launch...");
        }

        private void HandleSessionError(string error)
        {
            SetStatus($"Session error: {error}");
        }

        private void HandleSessionLeft()
        {
            if (_mainMenuPanel != null) _mainMenuPanel.SetActive(true);
            if (_lobbyRoomPanel != null) _lobbyRoomPanel.SetActive(false);
            SetStatus("Disconnected from session.");
        }

        private void SwitchToLobbyPanel(string joinCode, bool isHost)
        {
            if (_mainMenuPanel != null) _mainMenuPanel.SetActive(false);
            if (_lobbyRoomPanel != null) _lobbyRoomPanel.SetActive(true);
            if (_roomJoinCodeDisplay != null) _roomJoinCodeDisplay.text = joinCode;
            if (_startMissionButton != null) _startMissionButton.gameObject.SetActive(isHost);
        }

        private void SetStatus(string message)
        {
            if (_connectionStatusDisplay != null)
            {
                _connectionStatusDisplay.text = message;
            }
            Debug.Log($"[ECLIPSE RoomUI] {message}");
        }
    }
}
