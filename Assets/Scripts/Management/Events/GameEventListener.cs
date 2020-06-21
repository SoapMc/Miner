using UnityEngine;
using UnityEngine.Events;
using System;

namespace Miner.Management.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField]
        private GameEvent _event = null;
        [SerializeField]
        private UnityEventWithParameter _response = null;

        private void OnEnable()
        {
            _event.RegisterEvent(this);
        }

        private void OnDisable()
        {
            _event.UnregisterEvent(this);
        }

        public void OnEventRaised(EventArgs args)
        {
            _response.Invoke(args);
        }
    }
}