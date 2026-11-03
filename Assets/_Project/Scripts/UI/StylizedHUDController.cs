using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Eclipse.Damage;

namespace Eclipse.UI
{
    public class StylizedHUDController : MonoBehaviour
    {
        [Header("Theme Configuration")]
        [SerializeField] private StylizedUITheme _theme;

        [Header("Survivor Vitals (Top-Left)")]
        [SerializeField] private Image _healthFillImage;
        [SerializeField] private Image _healthGhostImage;
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private Image _staminaFillImage;
        [SerializeField] private TextMeshProUGUI _staminaText;
        [SerializeField] private CanvasGroup _criticalHealthVignette;

        [Header("Survival Meters")]
        [SerializeField] private Image _hungerFillImage;
        [SerializeField] private Image _thirstFillImage;

        [Header("Tactical Weapon Card (Bottom-Right)")]
        [SerializeField] private Image _weaponIcon;
        [SerializeField] private TextMeshProUGUI _weaponNameText;
        [SerializeField] private TextMeshProUGUI _currentAmmoText;
        [SerializeField] private TextMeshProUGUI _reserveAmmoText;
        [SerializeField] private TextMeshProUGUI _fireModeText;

        [Header("Threat Radar & Objective (Top-Right)")]
        [SerializeField] private TextMeshProUGUI _objectiveTitleText;
        [SerializeField] private TextMeshProUGUI _objectiveDistanceText;
        [SerializeField] private Image _threatIndicatorRing;

        [Header("Interaction & Reticle (Center)")]
        [SerializeField] private GameObject _interactionPromptPanel;
        [SerializeField] private TextMeshProUGUI _interactionPromptText;
        [SerializeField] private RectTransform _dynamicCrosshair;

        [Header("Smoothing")]
        [SerializeField] private float _barLerpSpeed = 8.0f;

        private float _targetHealthPercent = 1.0f;
        private float _currentHealthPercent = 1.0f;
        private float _targetStaminaPercent = 1.0f;
        private float _currentStaminaPercent = 1.0f;
        private float _vignettePulseTimer;

        public static StylizedHUDController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Update()
        {
            UpdateVitalFills();
            UpdateCriticalPulse();
        }

        private void UpdateVitalFills()
        {
            // Smoothly interpolate primary health bar
            if (_healthFillImage != null)
            {
                _currentHealthPercent = Mathf.Lerp(_currentHealthPercent, _targetHealthPercent, Time.deltaTime * _barLerpSpeed);
                _healthFillImage.fillAmount = _currentHealthPercent;
            }

            // Delayed ghost bar for impact feedback
            if (_healthGhostImage != null)
            {
                _healthGhostImage.fillAmount = Mathf.Lerp(_healthGhostImage.fillAmount, _targetHealthPercent, Time.deltaTime * (_barLerpSpeed * 0.4f));
            }

            // Stamina interpolation
            if (_staminaFillImage != null)
            {
                _currentStaminaPercent = Mathf.Lerp(_currentStaminaPercent, _targetStaminaPercent, Time.deltaTime * _barLerpSpeed);
                _staminaFillImage.fillAmount = _currentStaminaPercent;
            }
        }

        private void UpdateCriticalPulse()
        {
            if (_criticalHealthVignette == null) return;

            if (_targetHealthPercent <= 0.25f)
            {
                _vignettePulseTimer += Time.deltaTime * 4.0f;
                float alpha = (Mathf.Sin(_vignettePulseTimer) + 1.0f) * 0.5f * 0.75f + 0.15f;
                _criticalHealthVignette.alpha = alpha;
            }
            else
            {
                _criticalHealthVignette.alpha = Mathf.Lerp(_criticalHealthVignette.alpha, 0.0f, Time.deltaTime * 5.0f);
            }
        }

        public void SetHealth(float current, float max)
        {
            max = Mathf.Max(1.0f, max);
            _targetHealthPercent = Mathf.Clamp01(current / max);
            if (_healthText != null)
            {
                _healthText.text = $"{Mathf.CeilToInt(current)}";
            }
        }

        public void SetStamina(float current, float max)
        {
            max = Mathf.Max(1.0f, max);
            _targetStaminaPercent = Mathf.Clamp01(current / max);
            if (_staminaText != null)
            {
                _staminaText.text = $"{Mathf.CeilToInt(current)}";
            }
        }

        public void SetWeaponData(string weaponName, int currentAmmo, int reserveAmmo, string fireMode = "AUTO")
        {
            if (_weaponNameText != null) _weaponNameText.text = weaponName.ToUpper();
            if (_currentAmmoText != null) _currentAmmoText.text = currentAmmo.ToString("D2");
            if (_reserveAmmoText != null) _reserveAmmoText.text = $"/ {reserveAmmo:D3}";
            if (_fireModeText != null) _fireModeText.text = fireMode;
        }

        public void ShowInteractionPrompt(bool show, string message = "")
        {
            if (_interactionPromptPanel != null)
            {
                _interactionPromptPanel.SetActive(show);
            }
            if (_interactionPromptText != null && show)
            {
                _interactionPromptText.text = message;
            }
        }

        public void SetThreatLevel(float threatAlertZeroToOne)
        {
            if (_threatIndicatorRing == null || _theme == null) return;

            if (threatAlertZeroToOne > 0.05f)
            {
                _threatIndicatorRing.enabled = true;
                _threatIndicatorRing.color = Color.Lerp(_theme.BrandAmber, _theme.BrandRed, threatAlertZeroToOne);
            }
            else
            {
                _threatIndicatorRing.enabled = false;
            }
        }
    }
}
