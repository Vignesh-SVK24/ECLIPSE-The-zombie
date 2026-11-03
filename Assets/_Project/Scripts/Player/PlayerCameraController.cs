using System;
using UnityEngine;
using Eclipse.Mobile;

namespace Eclipse.Player
{
    public enum CameraViewMode
    {
        ThirdPerson,
        FirstPerson
    }

    /// <summary>
    /// Dual-mode camera controller supporting seamless toggling between First-Person
    /// and Third-Person Over-The-Shoulder perspectives with obstacle occlusion clipping prevention.
    /// Compatible with Desktop Mouse Look and Mobile Touch Swipe.
    /// </summary>
    public class PlayerCameraController : MonoBehaviour
    {
        [Header("Target & View Mode")]
        [SerializeField] private Transform _target;
        [SerializeField] private CameraViewMode _currentMode = CameraViewMode.ThirdPerson;

        [Header("First Person Configuration")]
        [SerializeField] private Vector3 _firstPersonOffset = new Vector3(0, 1.72f, 0.08f);

        [Header("Third Person Configuration")]
        [SerializeField] private float _distance = 3.2f;
        [SerializeField] private float _aimDistance = 1.8f;
        [SerializeField] private Vector3 _shoulderOffset = new Vector3(0.55f, 1.65f, 0);
        [SerializeField] private Vector3 _aimShoulderOffset = new Vector3(0.42f, 1.55f, 0);
        [SerializeField] private float _collisionRadius = 0.25f;
        [SerializeField] private LayerMask _collisionMask = ~0;

        [Header("Rotation Settings")]
        [SerializeField] private float _mouseSensitivity = 2.2f;
        [SerializeField] private float _minPitch = -60f;
        [SerializeField] private float _maxPitch = 70f;
        [SerializeField] private bool _smoothRotation = true;
        [SerializeField] private float _rotationDamping = 15f;

        private float _yaw;
        private float _pitch;
        private Camera _camera;
        private bool _isAiming;

        public CameraViewMode CurrentMode => _currentMode;
        public Camera Camera => _camera;
        public float Yaw => _yaw;
        public float Pitch => _pitch;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            if (_camera == null) _camera = Camera.main;
        }

        private void Start()
        {
            if (_target != null)
            {
                _yaw = _target.eulerAngles.y;
            }
        }

        public void Initialize(Transform target)
        {
            _target = target;
            if (_target != null)
            {
                _yaw = _target.eulerAngles.y;
            }
        }

        public void SetAim(bool isAiming)
        {
            _isAiming = isAiming;
        }

        public void ToggleViewMode()
        {
            _currentMode = (_currentMode == CameraViewMode.ThirdPerson) ? CameraViewMode.FirstPerson : CameraViewMode.ThirdPerson;
        }

        private void LateUpdate()
        {
            if (_target == null) return;

            HandleRotationInput();
            UpdateCameraPosition();
        }

        private void HandleRotationInput()
        {
            float lookX = Input.GetAxis("Mouse X") * _mouseSensitivity;
            float lookY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

            // Mobile touch swipe delta input
            if (MobileInputManager.Instance != null && MobileInputManager.Instance.LookDelta.sqrMagnitude > 0.001f)
            {
                lookX = MobileInputManager.Instance.LookDelta.x;
                lookY = MobileInputManager.Instance.LookDelta.y;
            }

            _yaw += lookX;
            _pitch = Mathf.Clamp(_pitch - lookY, _minPitch, _maxPitch);

            // Orient character towards camera yaw
            _target.rotation = Quaternion.Euler(0, _yaw, 0);
        }

        private void UpdateCameraPosition()
        {
            Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);

            if (_currentMode == CameraViewMode.FirstPerson)
            {
                // First Person: Fixed to eye socket height
                Vector3 eyePos = _target.position + _target.rotation * _firstPersonOffset;
                transform.position = eyePos;
                transform.rotation = rotation;
            }
            else
            {
                // Third Person Over-The-Shoulder with Obstacle Clipping Protection
                Vector3 currentShoulder = _isAiming ? _aimShoulderOffset : _shoulderOffset;
                float targetDist = _isAiming ? _aimDistance : _distance;

                Vector3 pivot = _target.position + _target.rotation * currentShoulder;
                Vector3 desiredPos = pivot - (rotation * Vector3.forward * targetDist);

                // SphereCast from pivot to desired position to prevent clipping through terrain/walls
                Vector3 dir = (desiredPos - pivot).normalized;
                if (Physics.SphereCast(pivot, _collisionRadius, dir, out RaycastHit hit, targetDist, _collisionMask, QueryTriggerInteraction.Ignore))
                {
                    float adjustedDist = Mathf.Max(hit.distance - 0.1f, 0.4f);
                    transform.position = pivot + dir * adjustedDist;
                }
                else
                {
                    transform.position = desiredPos;
                }

                transform.rotation = rotation;
            }
        }
    }
}
