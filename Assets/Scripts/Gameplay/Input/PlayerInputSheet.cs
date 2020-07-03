using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Input/Input Sheet")]
    public class PlayerInputSheet : ScriptableObject
    {
        [Header("Keybinding")]
        [SerializeField] private KeyCode _inventoryViewKey = KeyCode.Tab;
        [SerializeField] private KeyCode _confirmKey = KeyCode.Return;  //also open window
        [SerializeField] private KeyCode _nextUsableItemKey = KeyCode.E;
        [SerializeField] private KeyCode _previousUsableItemKey = KeyCode.Q;
        [SerializeField] private KeyCode _useItemKey = KeyCode.Space;
        [SerializeField] private KeyCode _openMenuKey = KeyCode.Escape;

        private float _horizontalMove = 0f;
        private float _verticalMove = 0f;
        private bool _inventoryView = false;
        private bool _openWindow = false;
        private bool _nextUsableItem = false;
        private bool _previousUsableItem = false;
        private bool _confirm = false;
        private bool _openMenu = false;

        public float HorizontalMove => _horizontalMove;
        public float VecticalMove => _verticalMove;

        public event Action InventoryViewKeyPressed;
        public event Action ConfirmKeyPressed;
        public event Action NextUsableItemKeyPressed;
        public event Action PreviousUsableItemKeyPressed;
        public event Action UseItemKeyPressed;
        public event Action OpenMenuKeyPressed;

        private void Update()
        {
            _horizontalMove = Input.GetAxis("Horizontal");
            _verticalMove = Input.GetAxis("Vertical");
        }

    }
}