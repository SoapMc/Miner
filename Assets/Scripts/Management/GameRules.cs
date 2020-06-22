using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Management
{
    [CreateAssetMenu(menuName = "Game Rules")]
    public class GameRules : ScriptableObject
    {
        private static GameRules _instance = null;

        public static GameRules Instance => _instance;

        [SerializeField] private float _realDimensionOfTile = 5;    //in meters

        public float RealDimensionOfTile => _realDimensionOfTile;

        public float GetDrillSharpnessCoefficient(int gridDepth)
        {
            return 0.9f / (0.0005f * (Mathf.Abs(gridDepth) * _realDimensionOfTile) + 1) + 0.1f;
        }

        private void OnEnable()
        {
            if (_instance == null)
                _instance = this;
            else if(_instance != null && _instance != this)
                Destroy(this);
        }
    }
}