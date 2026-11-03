using System;
using UnityEngine;

namespace Eclipse.Settings
{
    [CreateAssetMenu(fileName = "QualityPresetConfiguration", menuName = "ECLIPSE/Settings/Quality Preset Configuration")]
    public class QualityPresetConfiguration : ScriptableObject
    {
        [Header("Default Tier")]
        [SerializeField] private QualityLevelTier _defaultTier = QualityLevelTier.Medium;

        [Header("Configured Presets")]
        [SerializeField] private QualityPresetData _lowPreset;
        [SerializeField] private QualityPresetData _mediumPreset;
        [SerializeField] private QualityPresetData _highPreset;

        public QualityLevelTier DefaultTier => _defaultTier;

        public QualityPresetData GetPreset(QualityLevelTier tier)
        {
            switch (tier)
            {
                case QualityLevelTier.Low:
                    return _lowPreset ?? CreateDefaultLow();
                case QualityLevelTier.Medium:
                    return _mediumPreset ?? CreateDefaultMedium();
                case QualityLevelTier.High:
                    return _highPreset ?? CreateDefaultHigh();
                default:
                    return _mediumPreset ?? CreateDefaultMedium();
            }
        }

        private void Reset()
        {
            _lowPreset = CreateDefaultLow();
            _mediumPreset = CreateDefaultMedium();
            _highPreset = CreateDefaultHigh();
        }

        public static QualityPresetData CreateDefaultLow()
        {
            return new QualityPresetData(
                tier: QualityLevelTier.Low,
                presetName: "Low (Battery Saver)",
                renderScale: 0.75f,
                msaaSamples: 1,
                anisotropicFiltering: AnisotropicFiltering.Disable,
                enableShadows: true,
                shadowDistance: 25f,
                shadowCascadeCount: 1,
                shadowResolution: 512,
                vegetationDensity: 0.35f,
                particleBudgetMultiplier: 0.35f,
                lodBias: 0.7f,
                enablePostProcessing: false,
                enableBloom: false,
                enableAmbientOcclusion: false,
                targetFrameRate: 30
            );
        }

        public static QualityPresetData CreateDefaultMedium()
        {
            return new QualityPresetData(
                tier: QualityLevelTier.Medium,
                presetName: "Medium (Balanced)",
                renderScale: 0.88f,
                msaaSamples: 2,
                anisotropicFiltering: AnisotropicFiltering.Enable,
                enableShadows: true,
                shadowDistance: 45f,
                shadowCascadeCount: 2,
                shadowResolution: 1024,
                vegetationDensity: 0.70f,
                particleBudgetMultiplier: 0.70f,
                lodBias: 1.0f,
                enablePostProcessing: true,
                enableBloom: true,
                enableAmbientOcclusion: false,
                targetFrameRate: 30
            );
        }

        public static QualityPresetData CreateDefaultHigh()
        {
            return new QualityPresetData(
                tier: QualityLevelTier.High,
                presetName: "High (Photorealistic)",
                renderScale: 1.0f,
                msaaSamples: 4,
                anisotropicFiltering: AnisotropicFiltering.ForceEnable,
                enableShadows: true,
                shadowDistance: 75f,
                shadowCascadeCount: 4,
                shadowResolution: 2048,
                vegetationDensity: 1.0f,
                particleBudgetMultiplier: 1.0f,
                lodBias: 1.3f,
                enablePostProcessing: true,
                enableBloom: true,
                enableAmbientOcclusion: true,
                targetFrameRate: 60
            );
        }
    }
}
