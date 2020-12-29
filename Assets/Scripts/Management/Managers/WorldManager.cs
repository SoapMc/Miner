using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.Management;
using UnityEngine.Events;
using Miner.Gameplay;
using NewWorldCreatedEA = Miner.Management.Events.WorldLoadedEA;

namespace Miner.Management
{
    public class WorldManager : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private GameEvent _worldLoaded = null;
        [SerializeField] private GameEvent _newWorldCreated = null;
        [SerializeField] private GameEvent _worldReset = null;

        [Header("Resources")]
        [SerializeField] private IntReference _timeOfDay = null;
        [SerializeField] private IntReference _elapsedDays = null;
        [SerializeField] private MissionController _missionController = null;
        [SerializeField] private WorldController _worldPrefab = null;
        private WorldController _worldController = null;
        private int _seed = -1;

        public void ResetState()
        {
            if (_worldController != null)
                Destroy(_worldController.gameObject);
            _worldController = null;
            _missionController.ResetState();
            _timeOfDay.Value = 0;
            _elapsedDays.Value = 0;
            _seed = -1;
            _worldReset.Raise();
        }

        public void CreateNewWorld()
        {
            if (_worldController != null)
                Destroy(_worldController.gameObject);
            _worldController = Instantiate(_worldPrefab, Vector3.zero, Quaternion.identity);
            _seed = (int)System.DateTime.Now.Ticks;
            Random.InitState(_seed);
            WorldGenerator wg = _worldController.GetComponent<WorldGenerator>();
            wg.GenerateWorld(_worldController, _seed);
            _missionController.Initialize();
            _newWorldCreated.Raise(new NewWorldCreatedEA(_worldController.Grid, _worldController.PlayerSpawnPoint));
        }

        public void Load(WorldData data)
        {
            _worldController = Instantiate(_worldPrefab, Vector3.zero, Quaternion.identity);
            _seed = data.Seed;
            WorldGenerator wg = _worldController.GetComponent<WorldGenerator>();
            wg.LoadWorld();
            _timeOfDay.Value = data.TimeOfDay;
            _elapsedDays.Value = data.ElapsedDays;
            _worldController.Initialize(data.WorldControllerData.Tilemap);
            _missionController.Load(data.MissionControllerData);
            _worldLoaded.Raise(new WorldLoadedEA(_worldController.Grid, _worldController.PlayerSpawnPoint));
        }

        public WorldData RetrieveSerializableData()
        {
            return new WorldData()
            {
                ElapsedDays = _elapsedDays.Value,
                TimeOfDay = _timeOfDay.Value,
                Seed = _seed,
                WorldControllerData = _worldController.RetrieveSerializableData(),
                MissionControllerData = _missionController.RetrieveSerializableData()
            };
        }
    }
}