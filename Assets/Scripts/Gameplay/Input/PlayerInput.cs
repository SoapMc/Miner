using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using Miner.Management;

namespace Miner.Gameplay
{
    public class PlayerInput : MonoBehaviour
    {
        private Controls _controls = null;

        public float HorizontalMove { get; private set; }
        public float VerticalMove { get; private set; }
        public Action RightMoveKeyPressed { get; private set; }
        public Action RightMoveKeyUp { get; set; }
        public Action LeftMoveKeyPressed { get; set; }
        public Action LeftMoveKeyUp { get; set; }
        public Action UpMoveKeyPressed { get; set; }
        public Action UpMoveKeyUp { get; set; }
        public Action DownMoveKeyPressed { get; set; }
        public Action DownMoveKeyUp { get; set; }
        public Action InventoryViewKeyPressed { get; set; }
        public Action InventoryViewKeyUp { get; set; }
        public Action ConfirmKeyPressed { get; set; }
        public Action NextKeyPressed { get; set; }
        public Action PreviousKeyPressed { get; set; }
        public Action UseKeyPressed { get; set; }
        public Action CancelKeyPressed { get; set; }
        public Action CancelKeyHold { get; set; }

        private void OnInventoryPerformed(InputAction.CallbackContext context)
        {
            InventoryViewKeyPressed?.Invoke();
        }

        private void OnCancelPerformed(InputAction.CallbackContext context)
        {
            CancelKeyPressed?.Invoke();
        }

        private void OnConfirmPerformed(InputAction.CallbackContext context)
        {
            ConfirmKeyPressed?.Invoke();
        }

        private void OnUsePerformed(InputAction.CallbackContext context)
        {
            UseKeyPressed?.Invoke();
        }

        private void OnPreviousPerformed(InputAction.CallbackContext context)
        {
            PreviousKeyPressed?.Invoke();
        }

        private void OnNextPerformed(InputAction.CallbackContext context)
        {
            NextKeyPressed?.Invoke();
        }

        private void Awake()
        {
            if(_controls == null)
                _controls = new Management.Controls();
        }

        private void OnEnable()
        {
            _controls.Player.Cancel.performed += OnCancelPerformed;
            _controls.Player.Inventory.performed += OnInventoryPerformed;
            _controls.Player.Confirm.performed += OnConfirmPerformed;
            _controls.Player.Use.performed += OnUsePerformed;
            _controls.Player.Previous.performed += OnPreviousPerformed;
            _controls.Player.Next.performed += OnNextPerformed;
            _controls.Enable();
        }

        private void Update()
        {
            Vector2 movement = _controls.Player.Movement.ReadValue<Vector2>();
            HorizontalMove = movement.x;
            VerticalMove = movement.y;
        }

        private void OnDisable()
        {
            _controls.Player.Cancel.performed -= OnCancelPerformed;
            _controls.Player.Inventory.performed -= OnInventoryPerformed;
            _controls.Player.Confirm.performed -= OnConfirmPerformed;
            _controls.Player.Use.performed -= OnUsePerformed;
            _controls.Player.Previous.performed -= OnPreviousPerformed;
            _controls.Player.Next.performed -= OnNextPerformed;
        }
    }
}