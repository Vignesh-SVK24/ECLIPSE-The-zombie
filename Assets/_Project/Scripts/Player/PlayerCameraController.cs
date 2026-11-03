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
    /// AAA Dual-mode camera controller for Ethan Cross (Batch A).
    /// Features smooth 360-degree orbit around the character, independent free-look exploration,
    /// dynamic over-the-shoulder aiming zoom, obstacle occlusion sphere-cast clipping prevention,
    /// and seamless first/third person toggling.
    /// Fully compatible with Desktop Mouse Look and Mobile Touch Swipe.
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
        [SerializeField] private float _aimDistance = 1.75f;
        [SerializeField] private Vector3 _pivotOffset = new Vector3(0, 1.60f, 0);
        [SerializeField] private Vector3 _shoulderOffset = new Vector3(0.55f, 0, 0);
        [SerializeField] private Vector3 _aimShoulderOffset = new Vector3(0.40f, -0.05f, 0);
        [SerializeField] private float _collisionRadius = 0.22f;
        [SerializeField] private LayerMask _collisionMask = ~0;

        [Header("Rotation & Smoothing")]
        [SerializeField] private float _mouseSensitivity = 2.2f;
        [SerializeField] private float _minPitch = -60f;
        [SerializeField] private float _maxPitch = 70f;
        [SerializeField] private float _cameraDamping = 18f;

        private float _yaw;
        private float _pitch;
        private Camera _camera;
        private bool _isAiming;
        private Vector3 _currentCameraPos;

        public CameraViewMode CurrentMode => _currentMode;
        public Camera Camera => _camera;
        public float Yaw => _yaw;
        public float Pitch => _pitch;
        public bool IsAiming => _isAiming;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            if (_camera == null) _camera = Camera.main;
            _currentCameraPos = transform.position;
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
                _currentCameraPos = transform.position;
            }
        }

        public void SetTarget(Transform target)
        {
            Initialize(target);
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

            // In First-Person mode, character body aligns immediately with camera yaw
            if (_currentMode == CameraViewMode.FirstPerson)
            {
                _target.rotation = Quaternion.Euler(0, _yaw, 0);
            }
        }

        private void UpdateCameraPosition()
        {
            Quaternion cameraRotation = Quaternion.Euler(_pitch, _yaw, 0);
            float dt = Time.deltaTime;

            if (_currentMode == CameraViewMode.FirstPerson)
            {
                // First Person View: Fixed to eye socket height
                Vector3 eyePos = _target.position + _target.rotation * _firstPersonOffset;
                transform.position = eyePos;
                transform.rotation = cameraRotation;
            }
            else
            {
                // Third Person Over-The-Shoulder View with Obstacle Occlusion Protection
                Vector3 currentShoulder = _isAiming ? _aimShoulderOffset : _shoulderOffset;
                float targetDist = _isAiming ? _aimDistance : _distance;

                // Pivot is anchored to target torso height and rotated by camera yaw for smooth orbiting
                Quaternion yawRotation = Quaternion.Euler(0, _yaw, 0);
                Vector3 shoulderWorldOffset = yawRotation * currentShoulder;
                Vector3 pivot = _target.position + _pivotOffset + shoulderWorldOffset;

                Vector3 desiredPos = pivot - (cameraRotation * Vector3.forward * targetDist);

                // SphereCast from pivot to desired position to avoid clipping walls/pillars
                Vector3 dir = (desiredPos - pivot).normalized;
                if (Physics.SphereCast(pivot, _collisionRadius, dir, out RaycastHit hit, targetDist, _collisionMask, QueryTriggerInteraction.Ignore))
                {
                    float adjustedDist = Mathf.Max(hit.distance - 0.1f, 0.4f);
                    desiredPos = pivot + dir * adjustedDist;
                }

                // Smooth camera position damping to eliminate jitter
                _currentCameraPos = Vector3.Lerp(_currentCameraPos, desiredPos, dt * _cameraDamping);
                transform.position = _currentCameraPos;
                transform.rotation = cameraRotation;
            }
        }
    }
}
