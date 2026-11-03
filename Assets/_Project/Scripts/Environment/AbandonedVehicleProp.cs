using System;
using UnityEngine;
using UnityEngine.AI;

namespace Eclipse.Environment
{
    public enum VehicleType
    {
        PoliceCruiser,
        PickupTruck,
        FamilySedan,
        Ambulance,
        Motorcycle
    }

    /// <summary>
    /// Controls abandoned vehicle props in Greyfall County.
    /// Manages collision meshes, visual damage states (flat tires, open doors, shattered glass),
    /// and NavMeshObstacle carving to ensure zombie AI navigates realistically around obstacles.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class AbandonedVehicleProp : MonoBehaviour
    {
        [Header("Vehicle Classification")]
        [SerializeField] private VehicleType _vehicleType = VehicleType.PoliceCruiser;
        [SerializeField] private bool _hasFlatTires = true;
        [SerializeField] private bool _isDriverDoorOpen = true;
        [SerializeField] private bool _hasShatteredGlass = true;
        [SerializeField] private bool _isBurnedOut = false;

        [Header("Interactive Emergency Elements")]
        [SerializeField] private Light _emergencyBeaconLight;
        [SerializeField] private bool _beaconActive = false;
        [SerializeField] private float _beaconPulseSpeed = 6f;

        [Header("AI Navigation")]
        [SerializeField] private NavMeshObstacle _navObstacle;

        public VehicleType Type => _vehicleType;
        public bool IsBurnedOut => _isBurnedOut;

        private void Awake()
        {
            SetupNavMeshObstacle();
            ApplyDamageVisuals();
        }

        private void SetupNavMeshObstacle()
        {
            _navObstacle = GetComponent<NavMeshObstacle>();
            if (_navObstacle == null)
            {
                _navObstacle = gameObject.AddComponent<NavMeshObstacle>();
            }

            var boxCol = GetComponent<BoxCollider>();
            if (boxCol != null)
            {
                _navObstacle.shape = NavMeshObstacleShape.Box;
                _navObstacle.size = boxCol.size;
                _navObstacle.center = boxCol.center;
            }
            _navObstacle.carving = true;
            _navObstacle.carveOnlyStationary = true;
        }

        private void ApplyDamageVisuals()
        {
            if (_hasFlatTires)
            {
                // Drop suspension slightly towards flat wheel side
                transform.localRotation *= Quaternion.Euler(0, 0, 1.8f);
            }
        }

        private void Update()
        {
            if (_beaconActive && _emergencyBeaconLight != null)
            {
                _emergencyBeaconLight.intensity = (Mathf.Sin(Time.time * _beaconPulseSpeed) > 0) ? 2.5f : 0.1f;
            }
        }

        public void SetEmergencyBeacon(bool active)
        {
            _beaconActive = active;
            if (_emergencyBeaconLight != null)
            {
                _emergencyBeaconLight.enabled = active;
            }
        }
    }
}
