using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.UI;

namespace Miner.Gameplay
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private Window _windowPrefab = null;
        [Header("Events")]
        [SerializeField] private GameEvent _createWindow = null;

        private bool _locked = true;

        public void OnTriggerInteraction()
        {
            if (_locked) return;
            _createWindow.Raise(new CreateWindowEA(_windowPrefab));
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.TryGetComponent<PlayerController>(out _))
            {
                _locked = false;
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<PlayerController>(out _))
            {
                _locked = true;
            }
        }
    }
}