using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Miner.Management.Events;
using Miner.Management;
using Miner.FX;
using UnityEngine.InputSystem;

namespace Miner.UI
{
    public class MainMenuWindow : Window
    {
        [SerializeField] private GameEvent _disableHUD = null;
        [SerializeField] private GameEvent _enableHUD = null;
        [SerializeField] private GameEvent _playMusic = null;
        [SerializeField] private GameEvent _disablePlayer = null;
        [SerializeField] private AudioClip _menuMusic = null;

        public void StartNewGame()
        {
            _enableHUD.Raise();
            GameManager.Instance.StartNewGame();
            _appearingEffect.TriggerDisappearing();
            Interactable = false;
        }

        public void LoadGame()
        {
            _enableHUD.Raise();
            GameManager.Instance.LoadGame();
            _appearingEffect.TriggerDisappearing();
            Interactable = false;
        }

        public void Exit()
        {
            Application.Quit();
            Interactable = false;
        }

        protected override void CloseWindow(InputAction.CallbackContext context)
        {
            
        }

        protected override void OnAppearingFinished()
        {
            base.OnAppearingFinished();
            GameManager.Instance.ResetState();
            _disableHUD.Raise();
        }

        private void Start()
        {
            _playMusic.Raise(new PlayMusicEA(_menuMusic));
            _disablePlayer.Raise();
        }
    }
}