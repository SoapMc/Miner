﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Input/Input Sheet")]
    public class PlayerInputSheet : ScriptableObject
    {
        private float _horizontalMove = 0f;
        private float _verticalMove = 0f;

        private bool _inventory = false;
        private bool _next = false;
        private bool _previous = false;
        private bool _confirm = false;
        private bool _cancel = false;
        private bool _use = false;

        public float HorizontalMove => _horizontalMove;
        public float VerticalMove => _verticalMove;

        public event Action InventoryViewKeyPressed;
        public event Action InventoryViewKeyUp;

        public event Action ConfirmKeyPressed;
        public event Action NextKeyPressed;
        public event Action PreviousKeyPressed;
        public event Action UseKeyPressed;
        public event Action CancelKeyPressed;

        public void Update()
        {
            _horizontalMove = Input.GetAxis("Horizontal");
            _verticalMove = Input.GetAxis("Vertical");
            if (Mathf.Abs(Input.GetAxis("Inventory")) > 0.1f)
            {
                if (_inventory == false)
                    InventoryViewKeyPressed?.Invoke();
                _inventory = true;
            }
            else
            {
                if (_inventory == true)
                    InventoryViewKeyUp?.Invoke();
                _inventory = false;
            }

            if (Mathf.Abs(Input.GetAxis("Confirm")) > 0.1f)
            {
                if (_confirm == false)
                    ConfirmKeyPressed?.Invoke();
                _confirm = true;
            }
            else
                _confirm = false;

            if (Mathf.Abs(Input.GetAxis("Cancel")) > 0.1f)
            {
                if (_cancel == false)
                    CancelKeyPressed?.Invoke();
                _cancel = true;
            }
            else
                _cancel = false;

            if (Mathf.Abs(Input.GetAxis("Use")) > 0.1f)
            {
                if (_use == false)
                    UseKeyPressed?.Invoke();
                _use = true;
            }
            else
                _use = false;

            if (Mathf.Abs(Input.GetAxis("Previous")) > 0.1f)
            {
                if (_previous == false)
                    PreviousKeyPressed?.Invoke();
                _previous = true;
            }
            else
                _previous = false;

            if (Mathf.Abs(Input.GetAxis("Next")) > 0.1f)
            {
                if (_next == false)
                    NextKeyPressed?.Invoke();
                _next = true;
            }
            else
                _next = false;
        }

    }
}