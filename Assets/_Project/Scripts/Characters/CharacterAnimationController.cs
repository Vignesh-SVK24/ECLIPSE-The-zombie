using System;
using UnityEngine;

namespace Eclipse.Character
{
    /// <summary>
    /// AAA Humanoid Character Animation & Procedural Bone Dynamics Controller (Batch A).
    /// Manages 360-degree locomotion blend parameters (Forward, Strafe, Speed, BankAngle),
    /// procedural breathing cycles, spine turn-banking, weapon aiming bone tracking,
    /// animated recoil recovery, and hit flinch reactions.
    /// </summary>
    public class CharacterAnimationController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _headBone;
        [SerializeField] private Transform _spineBone;
        [SerializeField] private Transform _chestBone;
        [SerializeField] private Transform _rightArmBone;
        [SerializeField] private Transform _leftArmBone;

        [Header("Locomotion Parameters")]
        [SerializeField] private float _forwardSpeed;
        [SerializeField] private float _strafeSpeed;
        [SerializeField] private float _movementSpeedMagnitude;
        [SerializeField] private float _turnBankAngle;
        [SerializeField] private bool _isMoving;
        [SerializeField] private bool _isSprinting;
        [SerializeField] private bool _isCrouching;
        [SerializeField] private bool _isAiming;

        [Header("Procedural Motion & Turn Banking")]
        [SerializeField] private bool _enableProceduralBreathing = true;
        [SerializeField] private float _breathingFrequency = 1.2f;
        [SerializeField] private float _breathingAmplitude = 0.008f;
        [SerializeField] private float _aimPitchAngle;
        [SerializeField] private float _aimYawAngle;

        private float _breathingTimer;
        private Vector3 _originalChestPos;
        private Quaternion _originalChestRot;
        private Quaternion _originalSpineRot;
        private float _currentRecoilPitch;
        private float _flinchTimer;
        private Vector3 _flinchOffset;
        private float _footstepTimer;

        // Animator Parameter Hash IDs for high-performance updates
        private static readonly int ParamForward = Animator.StringToHash("Forward");
        private static readonly int ParamStrafe = Animator.StringToHash("Strafe");
        private static readonly int ParamSpeed = Animator.StringToHash("Speed");
        private static readonly int ParamIsMoving = Animator.StringToHash("IsMoving");
        private static readonly int ParamIsSprinting = Animator.StringToHash("IsSprinting");
        private static readonly int ParamIsCrouching = Animator.StringToHash("IsCrouching");
        private static readonly int ParamIsAiming = Animator.StringToHash("IsAiming");
        private static readonly int ParamBankAngle = Animator.StringToHash("BankAngle");

        public float ForwardSpeed => _forwardSpeed;
        public float StrafeSpeed => _strafeSpeed;
        public float Speed => _movementSpeedMagnitude;
        public bool IsMoving => _isMoving;
        public bool IsAiming => _isAiming;
        public bool IsSprinting => _isSprinting;

        public event Action<string> OnFootstep;

