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
        [SerializeField] private IntReference _timeOfDay = null;
        [SerializeField] private IntReference _elapsedDays = null;
        [SerializeField] private WorldController _worldPrefab = null;
        private WorldController _worldController = null;
        private int _seed = -1;

        public void ResetState()
        {
            Unload();
            _worldController = Instantiate(_worldPrefab, Vector3.zero, Quaternion.identity);
            _seed = (int)System.DateTime.Now.Ticks;
            Random.InitState(_seed);
            WorldGenerator wg = _worldController.GetComponent<WorldGenerator>();
            wg.GenerateWorld(_worldController, _seed);
        }

        public void Load(WorldData data)
        {
            Unload();
            _worldController = Instantiate(_worldPrefab, Vector3.zero, Quaternion.identity);
            _seed = data.Seed;
            WorldGenerator wg = _worldController.GetComponent<WorldGenerator>();
            wg.LoadWorld();
            _timeOfDay.Value = data.TimeOfDay;
            _elapsedDays.Value = data.ElapsedDays;
            _worldController.Initialize(data.TilemapData);
        }

        public void Unload()
        {
            if (_worldController != null)
                Destroy(_worldController.gameObject);
        }

        public WorldData RetrieveSerializableData()
        {
            return new WorldData()
            {
                ElapsedDays = _elapsedDays.Value,
                TimeOfDay = _timeOfDay.Value,
                Seed = _seed,
                TilemapData = _worldController.RetrieveSerializableData()
            };
        }
    }
}