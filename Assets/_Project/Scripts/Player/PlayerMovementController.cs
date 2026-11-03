using System;
using UnityEngine;
using Eclipse.Character;
using Eclipse.Mobile;

namespace Eclipse.Player
{
    public enum MovementGait
    {
        Idle,
        Walking,
        Running,
        Sprinting,
        Crouching,
        AimStrafing
    }

    /// <summary>
    /// AAA Player Movement & 360-Degree Locomotion Controller for Ethan Cross (Batch A).
    /// Delivers fluid 360-degree directional traversal, camera-relative movement,
    /// smooth body rotation interpolation, normalized diagonal speeds, sprint acceleration,
    /// stamina regulation, and seamless transitions between Free Exploration and Combat Strafe modes.
    /// Fully optimized for PC Keyboard/Mouse and Mobile Virtual Joystick multi-touch.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement Speeds (m/s)")]
        [SerializeField] private float _walkSpeed = 3.6f;
        [SerializeField] private float _runSpeed = 5.4f;
        [SerializeField] private float _sprintSpeed = 7.5f;
        [SerializeField] private float _crouchSpeed = 2.0f;
        [SerializeField] private float _aimWalkSpeed = 2.8f;
        [SerializeField] private float _aimBackwardSpeed = 2.2f;

        [Header("Acceleration & Dynamics")]
        [SerializeField] private float _acceleration = 12f;
        [SerializeField] private float _deceleration = 16f;
        [SerializeField] private float _gravity = -18f;
        [SerializeField] private float _jumpHeight = 1.25f;

        [Header("360-Degree Rotation Settings")]
        [SerializeField] private float _freeRotationSpeed = 14f;
        [SerializeField] private float _aimRotationSpeed = 20f;
        [SerializeField] private float _turnBankingAmount = 2.5f; // Subtle body tilt into turns
        [SerializeField] private bool _orientToCameraWhenStationary = false;

        [Header("Stamina System")]
        [SerializeField] private float _maxStamina = 100f;
        [SerializeField] private float _currentStamina = 100f;
        [SerializeField] private float _staminaDrainRate = 22f;
        [SerializeField] private float _staminaRecoveryRate = 18f;

        [Header("Component References")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private CharacterAnimationController _animationController;
        [SerializeField] private PlayerCombatController _combatController;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _visualModelTransform;

        // Runtime Movement State
        private Vector3 _moveVelocity;
        private Vector3 _verticalVelocity;
        private MovementGait _currentGait = MovementGait.Idle;
        private bool _isGrounded;
        private bool _isSprinting;
        private bool _isCrouching;
        private bool _isAiming;
        private float _currentSpeed;
        private float _targetSpeed;
        private float _currentBankAngle;
        private float _originalControllerHeight;
        private Vector3 _originalControllerCenter;

        public MovementGait CurrentGait => _currentGait;
        public float CurrentSpeed => _currentSpeed;
        public float WalkSpeed => _walkSpeed;
        public float RunSpeed => _runSpeed;
        public float SprintSpeed => _sprintSpeed;
        public float CurrentStamina => _currentStamina;
        public float MaxStamina => _maxStamina;
        public bool IsSprinting => _isSprinting;
        public bool IsCrouching => _isCrouching;
        public bool IsGrounded => _isGrounded;
        public bool IsAiming => _isAiming;

        public event Action<float> OnStaminaChanged;
        public event Action<MovementGait> OnGaitChanged;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animationController = GetComponent<CharacterAnimationController>();
            _combatController = GetComponent<PlayerCombatController>();

            if (_characterController != null)
            {
                _originalControllerHeight = _characterController.height;
                _originalControllerCenter = _characterController.center;
            }

            if (_visualModelTransform == null)
            {
                // Find humanoid model child or default to transform
                var smr = GetComponentInChildren<SkinnedMeshRenderer>();
                _visualModelTransform = smr != null ? smr.transform.parent : transform;
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
            float dt = Time.deltaTime;

            UpdateCombatState();
            HandleGroundCheck();
            HandleMovementAndRotation(dt);
            HandleStamina(dt);
        }

        private void UpdateCombatState()
        {
            if (_combatController != null)
            {
                _isAiming = _combatController.IsAiming;
            }
        }

        private void HandleGroundCheck()
        {
            _isGrounded = _characterController.isGrounded;
            if (_isGrounded && _verticalVelocity.y < 0)
            {
                _verticalVelocity.y = -2.5f; // Downward bias for stairs and slopes
            }
        }

        private void HandleMovementAndRotation(float dt)
        {
            // 1. Gather raw inputs from Desktop (WASD) and Mobile Virtual Joystick
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputZ = Input.GetAxisRaw("Vertical");

            if (MobileInputManager.Instance != null && MobileInputManager.Instance.MovementInput.sqrMagnitude > 0.01f)
            {
                inputX = MobileInputManager.Instance.MovementInput.x;
                inputZ = MobileInputManager.Instance.MovementInput.y;
            }

            // 2. Normalized 360-Degree Input Vector (Eliminates faster diagonal movement)
            Vector3 rawInput = new Vector3(inputX, 0, inputZ);
            Vector3 normalizedInput = rawInput.sqrMagnitude > 1f ? rawInput.normalized : rawInput;
            float inputMagnitude = normalizedInput.magnitude;
            bool hasMovementInput = inputMagnitude > 0.05f;

            // 3. Stance & Gait Modifiers
            bool sprintHeld = Input.GetKey(KeyCode.LeftShift) || (MobileInputManager.Instance != null && MobileInputManager.Instance.IsSprinting);
            bool crouchHeld = Input.GetKey(KeyCode.C) || (MobileInputManager.Instance != null && MobileInputManager.Instance.IsCrouching);

            _isCrouching = crouchHeld;
            _isSprinting = sprintHeld && hasMovementInput && _currentStamina > 10f && !_isCrouching && !_isAiming;

            // Update Character Controller height for crouching
            UpdateCrouchDimensions(dt);

            // 4. Calculate Camera-Relative World Direction
            Vector3 worldMoveDirection = Vector3.zero;
            if (_cameraTransform != null && hasMovementInput)
            {
                Vector3 camForward = _cameraTransform.forward;
                Vector3 camRight = _cameraTransform.right;
                camForward.y = 0;
                camRight.y = 0;
                camForward.Normalize();
                camRight.Normalize();

                worldMoveDirection = (camForward * normalizedInput.z + camRight * normalizedInput.x).normalized;
            }

            // 5. Determine Target Speed & Gait
            if (!hasMovementInput)
            {
                _targetSpeed = 0f;
                _currentGait = MovementGait.Idle;
            }
            else if (_isAiming)
            {
                // Check if moving backward relative to aim direction
                float forwardDot = Vector3.Dot(worldMoveDirection, transform.forward);
                _targetSpeed = (forwardDot < -0.2f) ? _aimBackwardSpeed : _aimWalkSpeed;
                _currentGait = MovementGait.AimStrafing;
            }
            else if (_isCrouching)
            {
                _targetSpeed = _crouchSpeed;
                _currentGait = MovementGait.Crouching;
            }
            else if (_isSprinting)
            {
                _targetSpeed = _sprintSpeed;
                _currentGait = MovementGait.Sprinting;
            }
            else if (inputMagnitude > 0.75f)
            {
                _targetSpeed = _runSpeed;
                _currentGait = MovementGait.Running;
            }
            else
            {
                _targetSpeed = _walkSpeed;
                _currentGait = MovementGait.Walking;
            }

            // Smooth Acceleration / Deceleration
            float accelRate = hasMovementInput ? _acceleration : _deceleration;
            _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, dt * accelRate);

            Vector3 targetVelocity = worldMoveDirection * _currentSpeed;
            _moveVelocity = Vector3.MoveTowards(_moveVelocity, targetVelocity, dt * accelRate * _sprintSpeed);

            // 6. Smooth 360-Degree Body Rotation
            HandleSmoothBodyRotation(worldMoveDirection, hasMovementInput, dt);

            // 7. Jump & Vertical Gravity
            if (Input.GetButtonDown("Jump") && _isGrounded && !_isCrouching)
            {
                _verticalVelocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }
            _verticalVelocity.y += _gravity * dt;

            // 8. Execute Movement
            Vector3 finalDisplacement = (_moveVelocity + _verticalVelocity) * dt;
            _characterController.Move(finalDisplacement);

            // 9. Update Animation Controller (Forward/Strafe blend values)
            UpdateLocomotionAnimation();
        }

