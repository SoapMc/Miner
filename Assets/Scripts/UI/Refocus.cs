using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Miner.UI
{
    public class Refocus : MonoBehaviour
    {
        private GameObject _lastSelected = null;

        private void Start()
        {
            _lastSelected = new GameObject();
        }

        void Update()
        {

            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(_lastSelected);
            }
            else
            {
                _lastSelected = EventSystem.current.currentSelectedGameObject;
            }
        }
        
    }
}