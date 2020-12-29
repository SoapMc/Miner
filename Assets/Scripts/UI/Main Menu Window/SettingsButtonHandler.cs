using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.UI
{
    public class SettingsButtonHandler : MonoBehaviour
    {
        [SerializeField] private GameEvent _createWindow = null;
        [SerializeField] private SettingsWindow _settingsWindow = null;

        public void CreateSettingsWindow()
        {
            _createWindow.Raise(new CreateWindowEA(_settingsWindow));
        }
    }
}