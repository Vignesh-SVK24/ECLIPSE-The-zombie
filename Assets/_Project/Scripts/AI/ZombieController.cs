using System;
using UnityEngine;
using Eclipse.Damage;

namespace Eclipse.AI
{
    [RequireComponent(typeof(ZombieMovement))]
    [RequireComponent(typeof(HealthComponent))]
    public class ZombieController : MonoBehaviour
    {
        [Header("Data Configuration")]
        [SerializeField] private ZombieTypeData _typeData;

        [Header("Core Subsystems")]
        [SerializeField] private ZombieMovement _movement;
        [SerializeField] private ZombieDetection _detection;
        [SerializeField] private ZombieHearing _hearing;
        [SerializeField] private ZombieAnimationController _animationController;
        [SerializeField] private HealthComponent _health;

        [Header("Patrol Area")]
        [SerializeField] private Transform[] _patrolWaypoints;
        [SerializeField] private float _patrolRadius = 15f;

        private ZombieStateMachine _stateMachine;
        private Transform _targetPlayer;
        private Vector3 _lastKnownPosition;
        private float _lastAttackTime = -999f;
        private bool _isEnraged = false; // Phase two boss mechanic

        public ZombieTypeData TypeData => _typeData;
        public ZombieMovement Movement => _movement;
        public ZombieDetection Detection => _detection;
        public ZombieHearing Hearing => _hearing;
        public ZombieAnimationController AnimationController => _animationController;
        public HealthComponent Health => _health;
        public ZombieStateMachine StateMachine => _stateMachine;
        public Transform TargetPlayer => _targetPlayer;
        public Vector3 LastKnownPosition => _lastKnownPosition;
        public Transform[] PatrolWaypoints => _patrolWaypoints;
        public float PatrolRadius => _patrolRadius;
        public bool IsEnraged => _isEnraged;

        public event Action<ZombieController> OnZombieDeath;

        private void Awake()
        {
            InitializeComponents();
            InitializeStateMachine();
        }

        private void InitializeComponents()
        {
            if (_movement == null) _movement = GetComponent<ZombieMovement>();
            if (_health == null) _health = GetComponent<HealthComponent>();
            if (_detection == null) _detection = GetComponent<ZombieDetection>();
            if (_hearing == null) _hearing = GetComponent<ZombieHearing>();
            if (_animationController == null) _animationController = GetComponent<ZombieAnimationController>();

            if (_typeData != null)
            {
                ApplyTypeConfiguration();
            }
        }

        public void ApplyTypeConfiguration()
        {
            if (_typeData == null) return;

            if (_health != null)
            {
                _health.SetMaxHealth(_typeData.MaxHealth, true);
            }

            if (_movement != null && _movement.Agent != null)
            {
                _movement.SetSpeed(_typeData.WalkSpeed);
                _movement.Agent.angularSpeed = _typeData.AngularSpeed;
                _movement.Agent.acceleration = _typeData.Acceleration;
                _movement.SetStoppingDistance(_typeData.AttackRange * 0.85f);
            }

            if (_detection != null)
            {
                _detection.VisionRange = _typeData.VisionRange;
                _detection.FieldOfView = _typeData.FieldOfView;
            }

            if (_hearing != null)
            {
                _hearing.Sensitivity = _typeData.HearingSensitivity;
            }
        }

        private void InitializeStateMachine()
        {
            _stateMachine = new ZombieStateMachine();

            _stateMachine.RegisterState(new States.ZombieIdleState(this));
            _stateMachine.RegisterState(new States.ZombiePatrolState(this));
            _stateMachine.RegisterState(new States.ZombieInvestigateState(this));
            _stateMachine.RegisterState(new States.ZombieChaseState(this));
            _stateMachine.RegisterState(new States.ZombieAttackState(this));
            _stateMachine.RegisterState(new States.ZombieStunnedState(this));
            _stateMachine.RegisterState(new States.ZombieDeadState(this));

            _stateMachine.ChangeState(ZombieStateId.Idle);
        }

        private void OnEnable()
        {
            if (_health != null)
            {
                _health.OnDamageTaken += HandleDamageTaken;
                _health.OnDied += HandleDied;
            }

            if (_detection != null)
            {
                _detection.OnTargetSpotted += HandleTargetSpotted;
                _detection.OnTargetLost += HandleTargetLost;
            }

            if (_hearing != null)
            {
                _hearing.OnSoundHeard += HandleSoundHeard;
            }
        }

        private void OnDisable()
        {
            if (_health != null)
            {
                _health.OnDamageTaken -= HandleDamageTaken;
                _health.OnDied -= HandleDied;
            }

            if (_detection != null)
            {
                _detection.OnTargetSpotted -= HandleTargetSpotted;
                _detection.OnTargetLost -= HandleTargetLost;
            }

            if (_hearing != null)
            {
                _hearing.OnSoundHeard -= HandleSoundHeard;
            }
        }

