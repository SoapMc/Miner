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
#pragma warning disable 0414
        [SerializeField] private GameRules _gameRules = null;
        [SerializeField] private bool _hideCursor = true;
#pragma warning restore 0414
        private Log _log;
        private PlayerManager _player = null;
        private WorldManager _worldManager = null;
        private InfrastructureManager _infrastructure = null;
        private InputManager _inputManager = null;
        private HUDController _hud = null;

        public InputManager Input => _inputManager;
        public Log Log => _log;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _log = new Log();
                _worldManager = GetComponentInChildren<WorldManager>();
                _player = GetComponentInChildren<PlayerManager>();
                _infrastructure = GetComponentInChildren<InfrastructureManager>();
                _inputManager = GetComponentInChildren<InputManager>();
                _hud = FindObjectOfType<HUDController>();
                if (_hideCursor)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _log.Dispose();
        }

        private bool LoadFromFile()
        {
            ResetState();
            SavedData sd = new SavedData();
            
            bool success = SaveLoadSystem.LoadFromFile(ref sd, _profileFileName);
            if (SaveLoadSystem.LoadFromFile(ref sd.World.WorldControllerData.Tilemap, _worldFileName, SaveLoadSystem.EFormat.Txt) == false)
                success = false;

            if (success)
            {
                _worldManager.Load(sd.World);
                _player.Load(sd.Player);
                _infrastructure.Load(sd.Infrastructure);
            }
            else
            {
                Log.Write("Cannot load save state");
                StartNewGame();
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
            SaveLoadSystem.SaveToFile(sd.World.WorldControllerData.Tilemap, _worldFileName, SaveLoadSystem.EFormat.Txt);
        }

        private void RemoveSaveState()
        {
            SaveLoadSystem.RemoveFile(_profileFileName);
            SaveLoadSystem.RemoveFile(_worldFileName);
        }

        public void ResetState()
        {
            _worldManager.ResetState();
            _player.ResetState();
            _infrastructure.ResetState();
            _hud.ResetHUD();
        }

        public void StartNewGame()
        {
            RemoveSaveState();
            _worldManager.CreateNewWorld();
            _player.CreateNewPlayer();
            _infrastructure.CreateNewInfrastructure();
        }

        public void LoadGame()
        {
            LoadFromFile();
        }

        public class SavedData
        {
            public PlayerData Player = new PlayerData();
            public InfrastructureData Infrastructure = new InfrastructureData();
            public WorldData World = new WorldData();
        }
    }
}