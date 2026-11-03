using System.Collections.Generic;
using UnityEngine;

namespace Eclipse.AI
{
    public class ZombieGroupManager : MonoBehaviour
    {
        private static ZombieGroupManager _instance;
        public static ZombieGroupManager Instance => _instance;

        private readonly List<ZombieController> _activeZombies = new List<ZombieController>();
        private float _lastGroupAlertTime = -999f;
        private const float AlertCooldown = 2.0f;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
        }

        public void RegisterZombie(ZombieController zombie)
        {
            if (zombie != null && !_activeZombies.Contains(zombie))
            {
                _activeZombies.Add(zombie);
                zombie.OnZombieDeath += HandleZombieDeath;
            }
        }

        public void UnregisterZombie(ZombieController zombie)
        {
            if (zombie != null)
            {
                _activeZombies.Remove(zombie);
                zombie.OnZombieDeath -= HandleZombieDeath;
            }
        }

        private void HandleZombieDeath(ZombieController zombie)
        {
            UnregisterZombie(zombie);
        }

        public void AlertNearbyZombies(Vector3 alertPosition, float radius, Transform target)
        {
            if (Time.time - _lastGroupAlertTime < AlertCooldown) return;
            _lastGroupAlertTime = Time.time;

            float radiusSqr = radius * radius;

            foreach (var zombie in _activeZombies)
            {
                if (zombie == null || zombie.StateMachine.CurrentStateId == ZombieStateId.Dead) continue;

                float sqrDist = (zombie.transform.position - alertPosition).sqrMagnitude;
                if (sqrDist <= radiusSqr)
                {
                    zombie.SetTarget(target);
                    zombie.StateMachine.ChangeState(ZombieStateId.Chase);
                }
            }
        }
    }
}