        private void Awake()
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>() ?? GetComponentInChildren<Animator>();
            }
        }

        public void Initialize(Transform chest, Transform spine, Transform head, Transform rightArm, Transform leftArm, Animator animator = null)
        {
            _chestBone = chest;
            _spineBone = spine;
            _headBone = head;
            _rightArmBone = rightArm;
            _leftArmBone = leftArm;
            _animator = animator;

            if (_chestBone != null)
            {
                _originalChestPos = _chestBone.localPosition;
                _originalChestRot = _chestBone.localRotation;
            }
            if (_spineBone != null)
            {
                _originalSpineRot = _spineBone.localRotation;
            }
        }

        public void SetLocomotion(float forward, float strafe, bool isSprinting, bool isCrouching, float bankAngle = 0f)
        {
            float dt = Time.deltaTime;
            _forwardSpeed = Mathf.Lerp(_forwardSpeed, forward, dt * 12f);
            _strafeSpeed = Mathf.Lerp(_strafeSpeed, strafe, dt * 12f);
            _turnBankAngle = Mathf.Lerp(_turnBankAngle, bankAngle, dt * 8f);

            _movementSpeedMagnitude = Mathf.Sqrt(_forwardSpeed * _forwardSpeed + _strafeSpeed * _strafeSpeed);
            _isMoving = _movementSpeedMagnitude > 0.05f;
            _isSprinting = isSprinting;
            _isCrouching = isCrouching;

            // Trigger Footstep events based on gait frequency
            if (_isMoving)
            {
                float stepFreq = _isSprinting ? 0.30f : (_isCrouching ? 0.55f : 0.42f);
                _footstepTimer += dt;
                if (_footstepTimer >= stepFreq)
                {
                    _footstepTimer = 0f;
                    OnFootstep?.Invoke(_isSprinting ? "Footstep_Sprint" : "Footstep_Walk");
                }
            }
            else
            {
                _footstepTimer = 0f;
            }

            if (_animator != null)
            {
                _animator.SetFloat(ParamForward, _forwardSpeed);
                _animator.SetFloat(ParamStrafe, _strafeSpeed);
                _animator.SetFloat(ParamSpeed, _movementSpeedMagnitude);
                _animator.SetBool(ParamIsMoving, _isMoving);
                _animator.SetBool(ParamIsSprinting, _isSprinting);
                _animator.SetBool(ParamIsCrouching, _isCrouching);
                _animator.SetFloat(ParamBankAngle, _turnBankAngle);
            }
        }

        public void SetAim(bool isAiming, float pitchAngle, float yawAngle)
        {
            _isAiming = isAiming;
            _aimPitchAngle = Mathf.Clamp(pitchAngle, -60f, 60f);
            _aimYawAngle = Mathf.Clamp(yawAngle, -70f, 70f);

            if (_animator != null)
            {
                _animator.SetBool(ParamIsAiming, _isAiming);
            }
        }

        public void TriggerWeaponRecoil(float recoilStrength = 3.5f)
        {
            _currentRecoilPitch = Mathf.Min(_currentRecoilPitch + recoilStrength, 15f);
        }

        public void TriggerFlinch(Vector3 hitDirection)
        {
            _flinchTimer = 0.25f;
            _flinchOffset = -hitDirection.normalized * 0.06f;
        }

        private void LateUpdate()
        {
            UpdateProceduralMotion();
        }

        private void UpdateProceduralMotion()
        {
            float dt = Time.deltaTime;

            // Recoil decay
            if (_currentRecoilPitch > 0)
            {
                _currentRecoilPitch = Mathf.MoveTowards(_currentRecoilPitch, 0, dt * 18f);
            }

            // Flinch decay
            if (_flinchTimer > 0)
            {
                _flinchTimer -= dt;
                _flinchOffset = Vector3.Lerp(_flinchOffset, Vector3.zero, dt * 8f);
            }

            // 1. Procedural Breathing (Expands chest based on fatigue)
            if (_enableProceduralBreathing && _chestBone != null)
            {
                float sprintMultiplier = _isSprinting ? 2.2f : (_isMoving ? 1.4f : 1.0f);
                _breathingTimer += dt * _breathingFrequency * sprintMultiplier;
                float breathCycle = Mathf.Sin(_breathingTimer);

                Vector3 breathOffset = new Vector3(0, breathCycle * _breathingAmplitude, breathCycle * _breathingAmplitude * 0.5f);
                _chestBone.localPosition = _originalChestPos + breathOffset + _flinchOffset;
            }

            // 2. Procedural Spine Turn Banking & Weapon Aiming
            if (_spineBone != null)
            {
                Quaternion bankRot = Quaternion.Euler(0, 0, _turnBankAngle);
                _spineBone.localRotation = _originalSpineRot * bankRot;
            }

            if (_chestBone != null)
            {
                float totalPitch = (_isAiming ? _aimPitchAngle * 0.5f : 0f) - _currentRecoilPitch;
                Quaternion aimRot = Quaternion.Euler(totalPitch, _aimYawAngle * 0.4f, 0);
                _chestBone.localRotation = _originalChestRot * aimRot;
            }

            if (_headBone != null && _isAiming)
            {
                // Head tracks look target independently
                Quaternion headAimRot = Quaternion.Euler(_aimPitchAngle * 0.5f, _aimYawAngle * 0.6f, 0);
                _headBone.localRotation = headAimRot;
            }
        }
    }
}
