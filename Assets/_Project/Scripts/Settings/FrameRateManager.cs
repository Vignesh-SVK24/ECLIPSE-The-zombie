using System;
using UnityEngine;

namespace Eclipse.Settings
{
    public class FrameRateManager : MonoBehaviour
    {
        public enum TargetFPSMode
        {
            BatterySaver30 = 30,
            Performance60 = 60
        }

        [Header("Frame Rate Configuration")]
        [SerializeField] private TargetFPSMode _defaultMode = TargetFPSMode.Performance60;
        [SerializeField] private bool _disableVSyncOnStart = true;

        public static event Action<int> OnTargetFrameRateChanged;

        public int CurrentTargetFrameRate => Application.targetFrameRate;

        private void Awake()
        {
            InitializeFrameRate();
        }

        public void InitializeFrameRate()
        {
            if (_disableVSyncOnStart)
            {
                // Disabling vSync count is required for Application.targetFrameRate on mobile devices
                QualitySettings.vSyncCount = 0;
            }

            SetTargetFrameRate((int)_defaultMode);
        }

        public void SetTargetFrameRate(int targetFps)
        {
            // Clamp within sane boundaries for mobile OLED/LCD panels
            int clampedFps = Mathf.Clamp(targetFps, 30, 120);
            Application.targetFrameRate = clampedFps;
            Debug.Log($"[ECLIPSE FrameRateManager] Target frame rate set to {clampedFps} FPS (vSync={QualitySettings.vSyncCount}).");
            OnTargetFrameRateChanged?.Invoke(clampedFps);
        }

        public void SetMode(TargetFPSMode mode)
        {
            SetTargetFrameRate((int)mode);
        }
    }
}
