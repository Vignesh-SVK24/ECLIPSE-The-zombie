using System;
using UnityEngine;
using UnityEngine.AI;

namespace Eclipse.Environment
{
    /// <summary>
    /// Implements the Player-Only Interactive Door System for ECLIPSE: THE LAST CIVILIZATION.
    /// Strictly restricts door interaction to the player character only.
    /// Zombies are fundamentally prevented from opening, closing, or clipping through closed doors.
    /// Dynamically manages physical collision, bullet blocking, and NavMeshObstacle carving.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class InteractiveDoorController : MonoBehaviour
    {
        [Header("Door Visual Hinge & Pivot")]
        [SerializeField] private Transform _doorHinge;
        [SerializeField] private float _openAngle = 90f;
        [SerializeField] private float _animationSpeed = 4.5f;

        [Header("Door State")]
        [SerializeField] private bool _isOpen = false;
        [SerializeField] private bool _isLocked = false;
        [SerializeField] private string _requiredKeyId = "";

        [Header("Physics & Navigation")]
        [SerializeField] private BoxCollider _closedMovementCollider;
        [SerializeField] private NavMeshObstacle _navObstacle;
        [SerializeField] private LayerMask _bulletLayer;

        [Header("Interaction Configuration")]
        [SerializeField] private float _interactionDistance = 2.8f;
        [SerializeField] private string _doorName = "Security Quarantine Gate";

        // Sound Effects
        [Header("Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _openSound;
        [SerializeField] private AudioClip _closeSound;
        [SerializeField] private AudioClip _lockedSound;

        // Runtime State
        private Quaternion _closedRotation;
        private Quaternion _targetRotation;
        private bool _isAnimating = false;

        public bool IsOpen => _isOpen;
        public bool IsLocked => _isLocked;
        public string DoorName => _doorName;

        public event Action<bool> OnDoorStateChanged;

        private void Awake()
        {
            if (_doorHinge == null)
            {
                _doorHinge = transform;
            }
            _closedRotation = _doorHinge.localRotation;
            _targetRotation = _isOpen 
                ? _closedRotation * Quaternion.Euler(0, _openAngle, 0) 
                : _closedRotation;

            if (_closedMovementCollider == null)
            {
                _closedMovementCollider = GetComponent<BoxCollider>();
            }

            SetupNavMeshObstacle();
            UpdatePhysicsAndNavigationState();
        }

        private void SetupNavMeshObstacle()
        {
            _navObstacle = GetComponent<NavMeshObstacle>();
            if (_navObstacle == null)
            {
                _navObstacle = gameObject.AddComponent<NavMeshObstacle>();
            }

            if (_closedMovementCollider != null)
            {
                _navObstacle.shape = NavMeshObstacleShape.Box;
                _navObstacle.size = _closedMovementCollider.size;
                _navObstacle.center = _closedMovementCollider.center;
            }
            _navObstacle.carving = true;
            _navObstacle.carveOnlyStationary = true;
        }

        private void Update()
        {
            if (_isAnimating && _doorHinge != null)
            {
                _doorHinge.localRotation = Quaternion.Slerp(
                    _doorHinge.localRotation, 
                    _targetRotation, 
                    Time.deltaTime * _animationSpeed
                );

                if (Quaternion.Angle(_doorHinge.localRotation, _targetRotation) < 0.5f)
                {
                    _doorHinge.localRotation = _targetRotation;
                    _isAnimating = false;
                }
            }
        }

        /// <summary>
        /// Strict Player-Only Interaction Entry Point.
        /// Validates that the interacting entity is genuinely the player.
        /// Rejects all zombie entities unconditionally.
        /// </summary>
        /// <param name="initiator">The GameObject attempting to interact.</param>
        /// <returns>True if interaction was authorized and processed; false otherwise.</returns>
        public bool TryInteract(GameObject initiator)
        {
            if (initiator == null) return false;

            // STRICT ENTITY SECURITY CHECK:
            // 1. Must have "Player" tag
            // 2. Must NOT have "Zombie" tag or zombie components
            bool isPlayer = initiator.CompareTag("Player") || initiator.GetComponent<Player.PlayerMovementController>() != null || initiator.GetComponentInParent<Player.PlayerMovementController>() != null;
            bool isZombie = initiator.CompareTag("Zombie") || initiator.name.IndexOf("zombie", StringComparison.OrdinalIgnoreCase) >= 0;

            if (!isPlayer || isZombie)
            {
                // ZOMBIES MUST NEVER OPEN DOORS
                return false;
            }

            // Check if player is within valid interaction radius
            float distance = Vector3.Distance(transform.position, initiator.transform.position);
            if (distance > _interactionDistance)
            {
                return false;
            }

            // If door is locked, check player inventory for key
            if (_isLocked)
            {
                PlaySound(_lockedSound);
                return false;
            }

            // Toggle door state
            ToggleDoor();
            return true;
        }

        /// <summary>
        /// Toggles door between Open and Closed states.
        /// Updates animation target, physical colliders, and NavMesh carving.
        /// </summary>
        public void ToggleDoor()
        {
            _isOpen = !_isOpen;
            _isAnimating = true;

            _targetRotation = _isOpen
                ? _closedRotation * Quaternion.Euler(0, _openAngle, 0)
                : _closedRotation;

            UpdatePhysicsAndNavigationState();
            PlaySound(_isOpen ? _openSound : _closeSound);

            OnDoorStateChanged?.Invoke(_isOpen);
        }

        private void UpdatePhysicsAndNavigationState()
        {
            if (_isOpen)
            {
                // Door is OPEN:
                // Move or disable doorway blocking collider so player and zombies can enter doorway
                if (_closedMovementCollider != null)
                {
                    _closedMovementCollider.enabled = false;
                }
                if (_navObstacle != null)
                {
                    // Carving disabled or resized so AI can pathfind through open doorway
                    _navObstacle.carving = false;
                    _navObstacle.enabled = false;
                }
            }
            else
            {
                // Door is CLOSED:
                // Enable solid collider to block player movement, zombie movement, and bullets
                if (_closedMovementCollider != null)
                {
                    _closedMovementCollider.enabled = true;
                }
                if (_navObstacle != null)
                {
                    _navObstacle.enabled = true;
                    _navObstacle.carving = true;
                }
            }
        }

        public string GetInteractionPrompt()
        {
            if (_isLocked)
            {
                return $"[LOCKED] {_doorName} requires key: {_requiredKeyId}";
            }
            return _isOpen ? $"[E] Close {_doorName}" : $"[E] Open {_doorName}";
        }

        private void PlaySound(AudioClip clip)
        {
            if (_audioSource != null && clip != null)
            {
                _audioSource.PlayOneShot(clip);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _isOpen ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, _interactionDistance);
        }
    }
}
