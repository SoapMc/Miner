using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using Miner.Management.Events;

namespace Miner.UI
{
    public class WorkshopWindow : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private GameEvent _closeWindow = null;

        private void CloseWindow()
        {
            _closeWindow.Raise(new CloseWindowEA(gameObject));
        }

        private void Awake()
        {
            
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CloseWindow();
            }
        }
    }
}