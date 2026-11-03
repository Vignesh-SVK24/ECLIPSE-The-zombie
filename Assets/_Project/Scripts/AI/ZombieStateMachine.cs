using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eclipse.AI
{
    public class ZombieStateMachine
    {
        private readonly Dictionary<ZombieStateId, IZombieState> _states = new Dictionary<ZombieStateId, IZombieState>();
        private IZombieState _currentState;
        private ZombieStateId _previousStateId = ZombieStateId.None;

        public IZombieState CurrentState => _currentState;
        public ZombieStateId CurrentStateId => _currentState?.StateId ?? ZombieStateId.None;
        public ZombieStateId PreviousStateId => _previousStateId;

        public event Action<ZombieStateId, ZombieStateId> OnStateChanged;

        public void RegisterState(IZombieState state)
        {
            if (state == null) return;
            _states[state.StateId] = state;
        }

        public void ChangeState(ZombieStateId nextStateId)
        {
            // Once dead, cannot transition to any other state
            if (_currentState != null && _currentState.StateId == ZombieStateId.Dead)
            {
                return;
            }

            if (_currentState != null && _currentState.StateId == nextStateId)
            {
                return;
            }

            if (!_states.TryGetValue(nextStateId, out var nextState))
            {
                Debug.LogWarning($"[ECLIPSE ZombieStateMachine] State {nextStateId} is not registered.");
                return;
            }

            ZombieStateId oldId = CurrentStateId;
            _currentState?.Exit();
            _previousStateId = oldId;
            _currentState = nextState;
            _currentState.Enter();

            OnStateChanged?.Invoke(oldId, nextStateId);
        }

        public void Update()
        {
            _currentState?.Update();
        }

        public void PhysicsUpdate()
        {
            _currentState?.PhysicsUpdate();
        }
    }
}
