using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.Management;
using UnityEngine.Events;
using Miner.Gameplay;

namespace Miner.Management
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField] private WorldController _worldPrefab = null;
        private WorldController _worldController = null;
        private int _seed = -1;

        public void ResetState()
        {
            _worldController = Instantiate(_worldPrefab, Vector3.zero, Quaternion.identity);
            _seed = (int)System.DateTime.Now.Ticks;
            Random.InitState(_seed);
            _worldController.GenerateWorld(_seed);
        }

        public void Load(int seed)
        {
            _worldController = Instantiate(_worldPrefab, Vector3.zero, Quaternion.identity);
            _seed = seed;
            _worldController.GenerateWorld(seed);
        }

        public int RetrieveSerializableData()
        {
            return _seed;
        }
    }
}