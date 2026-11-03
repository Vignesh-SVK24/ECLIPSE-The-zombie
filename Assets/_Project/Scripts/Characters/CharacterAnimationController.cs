using System;
using UnityEngine;

namespace Eclipse.Character
{
    /// <summary>
    /// Controls humanoid character animation states, procedural breathing,
    /// weapon aiming bone offsets, recoil recovery, and hit flinch reactions.
    /// Interoperable with Unity Animator and procedural bone transforms.
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
        [SerializeField] private bool _isSprinting;
        [SerializeField] private bool _isCrouching;
        [SerializeField] private bool _isAiming;

        [Header("Procedural Motion")]
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

        public float ForwardSpeed => _forwardSpeed;
        public float StrafeSpeed => _strafeSpeed;
        public bool IsAiming => _isAiming;
        public bool IsSprinting => _isSprinting;

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

        public void SetLocomotion(float forward, float strafe, bool isSprinting, bool isCrouching)
        {
            _forwardSpeed = Mathf.Lerp(_forwardSpeed, forward, Time.deltaTime * 10f);
            _strafeSpeed = Mathf.Lerp(_strafeSpeed, strafe, Time.deltaTime * 10f);
            _isSprinting = isSprinting;
            _isCrouching = isCrouching;

            if (_animator != null)
            {
                _animator.SetFloat("Forward", _forwardSpeed);
                _animator.SetFloat("Strafe", _strafeSpeed);
                _animator.SetBool("IsSprinting", _isSprinting);
                _animator.SetBool("IsCrouching", _isCrouching);
            }
        }

        public void SetAim(bool isAiming, float pitchAngle, float yawAngle)
        {
            _isAiming = isAiming;
            _aimPitchAngle = Mathf.Clamp(pitchAngle, -60f, 60f);
            _aimYawAngle = Mathf.Clamp(yawAngle, -70f, 70f);

            if (_animator != null)
            {
                _animator.SetBool("IsAiming", _isAiming);
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

            // 1. Procedural Breathing
            if (_enableProceduralBreathing && _chestBone != null)
            {
                float sprintMultiplier = _isSprinting ? 2.2f : 1.0f;
                _breathingTimer += dt * _breathingFrequency * sprintMultiplier;
                float breathCycle = Mathf.Sin(_breathingTimer);

                Vector3 breathOffset = new Vector3(0, breathCycle * _breathingAmplitude, breathCycle * _breathingAmplitude * 0.5f);
                _chestBone.localPosition = _originalChestPos + breathOffset + _flinchOffset;
            }

            // 2. Procedural Spine / Chest Aiming & Recoil
            if (_chestBone != null)
            {
                float totalPitch = (_isAiming ? _aimPitchAngle * 0.5f : 0f) - _currentRecoilPitch;
                Quaternion aimRot = Quaternion.Euler(totalPitch, _aimYawAngle * 0.4f, 0);
                _chestBone.localRotation = _originalChestRot * aimRot;
            }

            if (_headBone != null && _isAiming)
            {
                // Head tracks look target
                Quaternion headAimRot = Quaternion.Euler(_aimPitchAngle * 0.5f, _aimYawAngle * 0.6f, 0);
                _headBone.localRotation = headAimRot;
            }
        }
    }
}
