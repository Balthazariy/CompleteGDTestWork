using ModestTree;
using System;
using System.Collections.Generic;
using Test.Enemies;
using Test.Enemies.Base;
using Test.ScriptableObjects;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Zenject;

namespace Test.Managers
{
    public class EnemySpawnerManager : MonoBehaviour
    {
        public event Action OnAllWavesCompletedEvent;

        public event Action<int, int> OnWaveChangeEvent;

        [SerializeField] private LevelConfig _config;
        [SerializeField] private GameObject _duplicatorCloneObject;
        [SerializeField] private int _countOfClone;

        private int _currentWave;

        private List<EnemyBase> _enemies;

        private Player.Player _player;

        [Inject]
        public void Construct(Player.Player player)
        {
            _player = player;

            _enemies = new List<EnemyBase>();

            Spawn();
        }

        private void Update()
        {
            if (_enemies == null)
            {
                return;
            }

            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].Move();
            }
        }

        public List<EnemyBase> GetList() => _enemies;

        private void Spawn()
        {
            if (_currentWave >= _config.Waves.Length)
            {
                OnAllWavesCompletedEvent?.Invoke();
                return;
            }

            var wave = _config.Waves[_currentWave];
            foreach (var character in wave.Characters)
            {
                SpawnEnemy(character);
            }

            _currentWave++;

            OnWaveChangeEvent?.Invoke(_currentWave, _config.Waves.Length);
        }

        private void SpawnEnemy(GameObject character, bool isRandomPosition = true, Vector3? worldPosition = null)
        {
            Vector3 pos = Vector3.zero;
            if (isRandomPosition)
            {
                pos = new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
            }
            else
            {
                if (worldPosition == null)
                {
                    return;
                }

                pos = (Vector3)worldPosition;
            }

            EnemyBase enemy = Instantiate(character, pos, Quaternion.identity).GetComponent<EnemyBase>();

            enemy.Initialize(_player);

            enemy.OnEnemyDeathEvent += OnEnemyDeathEvent;
            enemy.OnDuplicatorDeathEvent += OnDuplicatorDeathEventHandler;

            _enemies.Add(enemy);
        }

        private void OnDuplicatorDeathEventHandler(Vector3 position)
        {
            for (int i = 0; i < _countOfClone; i++)
            {
                SpawnEnemy(_duplicatorCloneObject, false, new Vector3(position.x + UnityEngine.Random.Range(-2, 2), 0, position.y + UnityEngine.Random.Range(-2, 2)));
            }
        }

        private void OnEnemyDeathEvent(EnemyBase enemy)
        {
            _player.Heal();

            _enemies.Remove(enemy);

            if (enemy is DuplicatorEnemy)
            {
                return;
            }

            if (_enemies.IsEmpty())
            {
                Spawn();
            }
        }
    }
}