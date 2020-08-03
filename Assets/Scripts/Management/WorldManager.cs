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
            if (_worldController != null)
                Destroy(_worldController.gameObject);
            _worldController = Instantiate(_worldPrefab, Vector3.zero, Quaternion.identity);
            _seed = (int)System.DateTime.Now.Ticks;
            Random.InitState(_seed);
            WorldGenerator wg = _worldController.GetComponent<WorldGenerator>();
            wg.GenerateWorld(_worldController, _seed);
        }

        public void Load(int seed)
        {
            if (_worldController != null)
                Destroy(_worldController.gameObject);
            _worldController = Instantiate(_worldPrefab, Vector3.zero, Quaternion.identity);
            _seed = seed;
            WorldGenerator wg = _worldController.GetComponent<WorldGenerator>();
            wg.GenerateWorld(_worldController, seed);
        }

        public int RetrieveSerializableData()
        {
            return _seed;
        }
    }
}