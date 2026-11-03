using System;
using UnityEngine;

namespace Eclipse.Settings
{
    public enum QualityLevelTier
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    [Serializable]
    public class QualityPresetData
    {
        [Header("Identity")]
        [SerializeField] private QualityLevelTier _tier;
        [SerializeField] private string _presetName;

        [Header("Rendering")]
        [Range(0.5f, 1.0f)]
        [SerializeField] private float _renderScale = 1.0f;
        [SerializeField] private int _msaaSamples = 1;
        [SerializeField] private AnisotropicFiltering _anisotropicFiltering = AnisotropicFiltering.Enable;

        [Header("Shadows")]
        [SerializeField] private bool _enableShadows = true;
        [SerializeField] private float _shadowDistance = 50f;
        [SerializeField] private int _shadowCascadeCount = 2;
        [SerializeField] private int _shadowResolution = 1024;

        [Header("Environment & Geometry")]
        [Range(0.2f, 1.0f)]
        [SerializeField] private float _vegetationDensity = 1.0f;
        [Range(0.2f, 1.0f)]
        [SerializeField] private float _particleBudgetMultiplier = 1.0f;
        [Range(0.5f, 2.0f)]
        [SerializeField] private float _lodBias = 1.0f;

        [Header("Post Processing")]
        [SerializeField] private bool _enablePostProcessing = true;
        [SerializeField] private bool _enableBloom = true;
        [SerializeField] private bool _enableAmbientOcclusion = false;

        [Header("Performance")]
        [SerializeField] private int _targetFrameRate = 60;

        public QualityLevelTier Tier => _tier;
        public string PresetName => _presetName;
        public float RenderScale => _renderScale;
        public int MsaaSamples => _msaaSamples;
        public AnisotropicFiltering AnisotropicFiltering => _anisotropicFiltering;
        public bool EnableShadows => _enableShadows;
        public float ShadowDistance => _shadowDistance;
        public int ShadowCascadeCount => _shadowCascadeCount;
        public int ShadowResolution => _shadowResolution;
        public float VegetationDensity => _vegetationDensity;
        public float ParticleBudgetMultiplier => _particleBudgetMultiplier;
        public float LodBias => _lodBias;
        public bool EnablePostProcessing => _enablePostProcessing;
        public bool EnableBloom => _enableBloom;
        public bool EnableAmbientOcclusion => _enableAmbientOcclusion;
        public int TargetFrameRate => _targetFrameRate;

        public QualityPresetData(
            QualityLevelTier tier,
            string presetName,
            float renderScale,
            int msaaSamples,
            AnisotropicFiltering anisotropicFiltering,
            bool enableShadows,
            float shadowDistance,
            int shadowCascadeCount,
            int shadowResolution,
            float vegetationDensity,
            float particleBudgetMultiplier,
            float lodBias,
            bool enablePostProcessing,
            bool enableBloom,
            bool enableAmbientOcclusion,
            int targetFrameRate)
        {
            _tier = tier;
            _presetName = presetName;
            _renderScale = renderScale;
            _msaaSamples = msaaSamples;
            _anisotropicFiltering = anisotropicFiltering;
            _enableShadows = enableShadows;
            _shadowDistance = shadowDistance;
            _shadowCascadeCount = shadowCascadeCount;
            _shadowResolution = shadowResolution;
            _vegetationDensity = vegetationDensity;
            _particleBudgetMultiplier = particleBudgetMultiplier;
            _lodBias = lodBias;
            _enablePostProcessing = enablePostProcessing;
            _enableBloom = enableBloom;
            _enableAmbientOcclusion = enableAmbientOcclusion;
            _targetFrameRate = targetFrameRate;
        }
    }
}
