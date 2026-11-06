using System;
using UnityEngine;

namespace Eclipse.AI
{
    public enum WalkerVariantPreset
    {
        Civilian,
        FormerSecurity,
        Military,
        IndustrialWorker,
        Injured
    }

    /// <summary>
    /// Manages stylized infected appearance variations, procedural color tinting,
    /// and class-specific visual signatures (exposed bone fragments, toxic glowing eye)
    /// using MaterialPropertyBlocks to preserve mobile GPU instancing.
    /// </summary>
    [DisallowMultipleComponent]
    public class ZombieAppearanceVariant : MonoBehaviour
    {
        [Header("Class Configuration")]
        [SerializeField] private ZombieType _zombieType = ZombieType.Walker;
        [SerializeField] private WalkerVariantPreset _walkerPreset = WalkerVariantPreset.Civilian;
        [SerializeField] private bool _randomizeOnStart = true;

        [Header("Renderers")]
        [SerializeField] private Renderer _skinRenderer;
        [SerializeField] private Renderer _clothingRenderer;
        [SerializeField] private Renderer _leftEyeRenderer;
        [SerializeField] private Renderer _rightEyeRenderer;
        [SerializeField] private GameObject _exposedBoneObject;

        [Header("Emissive Eye Settings")]
        [SerializeField] private Color _toxicEyeGlowColor = new Color(0.17f, 1.0f, 0.04f, 1.0f); // #2BF805
        [SerializeField] private float _eyeGlowIntensity = 3.5f;
        [SerializeField] private bool _singleEyeGlowOnly = true;

        private MaterialPropertyBlock _propBlock;
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        public WalkerVariantPreset CurrentWalkerPreset => _walkerPreset;

        private void Awake()
        {
            _propBlock = new MaterialPropertyBlock();
        }

        private void Start()
        {
            if (_randomizeOnStart && _zombieType == ZombieType.Walker)
            {
                var values = (WalkerVariantPreset[])Enum.GetValues(typeof(WalkerVariantPreset));
                _walkerPreset = values[UnityEngine.Random.Range(0, values.Length)];
            }

            ApplyAppearance();
        }

        public void SetVariant(WalkerVariantPreset preset)
        {
            _walkerPreset = preset;
            ApplyAppearance();
        }

        public void ApplyAppearance()
        {
            if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

            Color skinColor = new Color(0.35f, 0.42f, 0.33f, 1.0f); // Default decayed gray-green
            Color clothesColor = new Color(0.25f, 0.28f, 0.25f, 1.0f);

            switch (_walkerPreset)
            {
                case WalkerVariantPreset.Civilian:
                    // Faded canvas jacket / denim
                    clothesColor = new Color(0.28f, 0.30f, 0.34f, 1.0f);
                    break;

                case WalkerVariantPreset.FormerSecurity:
                    // Navy blue security shirt with dark trousers
                    clothesColor = new Color(0.18f, 0.22f, 0.30f, 1.0f);
                    break;

                case WalkerVariantPreset.Military:
                    // Desaturated olive drab tactical camo
                    clothesColor = new Color(0.26f, 0.29f, 0.20f, 1.0f);
                    break;

                case WalkerVariantPreset.IndustrialWorker:
                    // Soot-stained orange / yellow-brown workwear
                    clothesColor = new Color(0.45f, 0.32f, 0.18f, 1.0f);
                    break;

                case WalkerVariantPreset.Injured:
                    // Blood-drenched dark ragged shirt with pale infected skin
                    clothesColor = new Color(0.32f, 0.18f, 0.18f, 1.0f);
                    skinColor = new Color(0.38f, 0.44f, 0.36f, 1.0f);
                    break;
            }

            // Apply to Clothing
            if (_clothingRenderer != null)
            {
                _clothingRenderer.GetPropertyBlock(_propBlock);
                _propBlock.SetColor(BaseColorId, clothesColor);
                _clothingRenderer.SetPropertyBlock(_propBlock);
            }

            // Apply to Skin
            if (_skinRenderer != null)
            {
                _skinRenderer.GetPropertyBlock(_propBlock);
                _propBlock.SetColor(BaseColorId, skinColor);
                _skinRenderer.SetPropertyBlock(_propBlock);
            }

            // Apply Emissive Eye Glow
            Color emissiveValue = _toxicEyeGlowColor * _eyeGlowIntensity;

            if (_leftEyeRenderer != null)
            {
                _leftEyeRenderer.GetPropertyBlock(_propBlock);
                _propBlock.SetColor(EmissionColorId, emissiveValue);
                _leftEyeRenderer.SetPropertyBlock(_propBlock);
            }

            if (_rightEyeRenderer != null)
            {
                _rightEyeRenderer.GetPropertyBlock(_propBlock);
                if (_singleEyeGlowOnly)
                {
                    // Clouded cataract eye for Walker asymmetry
                    _propBlock.SetColor(BaseColorId, new Color(0.75f, 0.78f, 0.75f, 1.0f));
                    _propBlock.SetColor(EmissionColorId, Color.black);
                }
                else
                {
                    _propBlock.SetColor(EmissionColorId, emissiveValue);
                }
                _rightEyeRenderer.SetPropertyBlock(_propBlock);
            }

            // Exposed Bone Fragment visibility
            if (_exposedBoneObject != null)
            {
                _exposedBoneObject.SetActive(_walkerPreset == WalkerVariantPreset.Injured || 
                                             _walkerPreset == WalkerVariantPreset.Civilian);
            }
        }
    }
}
