using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Eclipse.Mobile
{
    public class MobileInputManager : MonoBehaviour
    {
        private static MobileInputManager _instance;
        public static MobileInputManager Instance => _instance;

        [Header("Mobile Touch Elements")]
        [SerializeField] private VirtualJoystick _virtualJoystick;
        [SerializeField] private MobileCameraLook _cameraLookArea;
        [SerializeField] private CanvasGroup _mobileControlsCanvasGroup;

        [Header("Mobile Action Buttons")]
        [SerializeField] private MobileActionButton _fireButton;
        [SerializeField] private MobileActionButton _aimButton;
        [SerializeField] private MobileActionButton _reloadButton;
        [SerializeField] private MobileActionButton _sprintButton;
        [SerializeField] private MobileActionButton _switchWeaponButton;
        [SerializeField] private MobileActionButton _interactButton;
        [SerializeField] private MobileActionButton _inventoryButton;
        [SerializeField] private MobileActionButton _pauseButton;
        [SerializeField] private MobileActionButton _reviveButton;

        // Current Input States
        private Vector2 _moveInput;
        private Vector2 _lookDelta;
        private bool _isFiring;
        private bool _isAiming;
        private bool _isSprinting;

        public Vector2 MoveInput => _moveInput;
        public Vector2 LookDelta => _lookDelta;
        public bool IsFiring => _isFiring;
        public bool IsAiming => _isAiming;
        public bool IsSprinting => _isSprinting;

        public event Action OnReloadAction;
        public event Action OnInteractAction;
        public event Action OnWeaponSwitchAction;
        public event Action OnInventoryAction;
        public event Action OnPauseAction;
        public event Action OnReviveAction;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);

            ConfigurePlatformVisibility();
        }

        private void OnEnable()
        {
            SubscribeButtons();
        }

        private void OnDisable()
        {
            UnsubscribeButtons();
        }

        private void SubscribeButtons()
        {
            if (_reloadButton != null) _reloadButton.OnTriggered += HandleReloadTriggered;
            if (_interactButton != null) _interactButton.OnTriggered += HandleInteractTriggered;
            if (_switchWeaponButton != null) _switchWeaponButton.OnTriggered += HandleWeaponSwitchTriggered;
            if (_inventoryButton != null) _inventoryButton.OnTriggered += HandleInventoryTriggered;
            if (_pauseButton != null) _pauseButton.OnTriggered += HandlePauseTriggered;
            if (_reviveButton != null) _reviveButton.OnTriggered += HandleReviveTriggered;
        }

        private void UnsubscribeButtons()
        {
            if (_reloadButton != null) _reloadButton.OnTriggered -= HandleReloadTriggered;
            if (_interactButton != null) _interactButton.OnTriggered -= HandleInteractTriggered;
            if (_switchWeaponButton != null) _switchWeaponButton.OnTriggered -= HandleWeaponSwitchTriggered;
            if (_inventoryButton != null) _inventoryButton.OnTriggered -= HandleInventoryTriggered;
            if (_pauseButton != null) _pauseButton.OnTriggered -= HandlePauseTriggered;
            if (_reviveButton != null) _reviveButton.OnTriggered -= HandleReviveTriggered;
        }

        private void HandleReloadTriggered(string id) => OnReloadAction?.Invoke();
        private void HandleInteractTriggered(string id) => OnInteractAction?.Invoke();
        private void HandleWeaponSwitchTriggered(string id) => OnWeaponSwitchAction?.Invoke();
        private void HandleInventoryTriggered(string id) => OnInventoryAction?.Invoke();
        private void HandlePauseTriggered(string id) => OnPauseAction?.Invoke();
        private void HandleReviveTriggered(string id) => OnReviveAction?.Invoke();

        private void Update()
        {
            UpdateInputState();
        }

        private void UpdateInputState()
        {
#if UNITY_ANDROID || UNITY_IOS
            // Mobile Touch Input Gathering
            _moveInput = _virtualJoystick != null ? _virtualJoystick.InputDirection : Vector2.zero;
            _lookDelta = _cameraLookArea != null ? _cameraLookArea.LookDelta : Vector2.zero;
            _isFiring = _fireButton != null && _fireButton.IsPressed;
            _isAiming = _aimButton != null && _aimButton.IsPressed;
            _isSprinting = _sprintButton != null && _sprintButton.IsPressed;
#else
            // Desktop / Editor Keyboard & Mouse Fallback with Touch Support
            Vector2 touchMove = _virtualJoystick != null ? _virtualJoystick.InputDirection : Vector2.zero;
            if (touchMove.sqrMagnitude > 0.01f)
            {
                _moveInput = touchMove;
            }
            else
            {
                float h = 0f;
                float v = 0f;
                if (Keyboard.current != null)
                {
                    if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) v += 1f;
                    if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) v -= 1f;
                    if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h += 1f;
                    if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h -= 1f;
                }
                _moveInput = new Vector2(h, v).normalized;
            }

            Vector2 touchLook = _cameraLookArea != null ? _cameraLookArea.LookDelta : Vector2.zero;
            if (touchLook.sqrMagnitude > 0.001f)
            {
                _lookDelta = touchLook;
            }
            else if (Mouse.current != null)
            {
                _lookDelta = Mouse.current.delta.ReadValue() * 0.15f;
            }
            else
            {
                _lookDelta = Vector2.zero;
            }

            bool mouseFire = Mouse.current != null && Mouse.current.leftButton.isPressed;
            bool buttonFire = _fireButton != null && _fireButton.IsPressed;
            _isFiring = mouseFire || buttonFire;

            bool mouseAim = Mouse.current != null && Mouse.current.rightButton.isPressed;
            bool buttonAim = _aimButton != null && _aimButton.IsPressed;
            _isAiming = mouseAim || buttonAim;

            bool shiftSprint = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
            bool buttonSprint = _sprintButton != null && _sprintButton.IsPressed;
            _isSprinting = shiftSprint || buttonSprint;

            // Keyboard shortcut checks
            if (Keyboard.current != null)
            {
                if (Keyboard.current.rKey.wasPressedThisFrame) OnReloadAction?.Invoke();
                if (Keyboard.current.eKey.wasPressedThisFrame) OnInteractAction?.Invoke();
                if (Keyboard.current.qKey.wasPressedThisFrame) OnWeaponSwitchAction?.Invoke();
                if (Keyboard.current.tabKey.wasPressedThisFrame) OnInventoryAction?.Invoke();
                if (Keyboard.current.escapeKey.wasPressedThisFrame) OnPauseAction?.Invoke();
                if (Keyboard.current.fKey.wasPressedThisFrame) OnReviveAction?.Invoke();
            }
#endif
        }

        private void ConfigurePlatformVisibility()
        {
            if (_mobileControlsCanvasGroup == null) return;

#if UNITY_ANDROID || UNITY_IOS
            _mobileControlsCanvasGroup.alpha = 1f;
            _mobileControlsCanvasGroup.interactable = true;
            _mobileControlsCanvasGroup.blocksRaycasts = true;
#else
            // In editor/desktop, keep visible for testing unless standalone build
            _mobileControlsCanvasGroup.alpha = Application.isEditor ? 1f : 0f;
            _mobileControlsCanvasGroup.interactable = Application.isEditor;
            _mobileControlsCanvasGroup.blocksRaycasts = Application.isEditor;
#endif
        }

        public void SetControlOpacity(float alpha)
        {
            if (_mobileControlsCanvasGroup != null)
            {
                _mobileControlsCanvasGroup.alpha = Mathf.Clamp01(alpha);
            }
        }
    }
}
