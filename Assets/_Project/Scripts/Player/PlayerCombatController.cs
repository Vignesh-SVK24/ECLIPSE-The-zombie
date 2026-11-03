using System;
using UnityEngine;
using Eclipse.Character;
using Eclipse.Damage;
using Eclipse.Mobile;

namespace Eclipse.Player
{
    [Serializable]
    public class WeaponData
    {
        public string Name = "Weapon";
        public float Damage = 30f;
        public float FireRateSeconds = 0.2f;
        public int ClipSize = 15;
        public int CurrentClip = 15;
        public int ReserveAmmo = 60;
        public bool IsAutomatic = false;
        public float RecoilStrength = 3.5f;
        public float Range = 100f;
    }

    /// <summary>
    /// Production-ready combat controller for Ethan Cross.
    /// Manages weapon switching, raycast ballistics, sub-collider hitbox routing,
    /// animated recoil recovery, and mobile/desktop fire inputs.
    /// </summary>
    public class PlayerCombatController : MonoBehaviour
    {
        [Header("Weapon Armory")]
        [SerializeField] private WeaponData[] _weapons = new WeaponData[]
        {
            new WeaponData { Name = "9mm Tactical Pistol", Damage = 35f, FireRateSeconds = 0.25f, ClipSize = 15, CurrentClip = 15, ReserveAmmo = 60, IsAutomatic = false, RecoilStrength = 2.8f },
            new WeaponData { Name = "M4 Assault Rifle", Damage = 28f, FireRateSeconds = 0.11f, ClipSize = 30, CurrentClip = 30, ReserveAmmo = 120, IsAutomatic = true, RecoilStrength = 4.2f }
        };
        [SerializeField] private int _currentWeaponIndex = 0;

        [Header("References")]
        [SerializeField] private EthanCrossCharacter _character;
        [SerializeField] private CharacterAnimationController _animationController;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private LayerMask _hitMask = ~0;

        [Header("Combat State")]
        [SerializeField] private bool _isAiming;
        [SerializeField] private bool _isReloading;
        private float _lastFireTime;

        public WeaponData CurrentWeapon => _weapons[_currentWeaponIndex];
        public bool IsAiming => _isAiming;
        public bool IsReloading => _isReloading;
        public int CurrentWeaponIndex => _currentWeaponIndex;

        public event Action<WeaponData> OnWeaponFired;
        public event Action<WeaponData> OnWeaponReloaded;
        public event Action<WeaponData> OnWeaponSwitched;

        private void Awake()
        {
            _character = GetComponent<EthanCrossCharacter>();
            _animationController = GetComponent<CharacterAnimationController>();
        }

        private void Start()
        {
            if (_mainCamera == null && Camera.main != null)
            {
                _mainCamera = Camera.main;
            }
        }

        public void Initialize(EthanCrossCharacter character, CharacterAnimationController animController, Camera cam)
        {
            _character = character;
            _animationController = animController;
            _mainCamera = cam;
        }

        private void Update()
        {
            HandleWeaponInput();
        }

        private void HandleWeaponInput()
        {
            // 1. Aiming (Right Mouse Button or Mobile Aim Button)
            bool aimInput = Input.GetMouseButton(1);
            if (MobileInputManager.Instance != null && MobileInputManager.Instance.IsAiming)
            {
                aimInput = true;
            }
            _isAiming = aimInput;

            if (_animationController != null && _mainCamera != null)
            {
                float pitch = _mainCamera.transform.eulerAngles.x;
                if (pitch > 180f) pitch -= 360f;
                _animationController.SetAim(_isAiming, pitch, 0);
            }

            // 2. Weapon Switching (Keys 1 & 2 or Mobile Switch Button)
            if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchWeapon(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchWeapon(1);

            // 3. Reloading (Key R or Mobile Reload Button)
            if (Input.GetKeyDown(KeyCode.R) || (MobileInputManager.Instance != null && MobileInputManager.Instance.IsReloadPressed))
            {
                Reload();
            }

            // 4. Firing (Left Mouse Button or Mobile Fire Button)
            bool fireInput = CurrentWeapon.IsAutomatic ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);
            if (MobileInputManager.Instance != null && MobileInputManager.Instance.IsFirePressed)
            {
                fireInput = true;
            }

            if (fireInput && Time.time - _lastFireTime >= CurrentWeapon.FireRateSeconds && !_isReloading)
            {
                TryFire();
            }
        }

        public void SwitchWeapon(int index)
        {
            if (index < 0 || index >= _weapons.Length || index == _currentWeaponIndex || _isReloading) return;

            _currentWeaponIndex = index;
            OnWeaponSwitched?.Invoke(CurrentWeapon);
        }

        public void Reload()
        {
            var weapon = CurrentWeapon;
            if (_isReloading || weapon.CurrentClip >= weapon.ClipSize || weapon.ReserveAmmo <= 0) return;

            _isReloading = true;
            Invoke(nameof(FinishReload), 1.35f);
        }

        private void FinishReload()
        {
            var weapon = CurrentWeapon;
            int needed = weapon.ClipSize - weapon.CurrentClip;
            int added = Mathf.Min(needed, weapon.ReserveAmmo);

            weapon.CurrentClip += added;
            weapon.ReserveAmmo -= added;
            _isReloading = false;

            OnWeaponReloaded?.Invoke(weapon);
        }

        public void TryFire()
        {
            var weapon = CurrentWeapon;
            if (weapon.CurrentClip <= 0)
            {
                Reload();
                return;
            }

            _lastFireTime = Time.time;
            weapon.CurrentClip--;

            // Trigger procedural animation recoil
            if (_animationController != null)
            {
                _animationController.TriggerWeaponRecoil(weapon.RecoilStrength);
            }

            // Raycast Ballistics from Camera Center
            if (_mainCamera != null)
            {
                Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(ray, out RaycastHit hit, weapon.Range, _hitMask, QueryTriggerInteraction.Ignore))
                {
                    // Check precision HitBox routing (Headshot 2.5x critical, Limbs 0.75x)
                    if (hit.collider.TryGetComponent<HitBox>(out var hitBox))
                    {
                        DamageInfo info = new DamageInfo(
                            amount: weapon.Damage,
                            type: DamageType.Bullet,
                            source: gameObject,
                            hitPoint: hit.point,
                            hitNormal: hit.normal
                        );
                        hitBox.ReceiveDamage(info);
                    }
                    else if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
                    {
                        DamageInfo info = new DamageInfo(
                            amount: weapon.Damage,
                            type: DamageType.Bullet,
                            source: gameObject,
                            hitPoint: hit.point,
                            hitNormal: hit.normal
                        );
                        damageable.TakeDamage(info);
                    }
                }
            }

            OnWeaponFired?.Invoke(weapon);
        }
    }
}
