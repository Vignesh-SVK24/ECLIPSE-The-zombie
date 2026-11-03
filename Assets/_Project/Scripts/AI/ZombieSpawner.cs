using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Eclipse.AI
{
    public class ZombieSpawner : MonoBehaviour
    {
        [Header("Spawn Configuration")]
        [SerializeField] private GameObject _zombiePrefab;
        [SerializeField] private ZombieTypeData[] _zombieTypeDatas;
        [SerializeField] private int _maxActiveZombies = 20;
        [SerializeField] private float _spawnInterval = 5.0f;
        [SerializeField] private float _spawnRadiusMin = 15f;
        [SerializeField] private float _spawnRadiusMax = 35f;
        [SerializeField] private Transform _playerReference;

        private readonly List<ZombieController> _spawnedZombies = new List<ZombieController>();
        private float _spawnTimer;

        public int ActiveZombieCount => _spawnedZombies.Count;

        private void Update()
        {
            if (_playerReference == null)
            {
                var playerObj = GameObject.FindWithTag("Player");
                if (playerObj != null) _playerReference = playerObj.transform;
                if (_playerReference == null) return;
            }

            // Cleanup null or dead zombies
            _spawnedZombies.RemoveAll(z => z == null || z.StateMachine.CurrentStateId == ZombieStateId.Dead);

            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _spawnInterval && _spawnedZombies.Count < _maxActiveZombies)
            {
                _spawnTimer = 0f;
                TrySpawnZombie();
            }
        }

        public bool TrySpawnZombie()
        {
            if (_zombiePrefab == null || _playerReference == null) return false;

            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(_spawnRadiusMin, _spawnRadiusMax);
            Vector3 targetPos = _playerReference.position + new Vector3(randomCircle.x, 0f, randomCircle.y);

            if (NavMesh.SamplePosition(targetPos, out var hit, 5.0f, NavMesh.AllAreas))
            {
                GameObject instance = Instantiate(_zombiePrefab, hit.position, Quaternion.identity);
                if (instance.TryGetComponent<ZombieController>(out var controller))
                {
                    // Assign random type data or default
                    if (_zombieTypeDatas != null && _zombieTypeDatas.Length > 0)
                    {
                        int index = UnityEngine.Random.Range(0, _zombieTypeDatas.Length);
                        var serializedField = typeof(ZombieController).GetField("_typeData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (serializedField != null)
                        {
                            serializedField.SetValue(controller, _zombieTypeDatas[index]);
                        }
                        controller.ApplyTypeConfiguration();
                    }

                    _spawnedZombies.Add(controller);

                    if (ZombieGroupManager.Instance != null)
                    {
                        ZombieGroupManager.Instance.RegisterZombie(controller);
                    }

                    return true;
                }
            }

            return false;
        }

        public void DespawnAll()
        {
            foreach (var z in _spawnedZombies)
            {
                if (z != null) Destroy(z.gameObject);
            }
            _spawnedZombies.Clear();
        }
    }
}
