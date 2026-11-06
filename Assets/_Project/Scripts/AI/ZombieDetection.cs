using System;
using UnityEngine;

namespace Eclipse.AI
{
    public class ZombieDetection : MonoBehaviour
    {
        [Header("Vision Configuration")]
        [SerializeField] private float _visionRange = 18f;
        [SerializeField] private float _fieldOfView = 110f;
        [SerializeField] private Vector3 _eyeOffset = new Vector3(0f, 1.6f, 0f);
        [SerializeField] private LayerMask _targetMask;
        [SerializeField] private LayerMask _obstructionMask;

        [Header("Update Frequency (Mobile Optimization)")]
        [SerializeField] private float _checkInterval = 0.2f;

        private float _checkTimer;
        private Transform _detectedTarget;

        public Transform DetectedTarget => _detectedTarget;
        public float VisionRange { get => _visionRange; set => _visionRange = value; }
        public float FieldOfView { get => _fieldOfView; set => _fieldOfView = value; }

        public event Action<Transform> OnTargetSpotted;
        public event Action OnTargetLost;

        private void Awake()
        {
            if (_targetMask.value == 0)
            {
                _targetMask = LayerMask.GetMask("Player", "Default");
                if (_targetMask.value == 0) _targetMask = ~0;
            }
        }

        private void Update()
        {
            _checkTimer += Time.deltaTime;
            if (_checkTimer >= _checkInterval)
            {
                _checkTimer = 0f;
                PerformVisionCheck();
            }
        }

        private void PerformVisionCheck()
        {
            Vector3 eyePos = transform.position + _eyeOffset;
            Collider[] colliders = Physics.OverlapSphere(eyePos, _visionRange, _targetMask);

            Transform bestTarget = null;
            float closestDistance = float.MaxValue;

            foreach (var col in colliders)
            {
                if (col.gameObject == gameObject) continue;

                Vector3 targetCenter = col.bounds.center;
                Vector3 toTarget = targetCenter - eyePos;
                float distance = toTarget.magnitude;

                if (distance > _visionRange) continue;

                // Field of view check
                float angle = Vector3.Angle(transform.forward, toTarget.normalized);
                if (angle > _fieldOfView * 0.5f && distance > 3.0f) // Close proximity 360-degree awareness within 3 meters
                {
                    continue;
                }

                // Line of Sight obstruction raycast (cannot see through walls)
                if (Physics.Raycast(eyePos, toTarget.normalized, out var hit, distance, _obstructionMask))
                {
                    if (hit.collider.gameObject != col.gameObject && !hit.collider.transform.IsChildOf(col.transform))
                    {
                        // Struck a wall or obstacle
                        continue;
                    }
                }

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestTarget = col.transform;
                }
            }

            if (bestTarget != null && _detectedTarget != bestTarget)
            {
                _detectedTarget = bestTarget;
                OnTargetSpotted?.Invoke(_detectedTarget);
            }
            else if (bestTarget == null && _detectedTarget != null)
            {
                _detectedTarget = null;
                OnTargetLost?.Invoke();
            }
        }

        public bool HasLineOfSightTo(Vector3 worldPoint)
        {
            Vector3 eyePos = transform.position + _eyeOffset;
            Vector3 dir = worldPoint - eyePos;
            float dist = dir.magnitude;

            if (Physics.Raycast(eyePos, dir.normalized, out var hit, dist, _obstructionMask))
            {
                return hit.distance >= dist - 0.2f;
            }
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Vector3 eyePos = transform.position + _eyeOffset;
            Gizmos.DrawWireSphere(eyePos, _visionRange);
        }
    }
}
