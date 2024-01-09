using System.Collections;
using System.Collections.Generic;
using Test.Managers;
using Test.Scenes.Base;
using Test.UI;
using UnityEngine;
using Zenject;

namespace Test.Scenes
{
    public class SceneViewGame : SceneView
    {
        private Player.Player _player;
        private EnemySpawnerManager _enemySpawnerManager;

        [Inject]
        public void Construct(Player.Player player, EnemySpawnerManager enemySpawnerManager)
        {
            _player = player;
            _enemySpawnerManager = enemySpawnerManager;

            _player.OnPlayerDeathEvent += OnPlayerDeathEventHandler;
            _enemySpawnerManager.OnAllWavesCompletedEvent += OnAllWavesCompletedEventHandler;
        }

        private void OnPlayerDeathEventHandler()
        {
            _uiSystem.ShowView<ViewLosePage>();
        }

        private void OnAllWavesCompletedEventHandler()
        {
            _uiSystem.ShowView<ViewWinPage>();
        }
    }
}