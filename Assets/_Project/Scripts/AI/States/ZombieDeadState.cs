using UnityEngine;

namespace Eclipse.AI.States
{
    public class ZombieDeadState : IZombieState
    {
        private readonly ZombieController _controller;

        public ZombieStateId StateId => ZombieStateId.Dead;

        public ZombieDeadState(ZombieController controller)
        {
            _controller = controller;
        }

        public void Enter()
        {
            _controller.Movement.Stop();

            if (_controller.Movement.Agent != null)
            {
                _controller.Movement.Agent.enabled = false;
            }

            if (_controller.AnimationController != null)
            {
                _controller.AnimationController.TriggerDie();
            }

            // Disable detection and hearing
            if (_controller.Detection != null) _controller.Detection.enabled = false;
            if (_controller.Hearing != null) _controller.Hearing.enabled = false;

            // Disable main colliders to prevent blocking player movement
            var colliders = _controller.GetComponentsInChildren<Collider>();
            foreach (var col in colliders)
            {
                col.enabled = false;
            }

            // Schedule corpse despawn after 10 seconds for mobile memory pooling
            GameObject.Destroy(_controller.gameObject, 10.0f);
        }

        public void Update() { }

        public void PhysicsUpdate() { }

        public void Exit() { }
    }
}