        private void HandleSmoothBodyRotation(Vector3 worldDirection, bool hasMovementInput, float dt)
        {
            if (_isAiming)
            {
                // Combat Strafe Mode: Body smoothly aligns with camera yaw
                if (_cameraTransform != null)
                {
                    Vector3 camFwd = _cameraTransform.forward;
                    camFwd.y = 0;
                    if (camFwd.sqrMagnitude > 0.001f)
                    {
                        Quaternion targetRot = Quaternion.LookRotation(camFwd);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, dt * _aimRotationSpeed);
                    }
                }
            }
            else if (hasMovementInput && worldDirection.sqrMagnitude > 0.001f)
            {
                // Free Exploration Mode: Body smoothly rotates 360 degrees to face movement direction
                Quaternion targetRot = Quaternion.LookRotation(worldDirection);
                float angleDiff = Quaternion.Angle(transform.rotation, targetRot);

                // Smooth Slerp with dynamic speed based on turn angle
                float effectiveTurnSpeed = _freeRotationSpeed * (1.0f + (angleDiff / 180f) * 0.5f);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, dt * effectiveTurnSpeed);

                // Calculate Turn Banking (subtle body roll into turn)
                float turnAngle = Vector3.SignedAngle(transform.forward, worldDirection, Vector3.up);
                float targetBank = Mathf.Clamp(-turnAngle * 0.15f, -_turnBankingAmount, _turnBankingAmount);
                _currentBankAngle = Mathf.Lerp(_currentBankAngle, targetBank, dt * 8f);
            }
            else
            {
                _currentBankAngle = Mathf.Lerp(_currentBankAngle, 0f, dt * 10f);
            }
        }

        private void UpdateCrouchDimensions(float dt)
        {
            if (_characterController == null) return;

            float targetHeight = _isCrouching ? _originalControllerHeight * 0.65f : _originalControllerHeight;
            Vector3 targetCenter = _isCrouching ? new Vector3(0, targetHeight * 0.5f, 0) : _originalControllerCenter;

            _characterController.height = Mathf.Lerp(_characterController.height, targetHeight, dt * 10f);
            _characterController.center = Vector3.Lerp(_characterController.center, targetCenter, dt * 10f);
        }

        private void UpdateLocomotionAnimation()
        {
            if (_animationController == null) return;

            // Project velocity onto character local coordinates for directional strafe animations
            float localForward = Vector3.Dot(_moveVelocity, transform.forward) / _sprintSpeed;
            float localStrafe = Vector3.Dot(_moveVelocity, transform.right) / _sprintSpeed;

            _animationController.SetLocomotion(localForward, localStrafe, _isSprinting, _isCrouching, _currentBankAngle);
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
