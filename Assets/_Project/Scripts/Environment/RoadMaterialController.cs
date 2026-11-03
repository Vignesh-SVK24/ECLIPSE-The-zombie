using System;
using UnityEngine;

namespace Eclipse.Environment
{
    /// <summary>
    /// Controls layered PBR road materials in Greyfall County.
    /// Regulates asphalt macro-aggregate, crack intensity, faded road markings,
    /// and dynamic rain wetness / puddle specular reflections.
    /// </summary>
    [ExecuteInEditMode]
    public class RoadMaterialController : MonoBehaviour
    {
        [Header("Material References")]
        [SerializeField] private Material _roadMaterial;
        [SerializeField] private Renderer _roadRenderer;

        [Header("Asphalt & Crack Parameters")]
        [Range(0f, 1f)]
        [SerializeField] private float _crackIntensity = 0.65f;
        [Range(0f, 1f)]
        [SerializeField] private float _potholeDepth = 0.45f;
        [Range(0f, 1f)]
        [SerializeField] private float _markingFade = 0.55f;

        [Header("Wetness & Weather Response")]
        [Range(0f, 1f)]
        [SerializeField] private float _wetnessAmount = 0.35f;
        [Range(0f, 1f)]
        [SerializeField] private float _puddleThreshold = 0.60f;
        [SerializeField] private Color _asphaltBaseTint = new Color(0.12f, 0.13f, 0.12f, 1.0f);
        [SerializeField] private Color _markingFadedYellow = new Color(0.72f, 0.58f, 0.18f, 1.0f);

        private static readonly int PropCrackIntensity = Shader.PropertyToID("_CrackIntensity");
        private static readonly int PropMarkingFade = Shader.PropertyToID("_MarkingFade");
        private static readonly int PropWetness = Shader.PropertyToID("_Wetness");
        private static readonly int PropPuddleThreshold = Shader.PropertyToID("_PuddleThreshold");
        private static readonly int PropBaseColor = Shader.PropertyToID("_BaseColor");

        private MaterialPropertyBlock _propBlock;

        private void Awake()
        {
            if (_roadRenderer == null) _roadRenderer = GetComponent<Renderer>();
            _propBlock = new MaterialPropertyBlock();
        }

        private void Start()
        {
            ApplyProperties();
        }

        private void OnValidate()
        {
            ApplyProperties();
        }

        public void SetWetness(float wetness)
        {
            _wetnessAmount = Mathf.Clamp01(wetness);
            ApplyProperties();
        }

        public void ApplyProperties()
        {
            if (_roadRenderer == null) return;
            if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

            _roadRenderer.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat(PropCrackIntensity, _crackIntensity);
            _propBlock.SetFloat(PropMarkingFade, _markingFade);
            _propBlock.SetFloat(PropWetness, _wetnessAmount);
            _propBlock.SetFloat(PropPuddleThreshold, _puddleThreshold);
            _propBlock.SetColor(PropBaseColor, _asphaltBaseTint);
            _roadRenderer.SetPropertyBlock(_propBlock);
        }
    }
}
