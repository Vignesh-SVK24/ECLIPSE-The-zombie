using UnityEngine;

namespace Eclipse.AI.States
{
    public class ZombieAttackState : IZombieState
    {
        private readonly ZombieController _controller;
        private float _windupTimer;
        private bool _attackExecuted;

        public ZombieStateId StateId => ZombieStateId.Attack;

        public ZombieAttackState(ZombieController controller)
        {
            _controller = controller;
        }

        public void Enter()
        {
            _controller.Movement.Stop();
            _windupTimer = 0f;
            _attackExecuted = false;

            if (_controller.TargetPlayer != null)
            {
                _controller.Movement.RotateTowards(_controller.TargetPlayer.position);
            }
        }

        public void Update()
        {
            if (_controller.TargetPlayer == null)
            {
                _controller.StateMachine.ChangeState(ZombieStateId.Investigate);
                return;
            }

            _controller.Movement.RotateTowards(_controller.TargetPlayer.position);

            float windup = _controller.TypeData != null ? _controller.TypeData.AttackWindup : 0.4f;
            _windupTimer += Time.deltaTime;

            if (!_attackExecuted && _windupTimer >= windup)
            {
                _attackExecuted = true;
                _controller.ExecuteAttack();
            }

            // Once attack animation completes and cooldown is ready, return to chase or repeat
            if (_attackExecuted && _controller.CanAttack())
            {
                float range = _controller.TypeData != null ? _controller.TypeData.AttackRange : 1.6f;
                float distance = Vector3.Distance(_controller.transform.position, _controller.TargetPlayer.position);

                if (distance <= range)
                {
                    // Repeat attack
                    Enter();
                }
                else
                {
                    _controller.StateMachine.ChangeState(ZombieStateId.Chase);
                }
            }
        }

        public void PhysicsUpdate() { }

        public void Exit() { }
    }
}
