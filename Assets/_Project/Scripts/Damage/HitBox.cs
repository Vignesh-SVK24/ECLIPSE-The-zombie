using UnityEngine;

namespace Eclipse.Damage
{
    [RequireComponent(typeof(Collider))]
    public class HitBox : MonoBehaviour
    {
        [Header("Location Configuration")]
        [SerializeField] private HitLocation _location = HitLocation.Torso;
        [SerializeField] private float _damageMultiplier = 1.0f;
        [SerializeField] private bool _isCriticalPoint = false;

        [Header("Target Damageable")]
        [SerializeField] private MonoBehaviour _targetDamageableComponent;

        private IDamageable _damageable;

        public HitLocation Location => _location;
        public float DamageMultiplier => _damageMultiplier;
        public bool IsCriticalPoint => _isCriticalPoint;

        private void Awake()
        {
            ResolveDamageable();
        }

        private void ResolveDamageable()
        {
            if (_targetDamageableComponent is IDamageable explicitTarget)
            {
                _damageable = explicitTarget;
                return;
            }

            _damageable = GetComponentInParent<IDamageable>();
            if (_damageable == null)
            {
                Debug.LogWarning($"[ECLIPSE HitBox] HitBox on '{gameObject.name}' has no IDamageable component in parent hierarchy.");
            }
        }

        public bool ProcessHit(DamageInfo incomingInfo)
        {
            if (_damageable == null)
            {
                ResolveDamageable();
                if (_damageable == null) return false;
            }

            DamageInfo enriched = incomingInfo;
            enriched.Location = _location;
            enriched.CriticalMultiplier = _damageMultiplier;
            enriched.IsCritical = _isCriticalPoint || _damageMultiplier > 1.0f;

            return _damageable.TakeDamage(enriched);
        }
    }
}
