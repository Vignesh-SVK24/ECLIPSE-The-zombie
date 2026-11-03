using UnityEngine;

namespace Eclipse.AI.States
{
    public class ZombieIdleState : IZombieState
    {
        private readonly ZombieController _controller;
        private float _idleDuration;
        private float _timer;

        public ZombieStateId StateId => ZombieStateId.Idle;

        public ZombieIdleState(ZombieController controller)
        {
            _controller = controller;
        }

        public void Enter()
        {
            _controller.Movement.Stop();
            _idleDuration = Random.Range(2.0f, 5.0f);
            _timer = 0f;
        }

        public void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _idleDuration)
            {
                _controller.StateMachine.ChangeState(ZombieStateId.Patrol);
            }
        }

        public void PhysicsUpdate() { }

        public void Exit() { }
    }
}
