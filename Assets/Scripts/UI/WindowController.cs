using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Exceptions;
using Miner.Management.Events;
using System.Linq;

namespace Miner.UI
{
    public class WindowController : MonoBehaviour
    {
        [SerializeField] private GameEvent _enablePlayerController = null;
        [SerializeField] private GameEvent _disablePlayerController = null;

        private List<GameObject> _openedWindows = new List<GameObject>();

        public void OnCreateWindow(EventArgs args)
        {
            if(args is CreateWindowEA cw)
            {
                if (_openedWindows.FirstOrDefault( x => x.name == cw.WindowPrefab.name) == null)
                    _openedWindows.Add(Instantiate(cw.WindowPrefab, transform));

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
                Destroy(cw.ClosedWindow);
                _openedWindows.Remove(cw.ClosedWindow);

                if (_openedWindows.Count == 0)
                    _enablePlayerController.Raise();
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }
    }
}