        private void Update()
        {
            _stateMachine?.Update();

            if (_animationController != null && _movement != null)
            {
                _animationController.SetSpeed(_movement.CurrentSpeed);
            }
        }

        private void FixedUpdate()
        {
            _stateMachine?.PhysicsUpdate();
        }

        public void SetTarget(Transform target)
        {
            _targetPlayer = target;
            if (target != null)
            {
                _lastKnownPosition = target.position;
            }
        }

        public void SetInvestigationPoint(Vector3 point)
        {
            _lastKnownPosition = point;
            if (_stateMachine.CurrentStateId != ZombieStateId.Chase && _stateMachine.CurrentStateId != ZombieStateId.Attack)
            {
                _stateMachine.ChangeState(ZombieStateId.Investigate);
            }
        }

        public bool CanAttack()
        {
            if (_typeData == null) return Time.time - _lastAttackTime >= 1.5f;
            return Time.time - _lastAttackTime >= _typeData.AttackCooldown;
        }

        public void ExecuteAttack()
        {
            _lastAttackTime = Time.time;
            if (_animationController != null)
            {
                _animationController.TriggerAttack();
            }

            if (_targetPlayer == null) return;

            // Attack damage delivery
            float range = _typeData != null ? _typeData.AttackRange : 1.6f;
            float damage = _typeData != null ? _typeData.AttackDamage : 15f;

            if (_isEnraged)
            {
                damage *= 1.35f;
            }

            if (Vector3.Distance(transform.position, _targetPlayer.position) <= range + 0.5f)
            {
                Vector3 hitPoint = _targetPlayer.position + Vector3.up * 1.0f;
                Vector3 hitDir = (_targetPlayer.position - transform.position).normalized;

                DamageInfo biteInfo = new DamageInfo(
                    amount: damage,
                    type: DamageType.Infection,
                    source: gameObject,
                    hitPoint: hitPoint,
                    hitNormal: -hitDir,
                    hitDirection: hitDir,
                    location: HitLocation.Torso);

                DamageSystem.ApplyDamage(_targetPlayer.gameObject, biteInfo);
            }
        }

        private void HandleTargetSpotted(Transform target)
        {
            SetTarget(target);
            if (_stateMachine.CurrentStateId != ZombieStateId.Dead && _stateMachine.CurrentStateId != ZombieStateId.Stunned)
            {
                _stateMachine.ChangeState(ZombieStateId.Chase);
            }
        }

        private void HandleTargetLost()
        {
            if (_targetPlayer != null)
            {
                _lastKnownPosition = _targetPlayer.position;
            }

            if (_stateMachine.CurrentStateId == ZombieStateId.Chase)
            {
                _stateMachine.ChangeState(ZombieStateId.Investigate);
            }
        }

        private void HandleSoundHeard(Vector3 soundPos, SoundCategory category)
        {
            if (_stateMachine.CurrentStateId == ZombieStateId.Chase || _stateMachine.CurrentStateId == ZombieStateId.Attack)
            {
                return; // Already actively engaging
            }

            SetInvestigationPoint(soundPos);
        }

        private void HandleDamageTaken(DamageInfo info)
        {
            // If attacked from behind or from stealth, investigate damage source position
            if (_targetPlayer == null && info.Source != null)
            {
                SetTarget(info.Source.transform);
                _stateMachine.ChangeState(ZombieStateId.Chase);
            }

            // Alpha boss phase two transition
            if (_typeData != null && _typeData.HasMultiPhase && !_isEnraged)
            {
                float healthPercent = _health.CurrentHealth / _health.MaxHealth;
                if (healthPercent <= _typeData.PhaseTwoHealthThreshold)
                {
                    _isEnraged = true;
                    if (_animationController != null)
                    {
                        _animationController.TriggerSpecial();
                    }
                    if (_movement != null)
                    {
                        _movement.SetSpeed(_typeData.ChaseSpeed * 1.3f);
                    }
                }
            }

            // Stalker retreat check
            if (_typeData != null && _typeData.Type == ZombieType.Stalker)
            {
                float healthPercent = _health.CurrentHealth / _health.MaxHealth;
                if (healthPercent <= _typeData.RetreatHealthThreshold)
                {
                    _stateMachine.ChangeState(ZombieStateId.Retreating);
                    return;
                }
            }

            // Stun on heavy impact
            if (info.StunDuration > 0.2f && _stateMachine.CurrentStateId != ZombieStateId.Dead)
            {
                _stateMachine.ChangeState(ZombieStateId.Stunned);
            }
            else if (_animationController != null)
            {
                _animationController.TriggerHit();
            }
        }

        private void HandleDied(DamageInfo fatalHit)
        {
            _stateMachine.ChangeState(ZombieStateId.Dead);
            OnZombieDeath?.Invoke(this);
        }
    }
}
