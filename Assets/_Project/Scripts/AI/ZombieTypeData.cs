using UnityEngine;

namespace Eclipse.AI
{
    [CreateAssetMenu(fileName = "ZombieTypeData", menuName = "ECLIPSE/AI/Zombie Type Data")]
    public class ZombieTypeData : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private ZombieType _type;
        [SerializeField] private string _typeName;

        [Header("Health & Defense")]
        [SerializeField] private float _maxHealth = 100f;
        [Range(0f, 0.9f)]
        [SerializeField] private float _damageResistance = 0f;

        [Header("Locomotion Speeds")]
        [SerializeField] private float _walkSpeed = 1.4f;
        [SerializeField] private float _chaseSpeed = 3.8f;
        [SerializeField] private float _angularSpeed = 240f;
        [SerializeField] private float _acceleration = 8f;

        [Header("Perception & Hearing")]
        [SerializeField] private float _visionRange = 18f;
        [SerializeField] private float _fieldOfView = 110f;
        [SerializeField] private float _hearingSensitivity = 1.0f;
        [SerializeField] private float _investigateDuration = 6.0f;

        [Header("Combat & Attack")]
        [SerializeField] private float _attackDamage = 15f;
        [SerializeField] private float _attackRange = 1.6f;
        [SerializeField] private float _attackCooldown = 1.5f;
        [SerializeField] private float _attackWindup = 0.4f;
        [SerializeField] private bool _canDamageBarricades = false;
        [SerializeField] private float _barricadeDamageMultiplier = 1.0f;

        [Header("Special Attributes")]
        [SerializeField] private bool _hasMultiPhase = false;
        [SerializeField] private float _phaseTwoHealthThreshold = 0.5f;
        [SerializeField] private float _retreatHealthThreshold = 0.25f;
        [SerializeField] private bool _canCallHorde = false;
        [SerializeField] private float _hordeCallRadius = 25f;

        public ZombieType Type => _type;
        public string TypeName => _typeName;
        public float MaxHealth => _maxHealth;
        public float DamageResistance => _damageResistance;
        public float WalkSpeed => _walkSpeed;
        public float ChaseSpeed => _chaseSpeed;
        public float AngularSpeed => _angularSpeed;
        public float Acceleration => _acceleration;
        public float VisionRange => _visionRange;
        public float FieldOfView => _fieldOfView;
        public float HearingSensitivity => _hearingSensitivity;
        public float InvestigateDuration => _investigateDuration;
        public float AttackDamage => _attackDamage;
        public float AttackRange => _attackRange;
        public float AttackCooldown => _attackCooldown;
        public float AttackWindup => _attackWindup;
        public bool CanDamageBarricades => _canDamageBarricades;
        public float BarricadeDamageMultiplier => _barricadeDamageMultiplier;
        public bool HasMultiPhase => _hasMultiPhase;
        public float PhaseTwoHealthThreshold => _phaseTwoHealthThreshold;
        public float RetreatHealthThreshold => _retreatHealthThreshold;
        public bool CanCallHorde => _canCallHorde;
        public float HordeCallRadius => _hordeCallRadius;
    }
}
