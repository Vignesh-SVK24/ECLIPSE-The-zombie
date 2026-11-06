using System;
using UnityEngine;
using UnityEngine.UI;

namespace Eclipse.Mobile
{
    public enum DeviceFormFactor
    {
        StandardPhone, // 16:9 to 19.5:9 (~1.77 - 2.16)
        UltrawidePhone, // 20:9 to 21:9 (> 2.2)
        Tablet,        // 4:3, 16:10, 3:2 (< 1.6)
        FoldableSquare // 1:1 to 5:4
    }

    /// <summary>
    /// Monitors device aspect ratio and resolution, automatically configuring CanvasScalers,
    /// HUD control scales, and layout anchoring for phones, ultrawides, and tablets.
    /// </summary>
    [DisallowMultipleComponent]
    public class ResponsiveLayoutManager : MonoBehaviour
    {
        private static ResponsiveLayoutManager _instance;
        public static ResponsiveLayoutManager Instance => _instance;

        [Header("Canvas Scaler Target")]
        [SerializeField] private CanvasScaler _targetCanvasScaler;
        [SerializeField] private Vector2 _referenceResolution = new Vector2(1920f, 1080f);

        [Header("Responsive Scale Factors")]
        [Tooltip("Scale multiplier for touch buttons & vitals on tablets (aspect ratio < 1.6)")]
        [Range(0.6f, 1.0f)]
        [SerializeField] private float _tabletScaleFactor = 0.85f;
        [Tooltip("Scale multiplier on ultrawide phones (aspect ratio > 2.2)")]
        [Range(0.8f, 1.2f)]
        [SerializeField] private float _ultrawideScaleFactor = 1.0f;
        [Tooltip("Standard phone scale factor")]
        [SerializeField] private float _standardPhoneScaleFactor = 1.0f;

        [Header("Target UI Transforms (Optional)")]
        [SerializeField] private RectTransform _virtualControlsContainer;
        [SerializeField] private RectTransform _topVitalsContainer;
        [SerializeField] private RectTransform _actionButtonsContainer;

        private float _lastAspectRatio = 0f;
        private DeviceFormFactor _currentFormFactor = DeviceFormFactor.StandardPhone;
        private float _currentScaleFactor = 1.0f;

        public DeviceFormFactor CurrentFormFactor => _currentFormFactor;
        public float CurrentScaleFactor => _currentScaleFactor;
        public float CurrentAspectRatio => (float)Screen.width / Mathf.Max(1, Screen.height);

        public event Action<DeviceFormFactor, float> OnLayoutConfigured;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);

            if (_targetCanvasScaler == null)
            {
                _targetCanvasScaler = GetComponent<CanvasScaler>();
            }

            EvaluateAndApplyLayout();
        }

        private void Update()
        {
            float currentAspect = CurrentAspectRatio;
            if (Mathf.Abs(currentAspect - _lastAspectRatio) > 0.02f)
            {
                EvaluateAndApplyLayout();
            }
        }

        public void EvaluateAndApplyLayout()
        {
            float width = Screen.width;
            float height = Mathf.Max(1, Screen.height);
            float aspect = width / height;
            _lastAspectRatio = aspect;

            // Determine Form Factor based on aspect ratio in landscape
            if (aspect < 1.35f)
            {
                _currentFormFactor = DeviceFormFactor.FoldableSquare;
                _currentScaleFactor = _tabletScaleFactor * 0.95f;
            }
            else if (aspect < 1.62f)
            {
                _currentFormFactor = DeviceFormFactor.Tablet; // iPad 4:3 is 1.33, Android tablet 16:10 is 1.60
                _currentScaleFactor = _tabletScaleFactor;
            }
            else if (aspect > 2.15f)
            {
                _currentFormFactor = DeviceFormFactor.UltrawidePhone; // Sony 21:9 is 2.33, modern 20:9 is 2.22
                _currentScaleFactor = _ultrawideScaleFactor;
            }
            else
            {
                _currentFormFactor = DeviceFormFactor.StandardPhone; // 16:9 is 1.77, 18:9 is 2.0, 19.5:9 is 2.16
                _currentScaleFactor = _standardPhoneScaleFactor;
            }

            ConfigureCanvasScaler(aspect);
            ApplyResponsiveScaling();

            OnLayoutConfigured?.Invoke(_currentFormFactor, _currentScaleFactor);
        }

        private void ConfigureCanvasScaler(float aspect)
        {
            if (_targetCanvasScaler == null) return;

            _targetCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _targetCanvasScaler.referenceResolution = _referenceResolution;
            _targetCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

            switch (_currentFormFactor)
            {
                case DeviceFormFactor.Tablet:
                case DeviceFormFactor.FoldableSquare:
                    // On tablets, matching width (0.0) ensures horizontal layout doesn't get squished
                    _targetCanvasScaler.matchWidthOrHeight = 0.15f;
                    break;

                case DeviceFormFactor.UltrawidePhone:
                    // On ultrawide phones, matching height (1.0) prevents controls pushing off screen top/bottom
                    _targetCanvasScaler.matchWidthOrHeight = 0.85f;
                    break;

                case DeviceFormFactor.StandardPhone:
                default:
                    // Standard balance
                    _targetCanvasScaler.matchWidthOrHeight = 0.5f;
                    break;
            }
        }

        private void ApplyResponsiveScaling()
        {
            Vector3 targetScale = Vector3.one * _currentScaleFactor;

            if (_virtualControlsContainer != null)
            {
                _virtualControlsContainer.localScale = targetScale;
            }

            if (_actionButtonsContainer != null)
            {
                _actionButtonsContainer.localScale = targetScale;
            }

            if (_topVitalsContainer != null)
            {
                // Slightly less aggressive scaling for readable vitals
                float vitalsScale = Mathf.Lerp(1.0f, _currentScaleFactor, 0.6f);
                _topVitalsContainer.localScale = Vector3.one * vitalsScale;
            }
        }
    }
}
