using System;
using UnityEngine;

namespace Eclipse.Damage
{
    public interface IDamageable
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }
        bool IsDead { get; }
        GameObject OwnerObject { get; }

        bool TakeDamage(DamageInfo info);
        void Heal(float amount, GameObject source = null);

        event Action<DamageInfo> OnDamageTaken;
        event Action<float> OnHealed;
        event Action<DamageInfo> OnDied;
    }
}
