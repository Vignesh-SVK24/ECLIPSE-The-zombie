using System;
using UnityEngine;
using Eclipse.Character;
using Eclipse.Mobile;

namespace Eclipse.Player
{
    /// <summary>
    /// Production-ready movement controller for the protagonist Ethan Cross.
    /// Supports dual input methods (Keyboard/Mouse and Mobile Virtual Joystick).
    /// Regulates stamina, sprint boosts, crouch height transitions, and character rotation.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement Speeds")]
        [SerializeField] private float _walkSpeed = 3.8f;
        [SerializeField] private float _sprintSpeed = 7.2f;
        [SerializeField] private float _crouchSpeed = 2.0f;
        [SerializeField] private float _acceleration = 12f;
        [SerializeField] private float _gravity = -18f;
        [SerializeField] private float _jumpHeight = 1.2f;

        [Header("Stamina System")]
        [SerializeField] private float _maxStamina = 100f;
        [SerializeField] private float _currentStamina = 100f;
        [SerializeField] private float _staminaDrainRate = 22f;
        [SerializeField] private float _staminaRecoveryRate = 16f;

        [Header("References")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private CharacterAnimationController _animationController;
        [SerializeField] private Transform _cameraTransform;

        private Vector3 _velocity;
        private Vector3 _currentMovementVelocity;
        private bool _isGrounded;
        private bool _isSprinting;
        private bool _isCrouching;
        private float _originalControllerHeight;
        private Vector3 _originalControllerCenter;

        public float WalkSpeed => _walkSpeed;
        public float SprintSpeed => _sprintSpeed;
        public float CurrentStamina => _currentStamina;
        public float MaxStamina => _maxStamina;
        public bool IsSprinting => _isSprinting;
        public bool IsCrouching => _isCrouching;
        public bool IsGrounded => _isGrounded;

        public event Action<float> OnStaminaChanged;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animationController = GetComponent<CharacterAnimationController>();

            if (_characterController != null)
            {
                _originalControllerHeight = _characterController.height;
                _originalControllerCenter = _characterController.center;
            }
        }

        private void Start()
        {
            if (_cameraTransform == null && Camera.main != null)
            {
                _cameraTransform = Camera.main.transform;
            }
        }

        public void Initialize(CharacterAnimationController animController, Transform camTransform)
        {
            _animationController = animController;
            _cameraTransform = camTransform;
        }

        private void Update()
        {
            HandleGroundCheck();
            HandleMovementInput();
            HandleStamina(Time.deltaTime);
        }

        private void HandleGroundCheck()
        {
            _isGrounded = _characterController.isGrounded;
            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f; // Slight downward force to keep grounded on slopes
            }
        }

        private void HandleMovementInput()
        {
            // 1. Collect Input from Keyboard or Virtual Joystick
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            // Mobile input fallback if joystick is active
            if (MobileInputManager.Instance != null && MobileInputManager.Instance.MovementInput.sqrMagnitude > 0.01f)
            {
                moveX = MobileInputManager.Instance.MovementInput.x;
                moveZ = MobileInputManager.Instance.MovementInput.y;
            }

            Vector3 inputDirection = new Vector3(moveX, 0, moveZ).normalized;

            // 2. Sprint & Crouch Toggles
            bool sprintKey = Input.GetKey(KeyCode.LeftShift);
            if (MobileInputManager.Instance != null && MobileInputManager.Instance.IsSprinting)
            {
                sprintKey = true;
            }

            _isSprinting = sprintKey && inputDirection.sqrMagnitude > 0.1f && _currentStamina > 8f && !_isCrouching;
            _isCrouching = Input.GetKey(KeyCode.C) || (MobileInputManager.Instance != null && MobileInputManager.Instance.IsCrouching);

            // Adjust character controller height when crouching
            if (_characterController != null)
            {
                float targetHeight = _isCrouching ? _originalControllerHeight * 0.65f : _originalControllerHeight;
                Vector3 targetCenter = _isCrouching ? new Vector3(0, targetHeight * 0.5f, 0) : _originalControllerCenter;

                _characterController.height = Mathf.Lerp(_characterController.height, targetHeight, Time.deltaTime * 10f);
                _characterController.center = Vector3.Lerp(_characterController.center, targetCenter, Time.deltaTime * 10f);
            }

            // 3. Speed Calculation
            float targetSpeed = _walkSpeed;
            if (_isSprinting) targetSpeed = _sprintSpeed;
            else if (_isCrouching) targetSpeed = _crouchSpeed;

            // 4. Direction Relative to Camera
            Vector3 worldDirection = Vector3.zero;
            if (_cameraTransform != null && inputDirection.sqrMagnitude > 0.001f)
            {
                Vector3 camForward = _cameraTransform.forward;
                Vector3 camRight = _cameraTransform.right;
                camForward.y = 0;
                camRight.y = 0;
                camForward.Normalize();
                camRight.Normalize();

                worldDirection = (camForward * inputDirection.z + camRight * inputDirection.x).normalized;
            }

            Vector3 targetVelocity = worldDirection * targetSpeed;
            _currentMovementVelocity = Vector3.Lerp(_currentMovementVelocity, targetVelocity, Time.deltaTime * _acceleration);

            // 5. Jump
            if (Input.GetButtonDown("Jump") && _isGrounded && !_isCrouching)
            {
                _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }

            // 6. Gravity
            _velocity.y += _gravity * Time.deltaTime;

            // 7. Apply Movement
            Vector3 finalMove = (_currentMovementVelocity + _velocity) * Time.deltaTime;
            _characterController.Move(finalMove);

            // 8. Update Animation Controller Blend values
            if (_animationController != null)
            {
                float forward = Vector3.Dot(_currentMovementVelocity, transform.forward) / _sprintSpeed;
                float strafe = Vector3.Dot(_currentMovementVelocity, transform.right) / _sprintSpeed;
                _animationController.SetLocomotion(forward, strafe, _isSprinting, _isCrouching);
            }
        }

        private void HandleStamina(float dt)
        {
            if (_isSprinting)
            {
                _currentStamina = Mathf.Max(0, _currentStamina - dt * _staminaDrainRate);
            }
            else
            {
                _currentStamina = Mathf.Min(_maxStamina, _currentStamina + dt * _staminaRecoveryRate);
            }

            OnStaminaChanged?.Invoke(_currentStamina / _maxStamina);
        }
    }
}
