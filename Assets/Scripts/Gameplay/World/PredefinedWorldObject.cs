using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Predefined World Object")]
    public class PredefinedWorldObject : ScriptableObject
    {
        [SerializeField] private GameObject _prefab = null;
        [SerializeField] private EPosition _position = EPosition.Random;

        public GameObject Prefab => _prefab;
        public EPosition Position => _position;

        public enum EPosition
        {
            Top,
            Bottom,
            Random
        }
    }
}