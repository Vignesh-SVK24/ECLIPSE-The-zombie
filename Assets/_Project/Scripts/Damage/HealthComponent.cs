using System;
using UnityEngine;

namespace Eclipse.Damage
{
    [DisallowMultipleComponent]
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [Header("Health Settings")]
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private float _currentHealth = 100f;
        [SerializeField] private bool _isInvulnerable = false;

        [Header("Armor & Resistance")]
        [Range(0f, 0.9f)]
        [SerializeField] private float _damageResistance = 0f;

        [Header("Invulnerability Window")]
        [SerializeField] private float _invulnerabilityDurationOnHit = 0f;
        private float _lastHitTime = -999f;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        public bool IsDead => _currentHealth <= 0f;
        public GameObject OwnerObject => gameObject;
        public bool IsInvulnerable { get => _isInvulnerable; set => _isInvulnerable = value; }

        public event Action<DamageInfo> OnDamageTaken;
        public event Action<float> OnHealed;
        public event Action<DamageInfo> OnDied;

        protected virtual void Awake()
        {
            _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);
        }

        public virtual bool TakeDamage(DamageInfo info)
        {
            if (IsDead || _isInvulnerable)
            {
                return false;
            }

            if (_invulnerabilityDurationOnHit > 0f && Time.time - _lastHitTime < _invulnerabilityDurationOnHit)
            {
                return false;
            }

            _lastHitTime = Time.time;

            float calculatedDamage = info.TotalDamage * (1f - _damageResistance);
            calculatedDamage = Mathf.Max(0.5f, calculatedDamage);

            _currentHealth = Mathf.Max(0f, _currentHealth - calculatedDamage);

            DamageInfo appliedInfo = info;
            appliedInfo.Amount = calculatedDamage;

            OnDamageTaken?.Invoke(appliedInfo);

            if (_currentHealth <= 0f)
            {
                Die(appliedInfo);
            }

            return true;
        }

        public virtual void Heal(float amount, GameObject source = null)
        {
            if (IsDead || amount <= 0f)
            {
                return;
            }

            float previousHealth = _currentHealth;
            _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
            float actualHealed = _currentHealth - previousHealth;

            if (actualHealed > 0f)
            {
                OnHealed?.Invoke(actualHealed);
            }
        }

        protected virtual void Die(DamageInfo fatalHit)
        {
            OnDied?.Invoke(fatalHit);
        }

        public virtual void ResetHealth()
        {
            _currentHealth = _maxHealth;
            _lastHitTime = -999f;
        }

        public void SetMaxHealth(float newMaxHealth, bool scaleCurrent = false)
        {
            float ratio = _maxHealth > 0f ? _currentHealth / _maxHealth : 1f;
            _maxHealth = Mathf.Max(1f, newMaxHealth);
            if (scaleCurrent)
            {
                _currentHealth = _maxHealth * ratio;
            }
            else
            {
                _currentHealth = Mathf.Min(_currentHealth, _maxHealth);
            }
        }
    }
}
