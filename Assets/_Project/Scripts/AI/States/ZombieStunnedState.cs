using UnityEngine;

namespace Eclipse.AI.States
{
    public class ZombieStunnedState : IZombieState
    {
        private readonly ZombieController _controller;
        private float _stunDuration = 1.0f;
        private float _timer;

        public ZombieStateId StateId => ZombieStateId.Stunned;

        public ZombieStunnedState(ZombieController controller)
        {
            _controller = controller;
        }

        public void Enter()
        {
            _controller.Movement.Stop();
            _timer = 0f;
            if (_controller.AnimationController != null)
            {
                _controller.AnimationController.SetStunned(true);
            }
        }

        public void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _stunDuration)
            {
                if (_controller.TargetPlayer != null)
                {
                    _controller.StateMachine.ChangeState(ZombieStateId.Chase);
                }
                else
                {
                    _controller.StateMachine.ChangeState(ZombieStateId.Investigate);
                }
            }
        }

        public void PhysicsUpdate() { }

        public void Exit()
        {
            if (_controller.AnimationController != null)
            {
                _controller.AnimationController.SetStunned(false);
            }
        }
    }
}
