using System;
using UnityEngine;

namespace Eclipse.Damage
{
    public static class DamageSystem
    {
        public static event Action<DamageInfo, IDamageable> OnGlobalDamageDelivered;
        public static event Action<DamageInfo, IDamageable> OnGlobalTargetKilled;

        public static bool ApplyDamage(GameObject target, DamageInfo info)
        {
            if (target == null || info.Amount <= 0f)
            {
                return false;
            }

            // Multiplayer authority preparation check
            if (!ValidateAuthority(info.Source, target))
            {
                Debug.LogWarning($"[ECLIPSE DamageSystem] Damage rejected due to authority check failure between source and target.");
                return false;
            }

            // Check if struck object is a HitBox (precise sub-collider)
            if (target.TryGetComponent<HitBox>(out var hitBox))
            {
                bool hitProcessed = hitBox.ProcessHit(info);
                if (hitProcessed)
                {
                    NotifyGlobalDamage(target.GetComponentInParent<IDamageable>(), info);
                }
                return hitProcessed;
            }

            // Check direct IDamageable on target or its parents
            IDamageable damageable = target.GetComponent<IDamageable>() ?? target.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                bool hitProcessed = damageable.TakeDamage(info);
                if (hitProcessed)
                {
                    NotifyGlobalDamage(damageable, info);
                }
                return hitProcessed;
            }

            return false;
        }

        public static bool ApplyDamage(Collider hitCollider, DamageInfo info)
        {
            if (hitCollider == null) return false;
            return ApplyDamage(hitCollider.gameObject, info);
        }

        private static void NotifyGlobalDamage(IDamageable damageable, DamageInfo info)
        {
            if (damageable == null) return;

            OnGlobalDamageDelivered?.Invoke(info, damageable);

            if (damageable.IsDead)
            {
                OnGlobalTargetKilled?.Invoke(info, damageable);
            }
        }

        /// <summary>
        /// Stub hook for multiplayer authority validation in Part 3.
        /// Always returns true in single-player foundation.
        /// </summary>
        public static bool ValidateAuthority(GameObject source, GameObject target)
        {
            // Single-player foundation: local authority valid
            return true;
        }
    }
}
