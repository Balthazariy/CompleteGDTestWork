using System;
using System.Collections;
using System.Collections.Generic;
using Test.Managers;
using Test.Player;
using Test.UI.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Test.UI
{
    public class ViewGamePage : View
    {
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _superAttackButton;
        [SerializeField] private Button _restartButton;

        [SerializeField] private Image _superAttackButtonFillImage;

        [SerializeField] private TextMeshProUGUI _waveText;
        [SerializeField] private TextMeshProUGUI _playerText;

        private Player.Player _player;
        private EnemySpawnerManager _enemySpawnerManager;

        [Inject]
        public void Construct(Player.Player player, EnemySpawnerManager enemySpawnerManager)
        {
            _player = player;
            _enemySpawnerManager = enemySpawnerManager;

            _superAttackButtonFillImage.gameObject.SetActive(false);

            _player.OnPlayerHealthUpdate += (float current, float max) => _playerText.text = $"Health: {current}/{max}";
            _enemySpawnerManager.OnWaveChangeEvent += (int current, int all) => _waveText.text = $"Wave: {current}/{all}";
            _player.OnSuperAttackCooldownEvent += OnSuperAttackCooldownEventHandler;
            _player.OnEnemyInAttackRangeEvent += (bool inRange) => _superAttackButton.gameObject.SetActive(inRange);
            _attackButton.onClick.AddListener(() => _player.Attack());
            _superAttackButton.onClick.AddListener(() => _player.SuperAttack());
            _restartButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("Game"));
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private void OnDisable()
        {
            _player.OnPlayerHealthUpdate -= (float current, float max) => _playerText.text = $"Health: {current}/{max}";
            _enemySpawnerManager.OnWaveChangeEvent -= (int current, int all) => _waveText.text = $"Wave: {current}/{all}";
            _player.OnSuperAttackCooldownEvent -= OnSuperAttackCooldownEventHandler;
            _player.OnEnemyInAttackRangeEvent -= (bool inRange) => _superAttackButton.gameObject.SetActive(inRange);
            _attackButton.onClick.RemoveListener(() => _player.Attack());
            _superAttackButton.onClick.RemoveListener(() => _player.SuperAttack());
            _restartButton.onClick.RemoveListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("Game"));
        }

        private void OnSuperAttackCooldownEventHandler(float value)
        {
            _superAttackButtonFillImage.gameObject.SetActive(true);
            _superAttackButtonFillImage.fillAmount = value / 2.0f;

            if (_superAttackButtonFillImage.fillAmount >= 0.99f)
            {
                _superAttackButtonFillImage.gameObject.SetActive(false);
            }
        }
    }
}