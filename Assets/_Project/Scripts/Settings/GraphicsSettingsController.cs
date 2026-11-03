using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Eclipse.Settings
{
    public class GraphicsSettingsController : MonoBehaviour
    {
        [Header("Configuration Asset")]
        [SerializeField] private QualityPresetConfiguration _presetConfiguration;

        [Header("Current State")]
        [SerializeField] private QualityLevelTier _activeTier = QualityLevelTier.Medium;

        public static event Action<QualityPresetData> OnQualityPresetApplied;

        public QualityLevelTier ActiveTier => _activeTier;
        public QualityPresetConfiguration PresetConfiguration => _presetConfiguration;

        private void Awake()
        {
            ApplyInitialSettings();
        }

        public void ApplyInitialSettings()
        {
            if (_presetConfiguration == null)
            {
                _presetConfiguration = ScriptableObject.CreateInstance<QualityPresetConfiguration>();
            }

            // Auto-detect optimal tier if not already established
            var caps = MobileDeviceProfile.Detect();
            _activeTier = caps.RecommendedTier;

            ApplyPreset(_activeTier);
        }

        public void ApplyPreset(QualityLevelTier tier)
        {
            if (_presetConfiguration == null)
            {
                Debug.LogWarning("[ECLIPSE GraphicsSettingsController] No configuration asset bound. Using default.");
                return;
            }

            QualityPresetData preset = _presetConfiguration.GetPreset(tier);
            if (preset == null)
            {
                Debug.LogError($"[ECLIPSE GraphicsSettingsController] Could not retrieve preset for tier: {tier}");
                return;
            }

            _activeTier = tier;

            // Apply standard engine quality settings
            QualitySettings.lodBias = preset.LodBias;
            QualitySettings.anisotropicFiltering = preset.AnisotropicFiltering;

            // Apply target frame rate
            if (preset.TargetFrameRate > 0)
            {
                Application.targetFrameRate = preset.TargetFrameRate;
                QualitySettings.vSyncCount = 0;
            }

            // Apply URP specific settings if current pipeline is URP
            ApplyURPSettings(preset);

            Debug.Log($"[ECLIPSE GraphicsSettingsController] Applied '{preset.PresetName}' (RenderScale: {preset.RenderScale:F2}, ShadowDist: {preset.ShadowDistance}m, FPS: {preset.TargetFrameRate}).");
            OnQualityPresetApplied?.Invoke(preset);
        }

        private void ApplyURPSettings(QualityPresetData preset)
        {
            RenderPipelineAsset currentPipeline = GraphicsSettings.currentRenderPipeline;
            if (currentPipeline is UniversalRenderPipelineAsset urpAsset)
            {
                urpAsset.renderScale = preset.RenderScale;
                urpAsset.shadowDistance = preset.ShadowDistance;
                urpAsset.shadowCascadeCount = preset.ShadowCascadeCount;
                urpAsset.msaaSamples = preset.MsaaSamples;
            }
        }
    }
}
