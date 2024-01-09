using System;
using UnityEngine;

namespace Test.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Wave", menuName = "Data/Waves")]
    [Serializable]
    public class Wave : ScriptableObject
    {
        public GameObject[] Characters;
    }
}