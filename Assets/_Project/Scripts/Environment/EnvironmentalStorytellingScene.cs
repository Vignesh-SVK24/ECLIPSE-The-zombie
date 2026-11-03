using System;
using UnityEngine;

namespace Eclipse.Environment
{
    public enum StorySceneType
    {
        AbandonedIntersection,
        HospitalEvacuation,
        MilitaryCheckpoint,
        PetrolStation,
        ResidentialAftermath,
        IndustrialCorridor
    }

    /// <summary>
    /// Organizes and coordinates handcrafted environmental storytelling vignettes in Greyfall County.
    /// Manages non-graphic human aftermath props, luggage, warning signs, and atmospheric details.
    /// </summary>
    public class EnvironmentalStorytellingScene : MonoBehaviour
    {
        [Header("Scene Archetype")]
        [SerializeField] private StorySceneType _sceneType = StorySceneType.AbandonedIntersection;
        [SerializeField] private string _sceneName = "Intersection Gridlock";

        [Header("Story Props Collections")]
        [SerializeField] private GameObject[] _abandonedVehicles;
        [SerializeField] private GameObject[] _barricadesAndObstacles;
        [SerializeField] private GameObject[] _scatteredBelongings;
        [SerializeField] private GameObject[] _hazardSigns;

        [Header("Atmospheric Sound Zone")]
        [SerializeField] private AudioSource _ambientAudioSource;
        [SerializeField] private AudioClip _localizedAmbienceClip;
        [SerializeField] private float _audioTriggerRadius = 25f;

        public StorySceneType SceneType => _sceneType;
        public string SceneName => _sceneName;

        private void Start()
        {
            if (_ambientAudioSource != null && _localizedAmbienceClip != null)
            {
                _ambientAudioSource.clip = _localizedAmbienceClip;
                _ambientAudioSource.spatialBlend = 1.0f; // 3D spatialized
                _ambientAudioSource.maxDistance = _audioTriggerRadius;
                _ambientAudioSource.rolloffMode = AudioRolloffMode.Linear;
                _ambientAudioSource.loop = true;
                _ambientAudioSource.Play();
            }
        }
    }
}
