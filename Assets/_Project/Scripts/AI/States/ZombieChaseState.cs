using UnityEngine;

namespace Eclipse.AI.States
{
    public class ZombieChaseState : IZombieState
    {
        private readonly ZombieController _controller;
        private float _repathTimer;
        private const float RepathInterval = 0.25f;

        public ZombieStateId StateId => ZombieStateId.Chase;

        public ZombieChaseState(ZombieController controller)
        {
            _controller = controller;
        }

        public void Enter()
        {
            float chaseSpeed = _controller.TypeData != null ? _controller.TypeData.ChaseSpeed : 3.8f;
            if (_controller.IsEnraged) chaseSpeed *= 1.3f;

            _controller.Movement.SetSpeed(chaseSpeed);
            _controller.Movement.Resume();
            _repathTimer = 0f;
        }

        public void Update()
        {
            if (_controller.TargetPlayer == null)
            {
                _controller.StateMachine.ChangeState(ZombieStateId.Investigate);
                return;
            }

            float attackRange = _controller.TypeData != null ? _controller.TypeData.AttackRange : 1.6f;
            float distanceToPlayer = Vector3.Distance(_controller.transform.position, _controller.TargetPlayer.position);

            if (distanceToPlayer <= attackRange)
            {
                _controller.StateMachine.ChangeState(ZombieStateId.Attack);
                return;
            }

            _repathTimer += Time.deltaTime;
            if (_repathTimer >= RepathInterval)
            {
                _repathTimer = 0f;
                _controller.Movement.SetDestination(_controller.TargetPlayer.position);
            }
        }

        public void PhysicsUpdate() { }

        public void Exit()
        {
            _controller.Movement.Stop();
        }
    }
}
