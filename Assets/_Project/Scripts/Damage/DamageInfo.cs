using System;
using UnityEngine;

namespace Eclipse.Damage
{
    [Serializable]
    public struct DamageInfo
    {
        public float Amount;
        public DamageType Type;
        public GameObject Source;
        public Vector3 HitPoint;
        public Vector3 HitNormal;
        public Vector3 HitDirection;
        public HitLocation Location;
        public float CriticalMultiplier;
        public float StunDuration;
        public bool IsCritical;

        public float TotalDamage => Amount * (CriticalMultiplier > 0f ? CriticalMultiplier : 1f);

        public DamageInfo(
            float amount,
            DamageType type,
            GameObject source,
            Vector3 hitPoint = default,
            Vector3 hitNormal = default,
            Vector3 hitDirection = default,
            HitLocation location = HitLocation.Default,
            float criticalMultiplier = 1.0f,
            float stunDuration = 0f,
            bool isCritical = false)
        {
            Amount = Mathf.Max(0f, amount);
            Type = type;
            Source = source;
            HitPoint = hitPoint;
            HitNormal = hitNormal;
            HitDirection = hitDirection;
            Location = location;
            CriticalMultiplier = Mathf.Max(0.1f, criticalMultiplier);
            StunDuration = Mathf.Max(0f, stunDuration);
            IsCritical = isCritical;
        }

        public static DamageInfo CreateSimple(float amount, DamageType type, GameObject source)
        {
            return new DamageInfo(amount, type, source);
        }
    }
}
