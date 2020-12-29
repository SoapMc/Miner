using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using Miner.FX;
using Miner.UI;
using Miner.Management;
using UnityEngine.InputSystem;

namespace Miner.Gameplay
{
    [RequireComponent(typeof(PlayerRaycaster), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Resources")]
        [SerializeField] private IntReference _playerEnginePower = null;
        [SerializeField] private Vector2IntReference _gridPosition = null;
        [SerializeField] private FloatReference _drillSharpness = null;
        [SerializeField] private IntReference _playerCargoMass = null;
        [SerializeField] private Window _inventoryWindowPrefab = null;

        [Header("Events")]
        [SerializeField] private GameEvent _tryDig = null;
        [SerializeField] private GameEvent _triggerInteraction = null;
        [SerializeField] private GameEvent _chooseNextUsableItem = null;
        [SerializeField] private GameEvent _choosePreviousUsableItem = null;
        [SerializeField] private GameEvent _useItemRequest = null;
        [SerializeField] private GameEvent _createWindow = null;
        [SerializeField] private GameEvent _openMainMenu = null;
        [SerializeField] private GameEvent _playerRespawned = null;

        private PlayerInput _input = null;
        private SpriteRenderer _sprite = null;
        private Animator _animator = null;
        private PlayerRaycaster _raycaster = null;
        private PlayerRocketController _rocketController = null;
        private Rigidbody2D _rigidbody = null;
        private bool _isFacingRight = true;

        public bool Locked { get; set; }

        public bool IsFacingRight
        {
            get => _isFacingRight;
            set
            {
                if (_animator.GetBool("Flip") == true) return;

                if (_isFacingRight != value)
                {
                    _animator.SetBool("Flip", true);
                }
                
                _isFacingRight = value;
            }
        }

        public void Flip()
        {
            _sprite.flipX = !_sprite.flipX;
            _animator.SetBool("Flip", false);
        }

        #region EVENT RESPONSES  

        public void OnPlayerKilled()
        {
            enabled = false;
            _rocketController.Locked = true;
        }

        public void OnRespawnPlayer()
        {
            enabled = true;
            _rocketController.Locked = false;
            _playerRespawned.Raise();
        }

        public void OnMassChanged(int oldMass, int newMass)
        {
            _rigidbody.mass += (newMass - oldMass);
        }
        
        private void OnConfirmKeyPressed()
        {
            if(!Locked && Time.timeScale > 0f)
                _triggerInteraction.Raise();
        }

        private void OnPreviousKeyPressed()
        {
            _choosePreviousUsableItem.Raise();
        }

        private void OnNextKeyPressed()
        {
            _chooseNextUsableItem.Raise();
        }

        private void OnUseKeyPressed()
        {
            if (!Locked && Time.timeScale > 0f)
                _useItemRequest.Raise();
        }

        private void OnInventoryKeyPressed()
        {
            if (!Locked)
                _createWindow.Raise(new CreateWindowEA(_inventoryWindowPrefab));
        }

        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _raycaster = GetComponent<PlayerRaycaster>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
            _input = GetComponent<PlayerInput>();
            _rocketController = GetComponentInChildren<PlayerRocketController>();
            
            _playerCargoMass.ValueChanged += OnMassChanged;

            _input.ConfirmKeyPressed += OnConfirmKeyPressed;
            _input.PreviousKeyPressed += OnPreviousKeyPressed;
            _input.NextKeyPressed += OnNextKeyPressed;
            _input.UseKeyPressed += OnUseKeyPressed;
            _input.InventoryViewKeyPressed += OnInventoryKeyPressed;
        }

        private void OnEnable()
        {
            Locked = false;
        }

        private void Update()
        {
            
            if (_rigidbody.velocity.x > 0.1f)
                IsFacingRight = true;
            else if(_rigidbody.velocity.x < -0.1f)
                IsFacingRight = false;

            if (Locked) return;

            if (_input.VerticalMove >= 0.15f)
                _rocketController.Enabled = true;
            else
                _rocketController.Enabled = false;

            _raycaster.UpdateRaycasts();
            if(_raycaster.IsGrounded == true)
            {
                if(_input.VerticalMove < -0.6f) //dig down
                {
                    _tryDig.Raise(new TryDigEA(_gridPosition + Vector2Int.down, _drillSharpness.Value, 1, transform.position, EDigDirection.Down));
                }
                else if (_raycaster.CanDigRight && _input.HorizontalMove > 0.8f) //dig right
                {
                    _tryDig.Raise(new TryDigEA(_gridPosition + Vector2Int.right, _drillSharpness.Value, 1, transform.position, EDigDirection.Right));
                }
                else if (_raycaster.CanDigLeft && _input.HorizontalMove < -0.8f) //dig left
                {
                    _tryDig.Raise(new TryDigEA(_gridPosition + Vector2Int.left, _drillSharpness.Value, 1, transform.position, EDigDirection.Left));
                }
            }
        }

        private void FixedUpdate()
        {
            if (Locked) return;
            _rigidbody.AddForce(new Vector2(_input.HorizontalMove * _playerEnginePower.Value, Mathf.Clamp(_input.VerticalMove * 2 * _playerEnginePower.Value, 0f, float.MaxValue)));
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _rigidbody.simulated = true;
            Locked = true;
        }

        private void OnDestroy()
        {
            _playerCargoMass.ValueChanged -= OnMassChanged;

            _input.ConfirmKeyPressed -= OnConfirmKeyPressed;
            _input.PreviousKeyPressed -= OnPreviousKeyPressed;
            _input.NextKeyPressed -= OnNextKeyPressed;
            _input.UseKeyPressed -= OnUseKeyPressed;
            _input.InventoryViewKeyPressed -= OnInventoryKeyPressed;
        }

        #endregion
    }
}