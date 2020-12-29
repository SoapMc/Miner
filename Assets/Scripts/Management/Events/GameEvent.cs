using System.Collections.Generic;
using UnityEngine;
using System;

namespace Miner.Management.Events
{
    [CreateAssetMenu(menuName = "Game Event")]
    public class GameEvent : ScriptableObject
    {
        private List<IGameEventListener> _listeners = new List<IGameEventListener>();

        public void Raise()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].OnEventRaised(null);
        }

        public void Raise(EventArgs args = null)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].OnEventRaised(args);
        }

        public void RegisterEvent(IGameEventListener listener)
        {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
            else
                Debug.LogWarning("The listener has been already registered!");
        }

        public void UnregisterEvent(IGameEventListener listener)
        {
            if (_listeners.Contains(listener))
                _listeners.Remove(listener);
            else
                Debug.LogWarning("The listener hasn't been registered yet!");
        }
    }
}
