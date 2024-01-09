using System.Collections;
using System.Collections.Generic;
using Test.Managers;
using UnityEngine;
using Zenject;

namespace Test.SceneEntries
{
    public class ProjectEntry : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UIManager>().FromComponentInHierarchy().AsCached();
        }

        public override void Start()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
    }
}