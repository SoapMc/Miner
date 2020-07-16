using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Exceptions;
using Miner.Management.Events;
using System.Linq;
using Miner.Management;

namespace Miner.UI
{
    public class WindowController : MonoBehaviour
    {
        [SerializeField] private GameEvent _enablePlayerController = null;
        [SerializeField] private GameEvent _disablePlayerController = null;
        [SerializeField] private GameObject _mainMenuWindow = null;

        private List<Tuple<string, GameObject>> _openedWindows = new List<Tuple<string, GameObject>>();

        public void OnCreateWindow(EventArgs args)
        {
            if(args is CreateWindowEA cw)
            {
                if (_openedWindows.FirstOrDefault(x => x.Item1 == cw.WindowPrefab.name) == null)
                {
                    GameObject newWindow = Instantiate(cw.WindowPrefab, transform);
                    _openedWindows.Add(new Tuple<string, GameObject>(cw.WindowPrefab.name, newWindow));
                }

                if (_openedWindows.Count > 0)
                    _disablePlayerController.Raise();
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
                    Destroy(cw.ClosedWindow);
                }
                catch(ArgumentNullException ex)
                {
                    Debug.LogException(ex);
                    if(cw.ClosedWindow != null)
                        DestroyImmediate(cw.ClosedWindow);
                }

                if (_openedWindows.Count == 0)
                    _enablePlayerController.Raise();

                GameManager.Instance.SaveToFile();
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        private void Start()
        {
            OnCreateWindow(new CreateWindowEA(_mainMenuWindow));
        }
    }
}