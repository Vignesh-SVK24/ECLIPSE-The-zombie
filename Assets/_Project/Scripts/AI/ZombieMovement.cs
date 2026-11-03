using UnityEngine;
using UnityEngine.AI;

namespace Eclipse.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class ZombieMovement : MonoBehaviour
    {
        [Header("Agent Configuration")]
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _stuckCheckInterval = 1.0f;
        [SerializeField] private float _stuckDistanceThreshold = 0.1f;

        private Vector3 _lastPosition;
        private float _stuckTimer;
        private bool _isStuck;

        public NavMeshAgent Agent => _agent;
        public float CurrentSpeed => _agent != null && _agent.enabled ? _agent.velocity.magnitude : 0f;
        public bool IsMoving => CurrentSpeed > 0.1f;
        public bool IsStuck => _isStuck;

        private void Awake()
        {
            if (_agent == null)
            {
                _agent = GetComponent<NavMeshAgent>();
            }

            // Configure sane mobile defaults for NavMeshAgent
            _agent.updateRotation = false; // Custom rotation for smoother turning
            _agent.autoBraking = true;
            _lastPosition = transform.position;
        }

        private void Update()
        {
            if (_agent == null || !_agent.enabled || !_agent.isOnNavMesh) return;

            // Handle smooth directional rotation towards movement vector
            if (_agent.velocity.sqrMagnitude > 0.05f)
            {
                Vector3 moveDir = _agent.velocity.normalized;
                moveDir.y = 0f;
                if (moveDir != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.LookRotation(moveDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * _rotationSpeed);
                }
            }

            UpdateStuckDetection();
        }

        public bool SetDestination(Vector3 targetPosition)
        {
            if (_agent == null || !_agent.enabled || !_agent.isOnNavMesh) return false;

            if (NavMesh.SamplePosition(targetPosition, out var hit, 4.0f, NavMesh.AllAreas))
            {
                return _agent.SetDestination(hit.position);
            }

            return _agent.SetDestination(targetPosition);
        }

        public void Stop()
        {
            if (_agent != null && _agent.enabled && _agent.isOnNavMesh)
            {
                _agent.isStopped = true;
                _agent.velocity = Vector3.zero;
            }
        }

        public void Resume()
        {
            if (_agent != null && _agent.enabled && _agent.isOnNavMesh)
            {
                _agent.isStopped = false;
            }
        }

        public void SetSpeed(float speed)
        {
            if (_agent != null)
            {
                _agent.speed = speed;
            }
        }

        public void SetStoppingDistance(float distance)
        {
            if (_agent != null)
            {
                _agent.stoppingDistance = distance;
            }
        }

        public bool HasReachedDestination()
        {
            if (_agent == null || !_agent.enabled || !_agent.isOnNavMesh) return true;

            if (!_agent.pathPending)
            {
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                {
                    if (!_agent.hasPath || _agent.velocity.sqrMagnitude < 0.01f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void RotateTowards(Vector3 targetWorldPosition)
        {
            Vector3 direction = (targetWorldPosition - transform.position);
            direction.y = 0f;
            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            }
        }

        private void UpdateStuckDetection()
        {
            _stuckTimer += Time.deltaTime;
            if (_stuckTimer >= _stuckCheckInterval)
            {
                _stuckTimer = 0f;
                if (_agent.hasPath && !_agent.isStopped)
                {
                    float distanceMoved = Vector3.Distance(transform.position, _lastPosition);
                    _isStuck = distanceMoved < _stuckDistanceThreshold;
                }
                else
                {
                    _isStuck = false;
                }
                _lastPosition = transform.position;
            }
        }
    }
}
