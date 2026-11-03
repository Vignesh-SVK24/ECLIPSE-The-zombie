using System;
using UnityEngine;

namespace Eclipse.Environment
{
    public enum BuildingStyle
    {
        ResidentialApartment,
        CommercialShopfront,
        MedicalClinic,
        GasStationService,
        IndustrialWarehouse
    }

    public enum BuildingDamageState
    {
        WeatheredOnly,
        BrokenWindows,
        PartiallyCollapsed,
        FireDamaged,
        Overgrown
    }

    /// <summary>
    /// Architectural building component supporting modular variations,
    /// damage states, fire escapes, rooftop utilities, and automatic LOD setup.
    /// </summary>
    [SelectionBase]
    public class ModularBuildingPiece : MonoBehaviour
    {
        [Header("Building Metadata")]
        [SerializeField] private BuildingStyle _style = BuildingStyle.ResidentialApartment;
        [SerializeField] private BuildingDamageState _damageState = BuildingDamageState.BrokenWindows;
        [SerializeField] private int _storyCount = 4;

        [Header("Modular Attachments")]
        [SerializeField] private GameObject _fireEscapeLadder;
        [SerializeField] private GameObject _rooftopACUnit;
        [SerializeField] private GameObject _rooftopWaterTower;
        [SerializeField] private GameObject _ivyOvergrowth;
        [SerializeField] private GameObject _awningShopfront;

        [Header("LOD & Collision")]
        [SerializeField] private LODGroup _lodGroup;
        [SerializeField] private Collider _structuralCollider;

        public BuildingStyle Style => _style;
        public BuildingDamageState DamageState => _damageState;
        public int StoryCount => _storyCount;

        private void Awake()
        {
            if (_structuralCollider == null)
            {
                _structuralCollider = GetComponent<Collider>();
            }
            if (_lodGroup == null)
            {
                _lodGroup = GetComponent<LODGroup>();
            }
        }

        public void ConfigureBuilding(BuildingStyle style, BuildingDamageState damageState, int stories)
        {
            _style = style;
            _damageState = damageState;
            _storyCount = Mathf.Clamp(stories, 1, 10);

            if (_fireEscapeLadder != null)
            {
                _fireEscapeLadder.SetActive(_style == BuildingStyle.ResidentialApartment && _storyCount >= 2);
            }

            if (_rooftopWaterTower != null)
            {
                _rooftopWaterTower.SetActive(_style == BuildingStyle.ResidentialApartment && _storyCount >= 3);
            }

            if (_rooftopACUnit != null)
            {
                _rooftopACUnit.SetActive(_style == BuildingStyle.CommercialShopfront || _style == BuildingStyle.MedicalClinic);
            }

            if (_ivyOvergrowth != null)
            {
                _ivyOvergrowth.SetActive(_damageState == BuildingDamageState.Overgrown || _damageState == BuildingDamageState.PartiallyCollapsed);
            }
        }
    }
}
