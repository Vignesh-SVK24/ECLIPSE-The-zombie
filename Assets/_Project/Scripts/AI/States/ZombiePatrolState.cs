using UnityEngine;

namespace Eclipse.AI.States
{
    public class ZombiePatrolState : IZombieState
    {
        private readonly ZombieController _controller;
        private int _currentWaypointIndex = 0;
        private Vector3 _patrolTarget;

        public ZombieStateId StateId => ZombieStateId.Patrol;

        public ZombiePatrolState(ZombieController controller)
        {
            _controller = controller;
        }

        public void Enter()
        {
            float walkSpeed = _controller.TypeData != null ? _controller.TypeData.WalkSpeed : 1.4f;
            _controller.Movement.SetSpeed(walkSpeed);
            _controller.Movement.Resume();

            SelectNextPatrolPoint();
        }

        public void Update()
        {
            if (_controller.Movement.HasReachedDestination() || _controller.Movement.IsStuck)
            {
                _controller.StateMachine.ChangeState(ZombieStateId.Idle);
            }
        }

        public void PhysicsUpdate() { }

        public void Exit()
        {
            _controller.Movement.Stop();
        }

        private void SelectNextPatrolPoint()
        {
            if (_controller.PatrolWaypoints != null && _controller.PatrolWaypoints.Length > 0)
            {
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _controller.PatrolWaypoints.Length;
                _patrolTarget = _controller.PatrolWaypoints[_currentWaypointIndex].position;
            }
            else
            {
                // Wander within patrol radius
                Vector2 randomCircle = Random.insideUnitCircle * _controller.PatrolRadius;
                _patrolTarget = _controller.transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
            }

            _controller.Movement.SetDestination(_patrolTarget);
        }
    }
}
