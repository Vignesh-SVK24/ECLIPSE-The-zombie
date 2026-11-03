using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Eclipse.Environment
{
    public enum AtmospherePreset
    {
        OvercastMorning,
        CloudyAfternoon,
        GoldenHourSunset,
        DarkStormyNight
    }

    /// <summary>
    /// Controls environmental atmosphere, cinematic sunlight, volumetric fog,
    /// and localized flickering streetlights across Greyfall County.
    /// Defaulted to Stylized Golden Hour Sunset matching the master visual key art.
    /// </summary>
    [ExecuteInEditMode]
    public class EnvironmentAtmosphereController : MonoBehaviour
    {
        [Header("Active Atmosphere Preset")]
        [SerializeField] private AtmospherePreset _currentPreset = AtmospherePreset.GoldenHourSunset;

        [Header("Lighting References")]
        [SerializeField] private Light _sunDirectionalLight;
        [SerializeField] private Volume _postProcessingVolume;

        [Header("Streetlights Management")]
        [SerializeField] private Light[] _municipalStreetlights;
        [SerializeField] private bool _enableFlickeringStreetlights = true;

        private float _flickerTimer;

        private void Start()
        {
            ApplyAtmospherePreset(_currentPreset);
        }

        private void Update()
        {
            if (Application.isPlaying && _enableFlickeringStreetlights)
            {
                UpdateFlickeringLights();
            }
        }

        public void ApplyAtmospherePreset(AtmospherePreset preset)
        {
            _currentPreset = preset;

            switch (preset)
            {
                case AtmospherePreset.GoldenHourSunset:
                    // Master Stylized Survival Art Direction (Golden orange sunlight, dusty violet sky fill, amber haze)
                    SetLighting(
                        sunColor: new Color(1.0f, 0.64f, 0.28f, 1.0f),
                        sunIntensity: 1.65f,
                        ambientColor: new Color(0.28f, 0.20f, 0.32f, 1.0f),
                        fogColor: new Color(0.42f, 0.24f, 0.16f, 1.0f),
                        fogDensity: 0.016f,
                        sunPitch: 16.0f,
                        sunYaw: 215.0f
                    );
                    break;
                case AtmospherePreset.OvercastMorning:
                    SetLighting(new Color(0.85f, 0.88f, 0.90f), 1.1f, new Color(0.20f, 0.22f, 0.24f), new Color(0.18f, 0.22f, 0.24f), 0.015f, 45f, 120f);
                    break;
                case AtmospherePreset.CloudyAfternoon:
                    SetLighting(new Color(0.92f, 0.90f, 0.85f), 1.3f, new Color(0.22f, 0.24f, 0.26f), new Color(0.16f, 0.18f, 0.19f), 0.012f, 60f, 150f);
                    break;
                case AtmospherePreset.DarkStormyNight:
                    SetLighting(new Color(0.25f, 0.32f, 0.40f), 0.25f, new Color(0.06f, 0.08f, 0.12f), new Color(0.04f, 0.06f, 0.08f), 0.035f, 15f, 45f);
                    break;
            }
        }

        private void SetLighting(Color sunColor, float sunIntensity, Color ambientColor, Color fogColor, float fogDensity, float sunPitch, float sunYaw)
        {
            if (_sunDirectionalLight != null)
            {
                _sunDirectionalLight.color = sunColor;
                _sunDirectionalLight.intensity = sunIntensity;
                _sunDirectionalLight.transform.rotation = Quaternion.Euler(sunPitch, sunYaw, 0f);
            }

            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = ambientColor;

            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
        }

        private void UpdateFlickeringLights()
        {
            _flickerTimer += Time.deltaTime;
            if (_flickerTimer > 0.08f)
            {
                _flickerTimer = 0;
                if (_municipalStreetlights != null)
                {
                    foreach (var light in _municipalStreetlights)
                    {
                        if (light != null && UnityEngine.Random.value < 0.05f)
                        {
                            light.intensity = (UnityEngine.Random.value < 0.4f) ? 0.2f : 1.8f;
                        }
                    }
                }
            }
        }
    }
}
