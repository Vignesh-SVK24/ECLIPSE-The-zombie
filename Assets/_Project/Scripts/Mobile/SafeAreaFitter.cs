using System;
using UnityEngine;

namespace Eclipse.Mobile
{
    /// <summary>
    /// Anchors a RectTransform within the device's hardware safe area, avoiding notches,
    /// camera cutouts, curved edges, and the iOS Home Indicator.
    /// Supports per-edge constraints, custom padding offsets, and Editor simulation.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class SafeAreaFitter : MonoBehaviour
    {
        [Header("Edge Insets Configuration")]
        [SerializeField] private bool _conformLeft = true;
        [SerializeField] private bool _conformRight = true;
        [SerializeField] private bool _conformTop = true;
        [SerializeField] private bool _conformBottom = true;

        [Header("Additional Padding Offsets (Pixels)")]
        [Tooltip("Extra clearance for iOS Home Indicator (recommended: 16-24px on iPhone)")]
        [SerializeField] private float _extraBottomPadding = 0f;
        [Tooltip("Extra clearance for status bar / Dynamic Island")]
        [SerializeField] private float _extraTopPadding = 0f;
        [SerializeField] private float _extraLeftPadding = 0f;
        [SerializeField] private float _extraRightPadding = 0f;

        [Header("Editor Simulation")]
        [SerializeField] private bool _simulateInEditor = true;
        [SerializeField] private EditorDeviceSimulation _simulatedDevice = EditorDeviceSimulation.iPhone15Pro_DynamicIsland;

        public enum EditorDeviceSimulation
        {
            None,
            iPhone15Pro_DynamicIsland, // Top 59px, Bottom 34px
            iPhoneX_Notch,             // Top 44px, Bottom 34px
            GalaxyS24_HolePunch,       // Left 32px or Top 32px
            iPadPro_HomeBar            // Bottom 20px
        }

        private RectTransform _rectTransform;
        private Rect _lastSafeArea = new Rect(0, 0, 0, 0);
        private Vector2Int _lastScreenSize = new Vector2Int(0, 0);
        private ScreenOrientation _lastOrientation = ScreenOrientation.AutoRotation;

        public event Action<Rect> OnSafeAreaApplied;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            ApplySafeArea();
        }

        private void Update()
        {
            if (HasScreenChanged())
            {
                ApplySafeArea();
            }
        }

        private bool HasScreenChanged()
        {
            return _lastSafeArea != GetCurrentSafeArea() ||
                   _lastScreenSize.x != Screen.width ||
                   _lastScreenSize.y != Screen.height ||
                   _lastOrientation != Screen.orientation;
        }

        public Rect GetCurrentSafeArea()
        {
#if UNITY_EDITOR
            if (_simulateInEditor && _simulatedDevice != EditorDeviceSimulation.None)
            {
                return GetSimulatedSafeArea();
            }
#endif
            return Screen.safeArea;
        }

        public void ApplySafeArea()
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
                if (_rectTransform == null) return;
            }

            Rect safeArea = GetCurrentSafeArea();
            _lastSafeArea = safeArea;
            _lastScreenSize = new Vector2Int(Screen.width, Screen.height);
            _lastOrientation = Screen.orientation;

            if (Screen.width <= 0 || Screen.height <= 0) return;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            // Apply per-edge constraints
            if (!_conformLeft) anchorMin.x = 0f;
            if (!_conformBottom) anchorMin.y = 0f;
            if (!_conformRight) anchorMax.x = Screen.width;
            if (!_conformTop) anchorMax.y = Screen.height;

            // Apply extra padding offsets (converted to normalized viewport coordinates)
            if (_extraLeftPadding > 0f) anchorMin.x += _extraLeftPadding;
            if (_extraBottomPadding > 0f) anchorMin.y += _extraBottomPadding;
            if (_extraRightPadding > 0f) anchorMax.x -= _extraRightPadding;
            if (_extraTopPadding > 0f) anchorMax.y -= _extraTopPadding;

            // Clamp and normalize
            anchorMin.x = Mathf.Clamp01(anchorMin.x / Screen.width);
            anchorMin.y = Mathf.Clamp01(anchorMin.y / Screen.height);
            anchorMax.x = Mathf.Clamp01(anchorMax.x / Screen.width);
            anchorMax.y = Mathf.Clamp01(anchorMax.y / Screen.height);

            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;
            _rectTransform.offsetMin = Vector2.zero;
            _rectTransform.offsetMax = Vector2.zero;

            OnSafeAreaApplied?.Invoke(safeArea);
        }

#if UNITY_EDITOR
        private Rect GetSimulatedSafeArea()
        {
            float width = Screen.width;
            float height = Screen.height;

            switch (_simulatedDevice)
            {
                case EditorDeviceSimulation.iPhone15Pro_DynamicIsland:
                    // Landscape: Left/Right notch ~59px, bottom home bar ~34px
                    return new Rect(59f, 34f, Mathf.Max(0, width - 118f), Mathf.Max(0, height - 54f));

                case EditorDeviceSimulation.iPhoneX_Notch:
                    return new Rect(44f, 34f, Mathf.Max(0, width - 88f), Mathf.Max(0, height - 54f));

                case EditorDeviceSimulation.GalaxyS24_HolePunch:
                    return new Rect(32f, 0f, Mathf.Max(0, width - 64f), height);

                case EditorDeviceSimulation.iPadPro_HomeBar:
                    return new Rect(0f, 20f, width, Mathf.Max(0, height - 20f));

                default:
                    return new Rect(0, 0, width, height);
            }
        }
#endif
    }
}
