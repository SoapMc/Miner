using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using System;
using Miner.Management.Exceptions;
using Miner.Management;

namespace Miner.UI
{
    public class MessageController : MonoBehaviour, IResettableHUDComponent
    {
        [SerializeField] private MessageWindow _messageWindowPrefab = null;
        private MessageWindow _messageWindow = null;

        public void OnCreateMessage(EventArgs args)
        {
            if(args is CreateMessageEA cm)
            {
                if (_messageWindow != null)
                    _messageWindow.Close();
                _messageWindow = Instantiate(_messageWindowPrefab, transform);
                _messageWindow.Initialize(cm.Title, cm.Message, cm.Type, cm.Time);
            }
            else
            {
                Log.Instance.WriteException(new InvalidEventArgsException());
            }
        }

        public void ResetComponent()
        {
            if (_messageWindow != null)
            {
                Destroy(_messageWindow.gameObject);
                _messageWindow = null;
            }
        }
    }
}