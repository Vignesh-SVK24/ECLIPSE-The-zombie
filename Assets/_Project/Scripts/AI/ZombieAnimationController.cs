using UnityEngine;

namespace Eclipse.AI
{
    public class ZombieAnimationController : MonoBehaviour
    {
        [Header("Animator Binding")]
        [SerializeField] private Animator _animator;
        [SerializeField] private float _speedDampTime = 0.15f;

        // Cached Parameter Hashes
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int HitHash = Animator.StringToHash("Hit");
        private static readonly int StunnedHash = Animator.StringToHash("Stunned");
        private static readonly int DieHash = Animator.StringToHash("Die");
        private static readonly int SpecialHash = Animator.StringToHash("Special");

        public Animator Animator => _animator;

        private void Awake()
        {
            if (_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }
        }

        public void SetSpeed(float speed)
        {
            if (_animator == null || !_animator.isActiveAndEnabled) return;
            _animator.SetFloat(SpeedHash, speed, _speedDampTime, Time.deltaTime);
        }

        public void TriggerAttack()
        {
            if (_animator == null || !_animator.isActiveAndEnabled) return;
            _animator.SetTrigger(AttackHash);
        }

        public void TriggerHit()
        {
            if (_animator == null || !_animator.isActiveAndEnabled) return;
            _animator.SetTrigger(HitHash);
        }

        public void SetStunned(bool isStunned)
        {
            if (_animator == null || !_animator.isActiveAndEnabled) return;
            _animator.SetBool(StunnedHash, isStunned);
        }

        public void TriggerDie()
        {
            if (_animator == null || !_animator.isActiveAndEnabled) return;
            _animator.SetTrigger(DieHash);
        }

        public void TriggerSpecial()
        {
            if (_animator == null || !_animator.isActiveAndEnabled) return;
            _animator.SetTrigger(SpecialHash);
        }
    }
}
