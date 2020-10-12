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
namespace Miner.UI
{
    public class WindowController : MonoBehaviour
    {
        [SerializeField] private GameEvent _enablePlayerController = null;
        [SerializeField] private GameEvent _disablePlayerController = null;
        [SerializeField] private GameEvent _windowCreated = null;
        [SerializeField] private GameEvent _windowClosed = null;
        [SerializeField] private GameObject _windowCreatorAndDestroyer = null;
        [SerializeField] private Window _mainMenuWindow = null;
        [SerializeField] private SoundEffect _openWindowSFX = null;
        [SerializeField] private PlayerInputSheet _input = null;
        private List<Tuple<string, Window>> _openedWindows = new List<Tuple<string, Window>>();


        public void OnCreateWindow(EventArgs args)
        {
            if(args is CreateWindowEA cw)
            {
                if (_openedWindows.FirstOrDefault(x => x.Item1 == cw.WindowPrefab.name) == null)
                {
                    _openedWindows.All(x => x.Item2.Interactable = false);
                    Window newWindow = Instantiate(cw.WindowPrefab, transform);
                    _openedWindows.Add(new Tuple<string, Window>(cw.WindowPrefab.name, newWindow));
                    _openWindowSFX.Play();
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
                throw new InvalidEventArgsException();
            }
        }

        public void OnCloseWindow(EventArgs args)
        {
            if (args is CloseWindowEA cw)
            {
                try
                {
                    _openedWindows.Remove(_openedWindows.Find(x => x.Item2 == cw.ClosedWindow));
                    Destroy(cw.ClosedWindow.gameObject);
                    _windowClosed.Raise();
                    if (_openedWindows.Count > 0)
                    {
                        _openedWindows.Last().Item2.Interactable = true;
                        _openedWindows.Last().Item2.SelectFirstObject();
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

                GameManager.Instance.SaveToFile();
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnCancelKeyHold()
        {
            for(int i = _openedWindows.Count - 1; i >= 0; --i)
            {
                OnCloseWindow(new CloseWindowEA(_openedWindows[i].Item2));
            }
            OnCreateWindow(new CreateWindowEA(_mainMenuWindow));
        }

        private void Start()
        {
            _input.CancelKeyHold += OnCancelKeyHold;
            OnCreateWindow(new CreateWindowEA(_mainMenuWindow));
        }

        private void OnDestroy()
        {
            _input.CancelKeyHold -= OnCancelKeyHold;
        }
    }
}