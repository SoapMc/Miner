using System.Collections;
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

        private bool _rightMoveKeyState = false;
        private bool _leftMoveKeyState = false;
        private bool _upMoveKeyState = false;
        private bool _downMoveKeyState = false;
        private bool _inventory = false;
        private bool _next = false;
        private bool _previous = false;
        private bool _confirm = false;
        private bool _cancel = false;
        private bool _use = false;
        private float _cancelKeyHoldTime = 0f;

        public float HorizontalMove => _horizontalMove;
        public float VerticalMove => _verticalMove;

        public event Action RightMoveKeyPressed;
        public event Action RightMoveKeyUp;
        public event Action LeftMoveKeyPressed;
        public event Action LeftMoveKeyUp;
        public event Action UpMoveKeyPressed;
        public event Action UpMoveKeyUp;
        public event Action DownMoveKeyPressed;
        public event Action DownMoveKeyUp;
        public event Action InventoryViewKeyPressed;
        public event Action InventoryViewKeyUp;
        public event Action ConfirmKeyPressed;
        public event Action NextKeyPressed;
        public event Action PreviousKeyPressed;
        public event Action UseKeyPressed;
        public event Action CancelKeyPressed;
        public event Action CancelKeyHold;

        public void Update()
        {
            _horizontalMove = Input.GetAxis("Horizontal");
            if (_horizontalMove > 0.05f)
            {
                if (_rightMoveKeyState == false)
                    RightMoveKeyPressed?.Invoke();
                _rightMoveKeyState = true;
            }
            else
            {
                if (_rightMoveKeyState == true)
                    RightMoveKeyUp?.Invoke();
                _rightMoveKeyState = false;
            }

            if (_horizontalMove < -0.05f)
            {
                if (_leftMoveKeyState == false)
                    LeftMoveKeyPressed?.Invoke();
                _leftMoveKeyState = true;
            }
            else
            {
                if (_leftMoveKeyState == true)
                    LeftMoveKeyUp?.Invoke();
                _leftMoveKeyState = false;
            }

            _verticalMove = Input.GetAxis("Vertical");
            if (_verticalMove > 0.05f)
            {
                if (_upMoveKeyState == false)
                    UpMoveKeyPressed?.Invoke();
                _upMoveKeyState = true;
            }
            else
            {
                if (_upMoveKeyState == true)
                    UpMoveKeyUp?.Invoke();
                _upMoveKeyState = false;
            }

            if (_verticalMove < -0.05f)
            {
                if (_downMoveKeyState == false)
                    DownMoveKeyPressed?.Invoke();
                _downMoveKeyState = true;
            }
            else
            {
                if (_downMoveKeyState == true)
                    DownMoveKeyUp?.Invoke();
                _downMoveKeyState = false;
            }

            if (Input.GetAxisRaw("Inventory") != 0)
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

            if (Input.GetAxisRaw("Confirm") != 0)
            {
                if (_confirm == false)
                    ConfirmKeyPressed?.Invoke();
                _confirm = true;
            }
            else
                _confirm = false;

            if (Input.GetAxisRaw("Cancel") != 0)
            {
                if (_cancel == false)
                {
                    _cancelKeyHoldTime = 0f;
                    CancelKeyPressed?.Invoke();
                }
                _cancel = true;
                _cancelKeyHoldTime += Time.unscaledDeltaTime;
                if (_cancelKeyHoldTime >= 1f)
                    CancelKeyHold.Invoke();
            }
            else
                _cancel = false;

            if (Input.GetAxisRaw("Use") != 0)
            {
                if (_use == false)
                    UseKeyPressed?.Invoke();
                _use = true;
            }
            else
                _use = false;

            if (Input.GetAxisRaw("Previous") != 0)
            {
                if (_previous == false)
                    PreviousKeyPressed?.Invoke();
                _previous = true;
            }
            else
                _previous = false;

            if (Input.GetAxisRaw("Next") != 0)
            {
                if (_next == false)
                    NextKeyPressed?.Invoke();
                _next = true;
            }
            else
                _next = false;
        }

        public void Reset()
        {
            if(_inventory == true)
                InventoryViewKeyUp?.Invoke();
            if (_downMoveKeyState == true)
                DownMoveKeyUp?.Invoke();
            if (_upMoveKeyState == true)
                UpMoveKeyUp?.Invoke();
            if (_leftMoveKeyState == true)
                LeftMoveKeyUp?.Invoke();
            if (_rightMoveKeyState == true)
                RightMoveKeyUp?.Invoke();

            _horizontalMove = 0f;
            _verticalMove = 0f;
            _inventory = false;
            _next = false;
            _previous = false;
            _confirm = false;
            _cancel = false;
            _use = false;
        }

    }
}