using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;
using Miner.Management.Events;

namespace Miner.UI
{
    public class WorkshopWindow : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private PlayerInputSheet _input = null;

        [Header("Events")]
        [SerializeField] private GameEvent _closeWindow = null;

        private void CloseWindow()
        {
            _closeWindow.Raise(new CloseWindowEA(gameObject));
            _input.CancelKeyPressed -= CloseWindow;
        }

        private void Start()
        {
            _input.CancelKeyPressed += CloseWindow;
        }
    }
}