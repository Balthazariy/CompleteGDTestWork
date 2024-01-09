using System.Collections;
using System.Collections.Generic;
using Test.Enemies;
using Test.Managers;
using Test.Player;
using UnityEngine;
using Zenject;

namespace Test.SceneEntries
{
    public class GameEntry : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Player.Player>().FromComponentInHierarchy().AsCached();
            Container.Bind<EnemySpawnerManager>().FromComponentInHierarchy().AsCached();
        }
    }
}