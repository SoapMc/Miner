using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using System;
using Miner.Management.Exceptions;

namespace Miner.UI
{
    public class MessageController : MonoBehaviour
    {
        [SerializeField] private GameEvent _createMessage = null;
        [SerializeField] private GameEvent _createWindow = null;
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
                throw new InvalidEventArgsException();
            }
        }

        public void OnDisableHUD()
        {
            if (_messageWindow != null)
                _messageWindow.Close();
        }
    }
}