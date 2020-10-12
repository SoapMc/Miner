using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management;
using Miner.Management.Events;
using Miner.UI;
using System.IO;
using Miner.Gameplay;
using UnityEngine.EventSystems;

namespace Miner.Management
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private readonly string _profileFileName = "Profile.json";
        private readonly string _worldFileName = "World.txt";
        [SerializeField] private bool _loadOnAwake = false;
        [SerializeField] private GameRules _gameRules = null;

        private Log _log;
        private PlayerManager _player;
        private WorldManager _worldManager;
        private InfrastructureManager _infrastructure;

        public Log Log => _log;
        public PlayerManager Player => _player;
        public WorldManager World => _worldManager;
        public InfrastructureManager Infrastructure => _infrastructure;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _log = new Log();
                _worldManager = GetComponentInChildren<WorldManager>();
                _player = GetComponentInChildren<PlayerManager>();
                _infrastructure = GetComponentInChildren<InfrastructureManager>();
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            if(Instance == this)
            {
                if (_loadOnAwake)
                {
                    LoadFromFile();
                }
            }
        }

        public bool LoadFromFile()
        {
            SavedData sd = new SavedData();
            
            bool success = SaveLoadSystem.LoadFromFile(ref sd, _profileFileName);
            if (SaveLoadSystem.LoadFromFile(ref sd.World.TilemapData, _worldFileName, SaveLoadSystem.EFormat.Txt) == false)
                success = false;

            if (success)
            {
                _worldManager.Load(sd.World);
                _player.Load(sd.Player);
                _infrastructure.Load(sd.Infrastructure);
            }
            else
            {
                Debug.Log("Cannot load save state");
                ResetState();
            }

            return success;
        }

        public void SaveToFile()
        {
            SavedData sd = new SavedData()
            {
                Player = _player.RetrieveSerializableData(),
                Infrastructure = _infrastructure.RetrieveSerializableData(),
                World = _worldManager.RetrieveSerializableData()
            };
            SaveLoadSystem.SaveToFile(sd, _profileFileName);
            SaveLoadSystem.SaveToFile(sd.World.TilemapData, _worldFileName, SaveLoadSystem.EFormat.Txt);
        }

        public void Unload()
        {
            _player.Unload();
            _worldManager.Unload();
        }

        public void ResetState()
        {
            SaveLoadSystem.RemoveFile(_profileFileName);
            SaveLoadSystem.RemoveFile(_worldFileName);
            _worldManager.ResetState();
            _player.ResetCharacter();
            _infrastructure.ResetState();
        }

        public void StartNewGame()
        {
            ResetState();
        }

        public class SavedData
        {
            public PlayerData Player;
            public InfrastructureData Infrastructure;
            public WorldData World;
        }
    }
}