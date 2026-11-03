using System;
using UnityEngine;

namespace Eclipse.Damage
{
    [RequireComponent(typeof(HealthComponent))]
    public class DestructibleObject : MonoBehaviour
    {
        [Header("Destruction Settings")]
        [SerializeField] private GameObject _intactModel;
        [SerializeField] private GameObject _destroyedModelPrefab;
        [SerializeField] private Collider _mainCollider;
        [SerializeField] private bool _destroyGameObjectOnDeath = true;
        [SerializeField] private float _destroyDelay = 0.1f;

        [Header("Explosion Properties (Optional)")]
        [SerializeField] private bool _explodesOnDeath = false;
        [SerializeField] private float _explosionRadius = 5.0f;
        [SerializeField] private float _explosionDamage = 80.0f;
        [SerializeField] private LayerMask _explosionLayerMask = ~0;

        private HealthComponent _health;

        public event Action<DestructibleObject> OnObjectDestroyed;

        private void Awake()
        {
            _health = GetComponent<HealthComponent>();
            if (_mainCollider == null)
            {
                _mainCollider = GetComponent<Collider>();
            }
        }

        private void OnEnable()
        {
            if (_health != null)
            {
                _health.OnDied += HandleDeath;
            }
        }

        private void OnDisable()
        {
            if (_health != null)
            {
                _health.OnDied -= HandleDeath;
            }
        }

        private void HandleDeath(DamageInfo fatalHit)
        {
            if (_explodesOnDeath)
            {
                TriggerExplosion();
            }

            if (_intactModel != null)
            {
                _intactModel.SetActive(false);
            }

            if (_mainCollider != null)
            {
                _mainCollider.enabled = false;
            }

            if (_destroyedModelPrefab != null)
            {
                Instantiate(_destroyedModelPrefab, transform.position, transform.rotation);
            }

            OnObjectDestroyed?.Invoke(this);

            if (_destroyGameObjectOnDeath)
            {
                Destroy(gameObject, _destroyDelay);
            }
        }

        private void TriggerExplosion()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius, _explosionLayerMask);
            foreach (var col in hits)
            {
                if (col.gameObject == gameObject) continue;

                // Check direct IDamageable or HitBox
                if (col.TryGetComponent<HitBox>(out var hitBox))
                {
                    Vector3 dir = (col.transform.position - transform.position).normalized;
                    DamageInfo blast = new DamageInfo(
                        amount: _explosionDamage,
                        type: DamageType.Explosion,
                        source: gameObject,
                        hitPoint: col.ClosestPoint(transform.position),
                        hitNormal: -dir,
                        hitDirection: dir,
                        location: HitLocation.Default);
                    hitBox.ProcessHit(blast);
                }
                else if (col.TryGetComponent<IDamageable>(out var dmg))
                {
                    Vector3 dir = (col.transform.position - transform.position).normalized;
                    DamageInfo blast = new DamageInfo(
                        amount: _explosionDamage,
                        type: DamageType.Explosion,
                        source: gameObject,
                        hitPoint: col.ClosestPoint(transform.position),
                        hitNormal: -dir,
                        hitDirection: dir,
                        location: HitLocation.Default);
                    dmg.TakeDamage(blast);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_explodesOnDeath)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, _explosionRadius);
            }
        }
    }
}
