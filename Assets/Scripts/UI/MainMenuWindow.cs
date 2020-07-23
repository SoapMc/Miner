using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Miner.Management.Events;
using Miner.Management;

namespace Miner.UI
{
    public class MainMenuWindow : MonoBehaviour
    {
        [SerializeField] private GameEvent _closeWindow = null;
        [SerializeField] private Selectable _firstSelectedElement = null;
        [SerializeField] private GameEvent _disableHUD = null;
        [SerializeField] private GameEvent _enableHUD = null;
        [SerializeField] private GameEvent _disablePlayerController = null;
        [SerializeField] private GameEvent _enablePlayerController = null;
        [SerializeField] private GameEvent _playMusic = null;
        [SerializeField] private AudioClip _menuMusic = null;

        public void StartNewGame()
        {
            GameManager.Instance.ResetState();
            _closeWindow.Raise(new CloseWindowEA(gameObject));
            _enableHUD.Raise();
            _enablePlayerController.Raise();
        }

        public void LoadGame()
        {
            GameManager.Instance.LoadFromFile();
            _closeWindow.Raise(new CloseWindowEA(gameObject));
            _enableHUD.Raise();
            _enablePlayerController.Raise();
        }

        public void Exit()
        {
            Application.Quit();
        }

        private void Start()
        {
            GameManager.Instance.Unload();
            EventSystem.current.SetSelectedGameObject(_firstSelectedElement.gameObject);
            _firstSelectedElement.OnSelect(null);
            _disableHUD.Raise();
            _disablePlayerController.Raise();
            _playMusic.Raise(new PlayMusicEA(_menuMusic));
        }
    }
}