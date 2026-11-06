using System;
using UnityEngine;

namespace Eclipse.Environment
{
    public enum CoverClassification
    {
        /// <summary>
        /// Heavy Cover: Trucks, ambulances, concrete Jersey barriers, structural walls.
        /// 100% blocks character movement, explosive blast fragments, and all ballistic projectiles.
        /// </summary>
        HeavyCover,

        /// <summary>
        /// Medium Cover: Civilian cars, reinforced crates, metal riot barricades.
        /// Blocks character movement. Blocks bullets hitting solid body panels; allows shots through clear windows.
        /// </summary>
        MediumCover,

        /// <summary>
        /// Light Cover: Small wooden supply boxes, light furniture, chain-link fences.
        /// Blocks character movement, but can be penetrated by high-caliber munitions with reduced damage.
        /// </summary>
        LightCover
    }

    /// <summary>
    /// Manages tactical environmental cover props in ECLIPSE: THE LAST CIVILIZATION.
    /// Provides physical ballistic protection, bullet collision detection, and damage attenuation.
    /// Shields players and zombies from line-of-sight projectile damage when positioned behind cover.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TacticalCoverProp : MonoBehaviour
    {
        [Header("Tactical Classification")]
        [SerializeField] private CoverClassification _classification = CoverClassification.HeavyCover;
        [SerializeField] private bool _isDestructible = false;
        [SerializeField] private float _maxDurability = 500f;
        [SerializeField] private float _currentDurability = 500f;

        [Header("Ballistic Properties")]
        [Range(0f, 1f)]
        [SerializeField] private float _penetrationFactor = 0f; // 0 = completely bulletproof, 0.5 = 50% damage passthrough
        [SerializeField] private MaterialType _surfaceMaterial = MaterialType.Concrete;

        [Header("VFX & Audio Feedback")]
        [SerializeField] private GameObject _bulletSparksVFX;
        [SerializeField] private GameObject _impactDustVFX;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _impactSound;

        public enum MaterialType
        {
            Metal,
            Concrete,
            Wood,
            Glass,
            Rubber
        }

        public CoverClassification Classification => _classification;
        public bool IsDestructible => _isDestructible;
        public float DurabilityPercent => _isDestructible ? (_currentDurability / _maxDurability) : 1f;

        private void Awake()
        {
            _currentDurability = _maxDurability;

            // Auto-configure ballistic penetration based on classification
            switch (_classification)
            {
                case CoverClassification.HeavyCover:
                    _penetrationFactor = 0f; // 100% bulletproof
                    break;
                case CoverClassification.MediumCover:
                    _penetrationFactor = 0f; // Blocks bullets hitting solid mesh
                    break;
                case CoverClassification.LightCover:
                    _penetrationFactor = 0.45f; // Light cover permits 45% penetration
                    break;
            }
        }

        /// <summary>
        /// Handles a projectile or raycast bullet impact against this cover obstacle.
        /// Spawns localized impact VFX, applies damage to destructible cover,
        /// and calculates whether the projectile was fully blocked or penetrated.
        /// </summary>
        /// <param name="hitPoint">World position of bullet impact.</param>
        /// <param name="hitNormal">Normal vector of impacted surface.</param>
        /// <param name="incomingDamage">Damage carried by the bullet.</param>
        /// <param name="penetratedDamage">Remaining damage if penetrated, or 0 if blocked.</param>
        /// <returns>True if the bullet was completely blocked; false if it penetrated.</returns>
        public bool ProcessBulletImpact(Vector3 hitPoint, Vector3 hitNormal, int incomingDamage, out int penetratedDamage)
        {
            SpawnImpactVFX(hitPoint, hitNormal);

            if (_isDestructible)
            {
                _currentDurability -= incomingDamage;
                if (_currentDurability <= 0)
                {
                    OnCoverDestroyed();
                }
            }

            if (_penetrationFactor <= 0.01f)
            {
                // Completely blocked by heavy or medium cover
                penetratedDamage = 0;
                return true; // Blocked
            }

            // Light cover allows reduced penetration
            penetratedDamage = Mathf.RoundToInt(incomingDamage * _penetrationFactor);
            return false; // Penetrated
        }

        private void SpawnImpactVFX(Vector3 point, Vector3 normal)
        {
            if (_impactDustVFX != null)
            {
                Instantiate(_impactDustVFX, point, Quaternion.LookRotation(normal));
            }
            if (_bulletSparksVFX != null)
            {
                Instantiate(_bulletSparksVFX, point, Quaternion.LookRotation(normal));
            }
            if (_audioSource != null && _impactSound != null)
            {
                _audioSource.PlayOneShot(_impactSound);
            }
        }

        private void OnCoverDestroyed()
        {
            // Optional destruction callback or visual state change
            gameObject.SetActive(false);
        }
    }
}
