using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Exceptions;
using Miner.Management.Events;
using System.Linq;
using Miner.Management;
using Miner.FX;
using Miner.Gameplay;
using WindowCreatedEA = Miner.Management.Events.CreateWindowEA;
using UnityEngine.InputSystem;

namespace Miner.UI
{
    public class WindowController : MonoBehaviour
    {
        [SerializeField] private GameEvent _enablePlayerController = null;
        [SerializeField] private GameEvent _disablePlayerController = null;
        [SerializeField] private GameEvent _windowCreated = null;
        [SerializeField] private GameEvent _windowClosed = null;
        [SerializeField] private Window _mainMenuWindow = null;
        [SerializeField] private Window _smallMenuWindow = null;
        [SerializeField] private SoundEffect _openWindowSFX = null;
        private List<Tuple<string, Window>> _openedWindows = new List<Tuple<string, Window>>();
        private Management.Controls _controls = null;

        public void OnCreateWindow(EventArgs args)
        {
            if(args is CreateWindowEA cw)
            {
                if (_openedWindows.Exists(x => x.Item1 == cw.WindowPrefab.name) == false)
                {
                    foreach(var openedWindow in _openedWindows)
                    {
                        openedWindow.Item2.Interactable = false;
                        openedWindow.Item2.LoseFocus();
                    }
                    Window newWindow = Instantiate(cw.WindowPrefab, transform);
                    _openedWindows.Add(new Tuple<string, Window>(cw.WindowPrefab.name, newWindow));
                    _openWindowSFX.Play();
                    newWindow.GainFocus();
                    _windowCreated.Raise(new WindowCreatedEA(newWindow));
                }

                if (_openedWindows.Count > 0)
                {
                    _disablePlayerController.Raise();
                    Time.timeScale = 0f;
                }
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnCloseWindow(EventArgs args)
        {
            if (args is CloseWindowEA cw)
            {
                try
                {
                    _openedWindows.Remove(_openedWindows.Find(x => x.Item2 == cw.ClosedWindow));
                    cw.ClosedWindow.LoseFocus();
                    Destroy(cw.ClosedWindow.gameObject);
                    _windowClosed.Raise();
                    if (_openedWindows.Count > 0)
                    {
                        var focusedWindow = _openedWindows.Last().Item2;
                        focusedWindow.Interactable = true;
                        focusedWindow.SelectFirstObject();
                        focusedWindow.GainFocus();
                    }
                }
                catch(ArgumentNullException ex)
                {
                    Debug.LogException(ex);
                    if(cw.ClosedWindow != null)
                        DestroyImmediate(cw.ClosedWindow.gameObject);
                }

                if (_openedWindows.Count == 0)
                {
                    _enablePlayerController.Raise();
                    Time.timeScale = 1f;
                }
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OpenMainMenu()
        {
            for (int i = _openedWindows.Count - 1; i >= 0; --i)
            {
                OnCloseWindow(new CloseWindowEA(_openedWindows[i].Item2));
            }
            OnCreateWindow(new CreateWindowEA(_mainMenuWindow));
        }

        public void OpenSmallMenu()
        {
            OnCreateWindow(new CreateWindowEA(_smallMenuWindow));
        }

        private void OnCancelPerformed(InputAction.CallbackContext context)
        {
            if (_openedWindows.Count == 0)
                OpenSmallMenu();
        }

        private void Start()
        {
            _controls = new Controls();
            _controls.Player.Cancel.performed += OnCancelPerformed;
            _controls.Enable();
            OnCreateWindow(new CreateWindowEA(_mainMenuWindow));
        }
    }
}