using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Data/BattleCampInfo")]
    public class LevelConfig : ScriptableObject
    {
        public Wave[] Waves;
    }
}