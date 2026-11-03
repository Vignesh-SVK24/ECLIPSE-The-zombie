using UnityEngine;

namespace Eclipse.AI.States
{
    public class ZombieInvestigateState : IZombieState
    {
        private readonly ZombieController _controller;
        private float _investigateDuration;
        private float _timer;

        public ZombieStateId StateId => ZombieStateId.Investigate;

        public ZombieInvestigateState(ZombieController controller)
        {
            _controller = controller;
        }

        public void Enter()
        {
            float speed = _controller.TypeData != null ? _controller.TypeData.WalkSpeed * 1.25f : 2.0f;
            _controller.Movement.SetSpeed(speed);
            _controller.Movement.Resume();
            _controller.Movement.SetDestination(_controller.LastKnownPosition);

            _investigateDuration = _controller.TypeData != null ? _controller.TypeData.InvestigateDuration : 6.0f;
            _timer = 0f;
        }

        public void Update()
        {
            if (_controller.Movement.HasReachedDestination())
            {
                _timer += Time.deltaTime;
                if (_timer >= _investigateDuration)
                {
                    // Investigation concluded without target spotted: return to idle/patrol
                    _controller.StateMachine.ChangeState(ZombieStateId.Idle);
                }
            }
        }

        public void PhysicsUpdate() { }

        public void Exit()
        {
            _controller.Movement.Stop();
        }
    }
}
