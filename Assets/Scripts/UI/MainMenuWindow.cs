using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Miner.Management.Events;
using Miner.Management;

namespace Miner.UI
{
    public class MainMenuWindow : Window
    {
        [SerializeField] private GameEvent _disableHUD = null;
        [SerializeField] private GameEvent _enableHUD = null;
        [SerializeField] private GameEvent _playMusic = null;
        [SerializeField] private AudioClip _menuMusic = null;

        public void StartNewGame()
        {
            GameManager.Instance.ResetState();
            _appearingEffect.TriggerDisappearing();
            _enableHUD.Raise();
        }

        public void LoadGame()
        {
            GameManager.Instance.LoadFromFile();
            _appearingEffect.TriggerDisappearing();
            _enableHUD.Raise();
        }

        public void Exit()
        {
            Application.Quit();
        }

        protected override void CloseWindow()
        {
            
        }

        protected override void OnAppearingFinished()
        {
            base.OnAppearingFinished();
            GameManager.Instance.Unload();
            _disableHUD.Raise();
        }

        private void Start()
        {
            _playMusic.Raise(new PlayMusicEA(_menuMusic));
            GameManager.Instance.Unload();
        }
    }
}