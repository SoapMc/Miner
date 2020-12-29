using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System;
using Miner.FX;
using System.Linq;

namespace Miner.UI
{
    public class InputHelpController : MonoBehaviour
    {
        [SerializeField] private InputHelpPanel _shownPanel = null;
        
        public void OnShowInputHelp(EventArgs args)
        {
            if (args is ShowInputHelpEA sih)
            {
                _shownPanel.gameObject.SetActive(true);
                _shownPanel.UpdatePanel(sih.Elements);
            }
            else
            {
                Management.Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void OnHideInputHelp()
        {
            _shownPanel.gameObject.SetActive(false);
        }

        private void Awake()
        {
            _shownPanel.gameObject.SetActive(false);
        }
    }
}