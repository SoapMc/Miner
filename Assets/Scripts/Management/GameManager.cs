using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management;
using Miner.Management.Events;
using Miner.UI;
using System.IO;
using Miner.Gameplay;

namespace Miner.Management
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private readonly string _profileFileName = "Profile.json";
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
        public GameRules GameRules => _gameRules;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _log = new Log();
                _player = GetComponentInChildren<PlayerManager>();
                _worldManager = GetComponentInChildren<WorldManager>();
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
            if(success)
            {
                _player.Load(sd.Player);
                _infrastructure.Load(sd.Infrastructure);
                _worldManager.Load(sd.Seed);
            }
            else
            {
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
                Seed = _worldManager.RetrieveSerializableData()
            };
            SaveLoadSystem.SaveToFile(sd, _profileFileName);
        }

        public void ResetState()
        {
            SaveLoadSystem.RemoveSaveState(_profileFileName);
            _player.ResetCharacter();
            _worldManager.ResetState();
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
            public int Seed;
        }
    }
}